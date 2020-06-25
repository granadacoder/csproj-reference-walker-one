using System;

namespace MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration
{
    /// <summary>
    /// Scalar properties
    /// </summary>
    public partial class VerticalConfigurationEntryEntity
    {
        public int VerticalConfigurationEntryKey { get; set; }

        public int ParentVerticalConfigurationCategoryKey { get; set; }

        public string KeyName { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTimeOffset UpdateDateUtc { get; set; }
    }
}