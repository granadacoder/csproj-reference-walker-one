namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps.Constants
{
    public static class SchemaNames
    {
#if (NETCOREAPP2_1 || NETSTANDARD2_0)
        public const string DefaultSchemaName = "MYORACLESCHEMAONE"; /* Oracle (??) It seems to require ALL-CAPS.  In Oracle, Users and Schemas are "the same". */
#endif

#if (NETCOREAPP3_1 || NETSTANDARD2_1)
        ///public const string DefaultSchemaName = "dbo"; /* Sql Server */
        
        ///public const string DefaultSchemaName = "public"; /* PostGres */
        ///
        public const string DefaultSchemaName = "mysqlschema1"; /* MySql */
        
#endif
    }
}
