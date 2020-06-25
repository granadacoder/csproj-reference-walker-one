using System;
using System.Collections.Generic;
using System.Text;

using MyCompany.MyExamples.ProjectParser.Domain.Dtos;

namespace MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking.Interfaces
{
    public interface IDependencyWalker
    {
        AssemblyParseResult PerformReferenceAnalysis(string assemblyFullFileName);
    }
}
