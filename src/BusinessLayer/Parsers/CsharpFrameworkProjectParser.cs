using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Microsoft.Extensions.Logging;

using MyCompany.MyExamples.ProjectParser.BusinessLayer.Configuration;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking.Interfaces;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.Parsers.Interfaces;
using MyCompany.MyExamples.ProjectParser.Domain.Dtos;
using MyCompany.MyExamples.ProjectParser.Domain.Enums;

namespace MyCompany.MyExamples.ProjectParser.BusinessLayer.Parsers
{
    public class CsharpFrameworkProjectParser : ICsharpProjectParser
    {
        public const string ErrorMessageILoggerFactoryWrapperIsNull = "ILoggerFactoryWrapper is null";

        private readonly ILogger<CsharpFrameworkProjectParser> logger;
        private readonly IDependencyWalker dependencyWalker;
        private readonly ParseSettings parseSettings;

        public CsharpFrameworkProjectParser(ILoggerFactory loggerFactory, IDependencyWalker dependencyWalker, ParseSettings parseSettings)
        {
            if (null == loggerFactory)
            {
                throw new ArgumentNullException(ErrorMessageILoggerFactoryWrapperIsNull, (Exception)null);
            }

            this.logger = loggerFactory.CreateLogger<CsharpFrameworkProjectParser>();
            this.dependencyWalker = dependencyWalker ?? throw new ArgumentNullException();
            this.parseSettings = parseSettings ?? throw new ArgumentNullException();
        }

        public void ConvertAssemblyParseResult(AssemblyParseResult input, StringBuilder sb, int nestLevel)
        {
            string leadingWhiteSpace = new string(' ', (nestLevel - 1) * 4);
            string leadingWhiteSpaceTwo = new string(' ', nestLevel * 4);
            sb.Append(leadingWhiteSpace + string.Format("({0}) AssemblyFullName:", input.NestLevel) + input.AssemblyFullName + System.Environment.NewLine);

            if (null != input.ChildrenAssemblyParseResults)
            {
                foreach (AssemblyParseResult childAssemblyParseResult in input.ChildrenAssemblyParseResults)
                {
                    this.ConvertAssemblyParseResult(childAssemblyParseResult, sb, nestLevel + 1);
                }
            }

            if (null != input.MissingOrDiscrepancyAssemblyParseResults)
            {
                foreach (MissingOrDiscrepancyAssemblyParseResult mord in input.MissingOrDiscrepancyAssemblyParseResults)
                {
                    sb.Append(leadingWhiteSpaceTwo + "MissingOrDiscrepancyAssemblyParseResult.AssemblyFullName:" + mord.AssemblyFullName + System.Environment.NewLine);
                }
            }
        }

        public string ConvertCsParseResult(CsParseResult input, StringBuilder sb, int nestLevel)
        {
            if (null == sb)
            {
                sb = new StringBuilder();
            }

            if (nestLevel <= 0)
            {
                nestLevel = 1;
            }

            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            string leadingWhiteSpace = new string(' ', (nestLevel - 1) * 4);

            sb.Append(leadingWhiteSpace + string.Format("({0}) CsProjFileName:", nestLevel) + input.CsProjFileName + System.Environment.NewLine);

            if (null != input.ChildrenCsParseReferences)
            {
                foreach (CsParseReference childRef in input.ChildrenCsParseReferences.OrderBy(x => x.RefType).ThenBy(x => x.ReferenceInclude))
                {
                    sb.Append(leadingWhiteSpace + string.Format("({0}) Reference:", childRef.RefType) + childRef.ReferenceInclude.Trim() + System.Environment.NewLine);
                }
            }

            if (null != input.AssemblyParseResults)
            {
                foreach (AssemblyParseResult assemParseResult in input.AssemblyParseResults)
                {
                    this.ConvertAssemblyParseResult(assemParseResult, sb, nestLevel);
                }
            }

            if (null != input.ChildrenCsParseResults)
            {
                foreach (CsParseResult childResult in input.ChildrenCsParseResults)
                {
                    this.ConvertCsParseResult(childResult, sb, nestLevel + 1);
                }
            }

            string returnValue = sb.ToString();
            return returnValue;
        }

        public CsParseResult Parse(string fileName, int nestLevel)
        {
            string exactPath = Path.GetFullPath(fileName);

            CsParseResult returnItem = new CsParseResult() { CsProjFileName = exactPath, NestLevel = nestLevel };

            XDocument xDoc = XDocument.Load(fileName);
            XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

            ////References "By DLL (file)"
            IEnumerable<CsParseReference> list1 = from list in xDoc.Descendants(ns + "ItemGroup")
                                                  from item in list.Elements(ns + "Reference")                                                       /* where item.Element(ns + "HintPath") != null */
                                                  select new CsParseReference
                                                  {
                                                      ReferenceInclude = item.Attribute("Include").Value,
                                                      RefType = (item.Element(ns + "HintPath") == null) ? RefTypeEnum.CompiledDllInGac : RefTypeEnum.CompiledDll,
                                                      HintPath = (item.Element(ns + "HintPath") == null) ? string.Empty : item.Element(ns + "HintPath").Value
                                                  };

            IEnumerable<CsParseReference> list1Filtered = list1.Where(r => !this.parseSettings.IgnorePrefixes.Any(ig => r.ReferenceInclude.StartsWith(ig)));

            list1Filtered.ToList().ForEach(item => returnItem.ChildrenCsParseReferences.Add(item));

            ////References "By Project"
            IEnumerable<CsParseReference> list2 = from list in xDoc.Descendants(ns + "ItemGroup")
                                                  from item in list.Elements(ns + "ProjectReference")
                                                  where
                                                  item.Element(ns + "Project") != null
                                                  select new CsParseReference
                                                  {
                                                      ReferenceInclude = item.Attribute("Include").Value,
                                                      RefType = RefTypeEnum.ProjectReference,
                                                      ProjectGuid = item.Element(ns + "Project").Value
                                                  };

            IEnumerable<CsParseReference> list2Filter = list2.Where(r => !this.parseSettings.IgnorePrefixes.Any(ig => r.ReferenceInclude.StartsWith(ig)));
            list2Filter.ToList().ForEach(item => returnItem.ChildrenCsParseReferences.Add(item));

            foreach (CsParseReference x in returnItem.ChildrenCsParseReferences.Where(csref => csref.RefType == RefTypeEnum.ProjectReference))
            {
                string childFullPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(returnItem.CsProjFileName), x.ReferenceInclude);

                string shortName = System.IO.Path.GetFileName(childFullPath);

                bool ignoreIt = this.parseSettings.IgnorePrefixes.Any(ig => shortName.StartsWith(ig));

                if (!ignoreIt)
                {
                    if (System.IO.File.Exists(childFullPath))
                    {
                        CsParseResult childResult = this.Parse(childFullPath, returnItem.NestLevel + 1);
                        returnItem.ChildrenCsParseResults.Add(childResult);
                    }
                }
            }

            foreach (CsParseReference x in returnItem.ChildrenCsParseReferences.Where(csref => csref.RefType == RefTypeEnum.CompiledDll))
            {
                string baseDirectory = System.IO.Path.GetDirectoryName(returnItem.CsProjFileName);

                if (!string.IsNullOrEmpty(x.HintPath))
                {
                    string shortDllName = System.IO.Path.GetFileName(x.HintPath);

                    bool ignoreIt = this.parseSettings.IgnorePrefixes.Any(ig => shortDllName.StartsWith(ig));

                    if (!ignoreIt)
                    {
                        IEnumerable<string> fileNames = Directory.EnumerateFiles(baseDirectory, shortDllName, SearchOption.AllDirectories);
                        foreach (string f in fileNames)
                        {
                            string assemblyFullFileName = f;
                            AssemblyParseResult val = this.dependencyWalker.PerformReferenceAnalysis(assemblyFullFileName);
                            returnItem.AssemblyParseResults.Add(val);
                            break;
                        }
                    }
                }
            }

            return returnItem;
        }
    }
}