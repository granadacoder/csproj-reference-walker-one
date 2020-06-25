using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyExamples.ProjectParser.Domain.Dtos
{
    [System.Diagnostics.DebuggerDisplay("AssemblyFullName='{AssemblyFullName}', NestLevel='{NestLevel}'")]
    public class AssemblyParseResult
    {
        public AssemblyParseResult()
        {
            this.ChildrenAssemblyParseResults = new List<AssemblyParseResult>();
            this.MissingOrDiscrepancyAssemblyParseResults = new List<MissingOrDiscrepancyAssemblyParseResult>();
        }

        public string AssemblyFullName { get; set; }

        public string AssemblyShortName { get; set; }

        public int NestLevel { get; set; }

        public ICollection<AssemblyParseResult> ChildrenAssemblyParseResults { get; set; }

        public ICollection<MissingOrDiscrepancyAssemblyParseResult> MissingOrDiscrepancyAssemblyParseResults { get; set; }
    }
}
