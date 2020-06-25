using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps.VerticalConfiguration
{
    public class VerticalConfigurationEntryEntityMap : IEntityTypeConfiguration<VerticalConfigurationEntryEntity>
    {
        public const string SchemaName = Constants.SchemaNames.DefaultSchemaName;
        public const string TableName = "VERT_CONF_ENTRY";

        public void Configure(EntityTypeBuilder<VerticalConfigurationEntryEntity> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(k => k.VerticalConfigurationEntryKey);
            builder.Property(x => x.VerticalConfigurationEntryKey).ValueGeneratedNever();

            builder.Property(cn => cn.KeyName).HasColumnName("VERT_CONF_ENTRY_KEY_NAME");
            builder.Property(req => req.KeyName).IsRequired();

            builder.Property(cn => cn.Value).HasColumnName("VERT_CONF_ENTRY_VALUE");
            builder.Property(req => req.Value).IsRequired();

            builder.Property(cn => cn.UpdateDateUtc).HasColumnName("VERT_CONF_CAT_UPDATE_TS");
            builder.Property(req => req.UpdateDateUtc).IsRequired();




            builder.HasOne<VerticalConfigurationCategoryEntity>(ent => ent.ParentVerticalConfigurationCategoryEntity)
            .WithMany(par => par.VerticalConfigurationEntryEntities)
            .HasForeignKey(ent => ent.ParentVerticalConfigurationCategoryKey).HasConstraintName("VerticalConfigurationEntry_VerticalConfigurationCategoryKeyFK");
        }
    }
}
