using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyCompany.MyExamples.ProjectParser.Domain.Entities;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.Contexts;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework
{
    public class BoardGameEntityFrameworkDataLayer : IBoardGameDataLayer
    {
        public const string ErrorMessageILoggerFactoryWrapperIsNull = "ILoggerFactoryWrapper is null";
        public const string ErrorMessageBoardGamesDbContextIsNull = "BoardGamesDbContext is null";

        private readonly ILogger<BoardGameEntityFrameworkDataLayer> logger;
        private readonly EfPlaygroundDbContext entityDbContext;

        public BoardGameEntityFrameworkDataLayer(ILoggerFactory loggerFactory, EfPlaygroundDbContext context)
        {
            if (null == loggerFactory)
            {
                throw new ArgumentNullException(ErrorMessageILoggerFactoryWrapperIsNull, (Exception)null);
            }

            this.logger = loggerFactory.CreateLogger<BoardGameEntityFrameworkDataLayer>();
            this.entityDbContext = context ?? throw new ArgumentNullException(ErrorMessageBoardGamesDbContextIsNull, (Exception)null);
        }


        public ICollection<BoardGameEntity> BoardGamesGetAll()
        {
            List<BoardGameEntity> returnItems = this.entityDbContext.BoardGames.ToList();
            return returnItems;
        }

        public async Task<ICollection<BoardGameEntity>> BoardGamesGetAllAsync()
        {
            List<BoardGameEntity> returnItems = await this.entityDbContext.BoardGames.ToListAsync();
            return returnItems;
        }

        public async Task<BoardGameEntity> BoardGamesGetSingleAsync(int id)
        {
            BoardGameEntity returnItem = await this.entityDbContext.BoardGames.FindAsync(id);
            return returnItem;
        }

        public async Task<int> AddAsync(BoardGameEntity bge, CancellationToken token)
        {
            this.entityDbContext.BoardGames.Add(bge);
            int returnValue = await this.entityDbContext.SaveChangesAsync(token);
            return returnValue;
        }

        public async Task<int> UpdateAsync(BoardGameEntity bge, CancellationToken token)
        {
            int returnValue = 0;
            var entity = this.entityDbContext.BoardGames.FirstOrDefault(item => item.BoardGameKey == bge.BoardGameKey);

            if (null != entity)
            {
                entity.MinPlayers = bge.MinPlayers;
                entity.MaxPlayers = bge.MaxPlayers;
                entity.PublishingCompany = bge.PublishingCompany;
                entity.Title = bge.Title;
                returnValue = await this.entityDbContext.SaveChangesAsync(token);
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("BoardGameEntity not found. ()", bge.BoardGameKey));
            }

            return returnValue;
        }
    }
}
