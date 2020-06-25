using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyExamples.ProjectParser.Domain.Entities.VerticalConfiguration;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.Contexts;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.DataGenerators
{
    public class VerticalConfigurationDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EfPlaygroundDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<EfPlaygroundDbContext>>(), serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>()))
            {
                // Look for any board games already in database.
                if (context.VerticalConfigurationCategories.Any())
                {
                    return;   // Database has been seeded
                }

                context.VerticalConfigurationCategories.AddRange(
                    new VerticalConfigurationCategoryEntity
                    {
                        VerticalConfigurationCategoryKey = 101,
                        VerticalConfigurationCategoryName = "General Settings",
                        UpdateDateUtc = DateTimeOffset.Now,
                        VerticalConfigurationEntryEntities = new List<VerticalConfigurationEntryEntity>
                        {
                            new VerticalConfigurationEntryEntity() { VerticalConfigurationEntryKey = 10101,  ParentVerticalConfigurationCategoryKey = 101, KeyName = "MinimumBirthDate", Value = "01/01/1890", UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationEntryEntity() { VerticalConfigurationEntryKey = 10102,  ParentVerticalConfigurationCategoryKey = 101, KeyName = "MinThreshold", Value = "-100", UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationEntryEntity() { VerticalConfigurationEntryKey = 10103,  ParentVerticalConfigurationCategoryKey = 101, KeyName = "MaxThreshold", Value = "100", UpdateDateUtc = DateTimeOffset.Now }
                        }
                    },
                    new VerticalConfigurationCategoryEntity
                    {
                        VerticalConfigurationCategoryKey = 201,
                        VerticalConfigurationCategoryName = "ZipCode Settings",
                        UpdateDateUtc = DateTimeOffset.Now,
                        VerticalConfigurationMultiEntries = new List<VerticalConfigurationMultiEntryEntity>
                        {
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 20101,  ParentVerticalConfigurationCategoryKey = 201, Value = "11111", Sequence = 1, UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 20102,  ParentVerticalConfigurationCategoryKey = 201, Value = "22222", Sequence = 2, UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 20109,  ParentVerticalConfigurationCategoryKey = 201, Value = "99999", Sequence = 3, UpdateDateUtc = DateTimeOffset.Now }
                        }
                    },
                    new VerticalConfigurationCategoryEntity
                    {
                        VerticalConfigurationCategoryKey = 301,
                        VerticalConfigurationCategoryName = "Favorite Colors",
                        UpdateDateUtc = DateTimeOffset.Now,
                        VerticalConfigurationMultiEntries = new List<VerticalConfigurationMultiEntryEntity>
                        {
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 30101,  ParentVerticalConfigurationCategoryKey = 301, Value = "Red", Sequence = 1, UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 30102,  ParentVerticalConfigurationCategoryKey = 301, Value = "Yellow", Sequence = 2, UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 30103,  ParentVerticalConfigurationCategoryKey = 301, Value = "Black", Sequence = 3, UpdateDateUtc = DateTimeOffset.Now },
                            new VerticalConfigurationMultiEntryEntity() { VerticalConfigurationMultiEntryKey = 30104,  ParentVerticalConfigurationCategoryKey = 301, Value = "White", Sequence = 3, UpdateDateUtc = DateTimeOffset.Now }
                        }
                    });

                context.SaveChanges();
            }
        }
    }
}
