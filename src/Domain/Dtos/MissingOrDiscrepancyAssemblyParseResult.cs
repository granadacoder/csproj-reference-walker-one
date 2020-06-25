using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyExamples.ProjectParser.Domain.Dtos
{
    [System.Diagnostics.DebuggerDisplay("AssemblyFullName='{AssemblyFullName}', FirstLayerException='{FirstLayerException.Message}'")]
    public class MissingOrDiscrepancyAssemblyParseResult
    {
        public string AssemblyFullName { get; set; }

        public string AssemblyShortName { get; set; }

        public string ExtraInformation { get; set; }

        public int NestLevel { get; set; }

        public Exception FirstLayerException { get; set; }

        public Exception SecondLayerException { get; set; }
    }
}
