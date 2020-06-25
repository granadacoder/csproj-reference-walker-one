using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MyCompany.MyExamples.ProjectParser.Domain.Entities;
using MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.OrmMaps.VerticalConfiguration;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.Contexts
{
    public class EfPlaygroundDbContext : DbContext
    {
        public const string ErrorMessageILoggerFactoryWrapperIsNull = "ILoggerFactoryWrapper is null";

        private readonly ILoggerFactory loggerFactory;

        public EfPlaygroundDbContext(DbContextOptions<EfPlaygroundDbContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(ErrorMessageILoggerFactoryWrapperIsNull, (Exception)null);

            this.Database.EnsureCreated();
        }

        //////public BoardGamesDbContext(DbContextOptions options) : base(options)
        //////{
        //////}

        public DbSet<BoardGameEntity> BoardGames { get; set; }

        public DbSet<MyParentEntity> MyParents { get; set; }

        public DbSet<MyChildEntity> MyChilds { get; set; }

        public DbSet<VerticalConfigurationCategoryEntity> VerticalConfigurationCategories { get; set; }

        public DbSet<VerticalConfigurationEntryEntity> VerticalConfigurationEntries { get; set; }

        public DbSet<VerticalConfigurationMultiEntryEntity> VerticalConfigurationMultiEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BoardGameMap());
            modelBuilder.ApplyConfiguration(new MyParentMap());
            modelBuilder.ApplyConfiguration(new MyChildMap());

            modelBuilder.ApplyConfiguration(new VerticalConfigurationCategoryEntityMap());
            modelBuilder.ApplyConfiguration(new VerticalConfigurationEntryEntityMap());
            modelBuilder.ApplyConfiguration(new VerticalConfigurationMultiEntryEntityMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Allow null in case you are using an IDesignTimeDbContextFactory
            if (this.loggerFactory != null) /* this.loggerFactory is Microsoft.Extensions.Logging.ILoggerFactory */
            {
                ////if (System.Diagnostics.Debugger.IsAttached)
                ////{
                ////    //// Probably shouldn't log sql statements in production reminder
                optionsBuilder.UseLoggerFactory(this.loggerFactory);
                ////}
            }
        }
    }
}
