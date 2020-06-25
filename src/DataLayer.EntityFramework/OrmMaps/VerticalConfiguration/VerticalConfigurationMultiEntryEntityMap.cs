using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps.VerticalConfiguration
{
    public class VerticalConfigurationMultiEntryEntityMap : IEntityTypeConfiguration<VerticalConfigurationMultiEntryEntity>
    {
        public const string SchemaName = Constants.SchemaNames.DefaultSchemaName;
        public const string TableName = "VERT_CONF_MULTI_ENTRY";

        public void Configure(EntityTypeBuilder<VerticalConfigurationMultiEntryEntity> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(k => k.VerticalConfigurationMultiEntryKey);
            builder.Property(x => x.VerticalConfigurationMultiEntryKey).ValueGeneratedNever();

            builder.HasOne<VerticalConfigurationCategoryEntity>(ent => ent.ParentVerticalConfigurationCategoryEntity)
            .WithMany(par => par.VerticalConfigurationMultiEntries)
            .HasForeignKey(ent => ent.ParentVerticalConfigurationCategoryKey).HasConstraintName("VerticalConfigurationMultiEntry_VerticalConfigurationCategoryKeyFK");
        }
    }
}
