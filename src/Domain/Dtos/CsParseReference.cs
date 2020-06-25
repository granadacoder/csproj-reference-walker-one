using System;
using System.Collections.Generic;
using System.Text;

using MyCompany.MyExamples.ProjectParser.Domain.Enums;

namespace MyCompany.MyExamples.ProjectParser.Domain.Dtos
{
    [System.Diagnostics.DebuggerDisplay("ReferenceInclude='{ReferenceInclude}', RefType='{RefType}'")]
    public class CsParseReference
    {
        public string ReferenceInclude { get; set; }

        public RefTypeEnum RefType { get; set; }

        public string ProjectGuid { get; set; }

        public string HintPath { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(ReferenceInclude)}={ReferenceInclude}, {nameof(RefType)}={RefType.ToString()}, {nameof(ProjectGuid)}={ProjectGuid}, {nameof(HintPath)}={HintPath}}}";
        }
    }
}
