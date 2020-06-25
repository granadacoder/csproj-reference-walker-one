using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyExamples.ProjectParser.Domain.Entities;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces.Constants.StringLengths;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps
{
    public class MyChildMap : IEntityTypeConfiguration<MyChildEntity>
    {
        public const string SchemaName = Constants.SchemaNames.DefaultSchemaName;
        public const string TableName = "MY_CHILD";

        public void Configure(EntityTypeBuilder<MyChildEntity> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(k => k.MyChildKey);
            builder.Property(cnpk => cnpk.MyChildKey).HasColumnName("MY_CHILD_UUID");
            builder.Property(dvsql => dvsql.MyChildKey).HasDefaultValueSql(Constants.SqlKeyWords.NewSequentialId);
            builder.Property(req => req.MyChildKey).IsRequired();

            builder.Property(cn => cn.MyParentUuidFk).HasColumnName("MY_PARENT_UUID");
            builder.Property(req => req.MyParentUuidFk).IsRequired();

            builder.Property(cn => cn.MyChildName).HasColumnName("MY_CHILD_NAME");
            builder.Property(ml => ml.MyChildName).HasMaxLength(MyChildValidationStringLengthConstants.MyChildNameMaxLength);
            builder.Property(req => req.MyChildName).IsRequired();

            builder.Property(cn => cn.MyChildMagicStatus).HasColumnName("MY_CHILD_MAGIC_STATUS");
            builder.Property(req => req.MyChildMagicStatus).IsRequired();

            builder.Property(cn => cn.UpdateDateStamp).HasColumnName("UPDATE_DATE_TS");
            builder.Property(req => req.UpdateDateStamp).IsRequired();

            builder.Property(cn => cn.FavoriteColor).HasColumnName("FAVORITE_COLOR_CODE");
            builder.Property(req => req.FavoriteColor).IsRequired();
            builder.Property(hc => hc.FavoriteColor).HasConversion<int>();
            ////builder.Property(dv => dv.FavoriteColor).HasDefaultValue((int)FavoriteColorEnum.Unknown);
            ////builder.Ignore(ig => ig.DirectWorkStepTypeCode);

            /*
            If you just want to enforce uniqueness on a column, define a unique index rather than an alternate key (see Indexes). In EF, alternate keys are read-only and provide additional semantics over unique indexes because they can be used as the target of a foreign key.
            */
            /* postgres sees this an column overlap */
            ////builder.HasIndex(ind => new { ind.MyParentUuidFk, ind.MyChildName }).IsUnique(true);

            ////builder.HasAlternateKey(uniq => new { uniq.MyParentUuidFk, uniq.MyChildName });

            builder.HasOne<MyParentEntity>(e => e.ParentMyParent)
            .WithMany(d => d.MyChilds)
            .HasForeignKey(e => e.MyParentUuidFk).HasConstraintName("MYCHD_TO_MY_PAR_FK_CONST");
        }
    }
}