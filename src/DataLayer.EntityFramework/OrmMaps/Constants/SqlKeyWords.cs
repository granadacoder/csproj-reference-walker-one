namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps.Constants
{
    public static class SqlKeyWords
    {
        public const string CurrentTimeStamp = "CURRENT_TIMESTAMP";

#if (NETCOREAPP2_1 || NETSTANDARD2_0)
        public const string CurrentUserDefault = "USER"; /* Oracle does not work with CURRENT_USER. :( */
        public const string NewSequentialId = "SYS_GUID()";
#endif

#if (NETCOREAPP3_1 || NETSTANDARD2_1)
        public const string CurrentUserDefault = "CURRENT_USER"; /* SqlServer, PostGres */

        public const string NewSequentialId = "newsequentialid()"; /* Sql Server */

        /* CREATE EXTENSION IF NOT EXISTS "uuid-ossp"; */
        //public const string NewSequentialId = "uuid_generate_v4()";
#endif
    }
}
