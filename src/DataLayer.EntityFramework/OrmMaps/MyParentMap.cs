using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyExamples.ProjectParser.Domain.Entities;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces.Constants.StringLengths;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps
{
    public class MyParentMap : IEntityTypeConfiguration<MyParentEntity>
    {
        public const string SchemaName = Constants.SchemaNames.DefaultSchemaName;
        public const string TableName = "MY_PARENT";

        public void Configure(EntityTypeBuilder<MyParentEntity> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(k => k.MyParentKey);
            builder.Property(cnpk => cnpk.MyParentKey).HasColumnName("MY_PARENT_UUID");
            builder.Property(dvsql => dvsql.MyParentKey).HasDefaultValueSql(Constants.SqlKeyWords.NewSequentialId);
            builder.Property(req => req.MyParentKey).IsRequired();

            builder.Property(cn => cn.MyParentName).HasColumnName("MY_PARENT_NAME");
            builder.Property(ml => ml.MyParentName).HasMaxLength(MyParentValidationStringLengthConstants.MyParentNameMaxLength);
            builder.Property(req => req.MyParentName).IsRequired();

            builder.Property(cn => cn.UpdateDateStamp).HasColumnName("UPDATE_DATE_TS");
            builder.Property(req => req.UpdateDateStamp).IsRequired();

            builder.HasIndex(ind => new { ind.MyParentName }).IsUnique(true);
        }
    }
}
