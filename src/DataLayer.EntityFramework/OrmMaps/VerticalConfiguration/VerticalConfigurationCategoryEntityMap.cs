using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps.VerticalConfiguration
{
    public class VerticalConfigurationCategoryEntityMap : IEntityTypeConfiguration<VerticalConfigurationCategoryEntity>
    {
        public const string SchemaName = Constants.SchemaNames.DefaultSchemaName;
        public const string TableName = "VERT_CONF_CATEGORY";

        public void Configure(EntityTypeBuilder<VerticalConfigurationCategoryEntity> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(k => k.VerticalConfigurationCategoryKey);
            builder.Property(x => x.VerticalConfigurationCategoryKey).ValueGeneratedNever();
            builder.Property(cn => cn.VerticalConfigurationCategoryKey).HasColumnName("VERT_CONF_CAT_KEY");
            builder.Property(req => req.VerticalConfigurationCategoryKey).IsRequired();

            builder.Property(cn => cn.VerticalConfigurationCategoryName).HasColumnName("VERT_CONF_CAT_NAME");
            builder.Property(req => req.VerticalConfigurationCategoryName).IsRequired();

            builder.Property(cn => cn.UpdateDateUtc).HasColumnName("VERT_CONF_CAT_UPDATE_TS");
            builder.Property(req => req.VerticalConfigurationCategoryKey).IsRequired();
        }
    }
}
