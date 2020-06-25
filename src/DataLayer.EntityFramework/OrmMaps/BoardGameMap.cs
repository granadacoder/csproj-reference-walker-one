using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyExamples.ProjectParser.Domain.Entities;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps
{
    public class BoardGameMap : IEntityTypeConfiguration<BoardGameEntity>
    {
        public const string SchemaName = Constants.SchemaNames.DefaultSchemaName;
        public const string TableName = "BOARD_GAME_TABLE";

        public void Configure(EntityTypeBuilder<BoardGameEntity> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(k => k.BoardGameKey);
        }
    }
}
