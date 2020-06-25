using System;

namespace MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration
{
    /// <summary>
    /// Scalar properties
    /// </summary>
    public partial class VerticalConfigurationCategoryEntity
    {
        public int VerticalConfigurationCategoryKey { get; set; }

        public string VerticalConfigurationCategoryName { get; set; }

        public DateTimeOffset UpdateDateUtc { get; set; }
    }
}
