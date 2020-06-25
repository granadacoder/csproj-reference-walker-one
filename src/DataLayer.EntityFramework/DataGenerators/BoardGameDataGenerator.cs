using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MyCompany.MyExamples.ProjectParser.Domain.Entities;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.Contexts;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.DataGenerators
{
    public class BoardGameDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EfPlaygroundDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<EfPlaygroundDbContext>>(), serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>()))
            {
                // Look for any board games already in database.
                if (context.BoardGames.Any())
                {
                    return;   // Database has been seeded
                }

                context.BoardGames.AddRange(
                    new BoardGameEntity
                    {
                        BoardGameKey = 1,
                        Title = "Candy Land",
                        PublishingCompany = "Hasbro",
                        MinPlayers = 2,
                        MaxPlayers = 4
                    },
                    new BoardGameEntity
                    {
                        BoardGameKey = 2,
                        Title = "Sorry!",
                        PublishingCompany = "Hasbro",
                        MinPlayers = 2,
                        MaxPlayers = 4
                    },
                    new BoardGameEntity
                    {
                        BoardGameKey = 3,
                        Title = "Monopoly",
                        PublishingCompany = "Parker Brothers",
                        MinPlayers = 2,
                        MaxPlayers = 5
                    },
                    new BoardGameEntity
                    {
                        BoardGameKey = 4,
                        Title = "King of Tokyo",
                        PublishingCompany = "Iello",
                        MinPlayers = 2,
                        MaxPlayers = 6
                    },
                    new BoardGameEntity
                    {
                        BoardGameKey = 5,
                        Title = "Guillotine",
                        PublishingCompany = "Avalon",
                        MinPlayers = 2,
                        MaxPlayers = 6
                    });

                context.SaveChanges();
            }
        }
    }
}
