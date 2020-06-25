using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Logging;

using MyCompany.MyExamples.ProjectParser.BusinessLayer.Configuration;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking.Interfaces;
using MyCompany.MyExamples.ProjectParser.Domain.Dtos;

namespace MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking
{
    public class DependencyWalker : IDependencyWalker
    {
        public const string ErrorMessageILoggerFactoryWrapperIsNull = "ILoggerFactoryWrapper is null";

        private readonly ILogger<DependencyWalker> logger;
        private readonly ParseSettings parseSettings;

        public DependencyWalker(ILoggerFactory loggerFactory, ParseSettings parseSettings)
        {
            if (null == loggerFactory)
            {
                throw new ArgumentNullException(ErrorMessageILoggerFactoryWrapperIsNull, (Exception)null);
            }

            this.logger = loggerFactory.CreateLogger<DependencyWalker>();
            this.parseSettings = parseSettings ?? throw new ArgumentNullException();
        }

        public AssemblyParseResult PerformReferenceAnalysis()
        {
            AssemblyParseResult returnValue = this.InternalPerformReferenceAnalysis(System.Reflection.Assembly.GetExecutingAssembly(), string.Empty);
            return returnValue;
        }

        public AssemblyParseResult PerformReferenceAnalysis(System.Reflection.Assembly assembly)
        {
            AssemblyParseResult returnValue = this.InternalPerformReferenceAnalysis(assembly, string.Empty);
            return returnValue;
        }

        public AssemblyParseResult PerformReferenceAnalysis(System.Reflection.Assembly assembly, string sourceFolder)
        {
            AssemblyParseResult returnValue = this.InternalPerformReferenceAnalysis(assembly, sourceFolder);
            return returnValue;
        }

        public AssemblyParseResult PerformReferenceAnalysis(string assemblyFullFileName)
        {
            ////System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyFullFileName);

            ////System.Reflection.Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullFileName);

#if (NETCOREAPP2_1 || NETSTANDARD2_0 || NETSTANDARD2_1)
            throw new ArgumentOutOfRangeException("System.Runtime.Loader only available in 3.1");
#endif

#if (NETCOREAPP3_1)

            System.Reflection.Assembly assembly = System.Runtime.Loader.AssemblyLoadContext.Default
                                           .LoadFromAssemblyPath(assemblyFullFileName);

            AssemblyParseResult returnValue = this.InternalPerformReferenceAnalysis(assembly, System.IO.Path.GetDirectoryName(assemblyFullFileName));
            return returnValue;
        
#endif
        }

        private AssemblyParseResult InternalPerformReferenceAnalysis(System.Reflection.Assembly assembly, string sourceFolder)
        {
            StringBuilder builder = new StringBuilder();
            HashSet<string> loadedAssemblies = new HashSet<string>();
            AssemblyParseResult returnItem = this.InternalPerformReferenceAnalysis(assembly, builder, string.Empty, loadedAssemblies, sourceFolder, string.Empty, 1);
            return returnItem;
        }

        private AssemblyParseResult InternalPerformReferenceAnalysis(System.Reflection.Assembly inputAssembly, StringBuilder builder, string leadingWhitespace, HashSet<string> loadedAssemblies, string sourceFolder, string parentAssemblyFriendlyName, int nestLevel)
        {
            AssemblyParseResult returnItem = new AssemblyParseResult();

            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            returnItem.AssemblyFullName = inputAssembly.FullName;
            returnItem.NestLevel = nestLevel;

            builder.Append(leadingWhitespace + inputAssembly.FullName);
            System.Reflection.AssemblyName[] referencedAssemblies = inputAssembly.GetReferencedAssemblies();

            List<System.Reflection.AssemblyName> assembs = new List<System.Reflection.AssemblyName>();
            assembs = referencedAssemblies.ToList().OrderBy(a => a.Name).ToList();

            int count = assembs.Count;

            foreach (System.Reflection.AssemblyName assemblyNameObj in assembs)
            {
                if (loadedAssemblies.Contains(assemblyNameObj.Name))
                {
                    continue;
                }

                loadedAssemblies.Add(assemblyNameObj.Name);
                System.Reflection.Assembly nextAssembly;
                try
                {
                    ////nextAssembly = System.Reflection.Assembly.ReflectionOnlyLoad(assemblyNameObj.FullName);

                    nextAssembly = null;
#if (NETCOREAPP3_1)

                    nextAssembly = System.Runtime.Loader.AssemblyLoadContext.Default
                               .LoadFromAssemblyName(assemblyNameObj);
#endif
                    if (null == nextAssembly)
                    {
                        throw new ArgumentNullException(assemblyNameObj.FullName);
                    }
                }
                catch (Exception ex)
                {
                    MissingOrDiscrepancyAssemblyParseResult mord = new MissingOrDiscrepancyAssemblyParseResult();

                    try
                    {
                        string exmsg = ex.Message;

                        string fullName = assemblyNameObj.FullName;
                        mord.AssemblyFullName = fullName;
                        mord.FirstLayerException = ex;

                        builder.Append(System.Environment.NewLine + leadingWhitespace + "***" + assemblyNameObj.FullName);

                        ////nextAssembly = System.Reflection.Assembly.ReflectionOnlyLoad(assemblyName.Name);
                        if (!string.IsNullOrEmpty(sourceFolder))
                        {
                            string otherFullFileName = System.IO.Path.Combine(sourceFolder, assemblyNameObj.Name + ".dll");
                            nextAssembly = System.Reflection.Assembly.LoadFile(otherFullFileName);

                            if (null != nextAssembly)
                            {
                                if (nextAssembly.FullName != assemblyNameObj.FullName)
                                {
                                    string extra = leadingWhitespace + "FullNameMISMATCH (" + assemblyNameObj.FullName + ") ||||| (" + nextAssembly.FullName + ")";
                                    builder.Append(System.Environment.NewLine + extra);
                                    mord.ExtraInformation = extra;
                                }
                            }
                        }
                        else
                        {
                            nextAssembly = null;
                        }
                    }
                    catch (Exception exx)
                    {
                        mord.SecondLayerException = exx;
                        string exmsg = exx.Message;
                        nextAssembly = null;
                    }

                    bool ignoreIt = this.parseSettings.IgnorePrefixes.Any(ig => mord.AssemblyFullName.StartsWith(ig));
                    if (!ignoreIt)
                    {
                        returnItem.MissingOrDiscrepancyAssemblyParseResults.Add(mord);
                    }
                }

                if (nextAssembly != null)
                {
                    bool ignoreIt = this.parseSettings.IgnorePrefixes.Any(ig => nextAssembly.FullName.StartsWith(ig));

                    if (!ignoreIt)
                    {
                        AssemblyParseResult childResult = this.InternalPerformReferenceAnalysis(nextAssembly, builder, leadingWhitespace + "| ", loadedAssemblies, sourceFolder, inputAssembly.FullName, nestLevel + 1);
                        returnItem.ChildrenAssemblyParseResults.Add(childResult);
                    }
                }
            }

            return returnItem;
        }
    }
}
