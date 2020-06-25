using System.Collections.Generic;

using MyCompany.MyExamples.ProjectParser.Domain.Enums;

namespace MyCompany.MyExamples.ProjectParser.Domain.Dtos
{
    [System.Diagnostics.DebuggerDisplay("CsProjFileName='{CsProjFileName}', NestLevel='{NestLevel}'")]
    public class CsParseResult
    {
        public CsParseResult()
        {
            this.ChildrenCsParseResults = new List<CsParseResult>();
            this.ChildrenCsParseReferences = new List<CsParseReference>();
            this.AssemblyParseResults = new List<AssemblyParseResult>();
        }

        public string CsProjFileName { get; set; }

        public int NestLevel { get; set; }

        public ICollection<CsParseResult> ChildrenCsParseResults { get; set; }

        public ICollection<CsParseReference> ChildrenCsParseReferences { get; set; }

        public ICollection<AssemblyParseResult> AssemblyParseResults { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
