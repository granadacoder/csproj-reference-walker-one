using System.Collections.Generic;

namespace MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration
{
    /// <summary>
    /// Navigation and Other properties
    /// </summary>
    public partial class VerticalConfigurationCategoryEntity
    {
        public VerticalConfigurationCategoryEntity()
        {
            this.VerticalConfigurationEntryEntities = new List<VerticalConfigurationEntryEntity>();
            this.VerticalConfigurationMultiEntries = new List<VerticalConfigurationMultiEntryEntity>();
        }

        public ICollection<VerticalConfigurationEntryEntity> VerticalConfigurationEntryEntities { get; set; }

        public ICollection<VerticalConfigurationMultiEntryEntity> VerticalConfigurationMultiEntries { get; set; }
    }
}
