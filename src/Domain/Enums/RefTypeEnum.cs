namespace MyCompany.MyExamples.ProjectParser.Domain.Enums
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public enum RefTypeEnum
    {
        /// <summary>
        /// unknown default
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// project reference
        /// </summary>
        ProjectReference = 1,

        /// <summary>
        /// copy of file dll
        /// </summary>
        CompiledDll = 2,

        /// <summary>
        /// gac dll
        /// </summary>
        CompiledDllInGac = 3
    }
}
