using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MyCompany.MyExamples.ProjectParser.Domain.Entities;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework.Contexts;
using MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework
{
    public class MyChildEntityFrameworkDomainDataLayer : IMyChildDomainData
    {
        public const string ErrorMessageILoggerFactoryWrapperIsNull = "ILoggerFactoryWrapper is null";
        public const string ErrorMessageBoardGamesDbContextIsNull = "BoardGamesDbContext is null";
        public const string ErrorMsgPrimaryEntityNotFound = "MyChildEntity not found. (MyChildKey='{0}')";
        public const string ErrorMsgExpectedSaveChangesAsyncRowCount = "SaveChangesAsync expected return value was not equal to 1. (SaveChangesAsync.ReturnValue='{0}')";
        public const int ExpectedAddSingleOrUpdateSingleOrDeleteSingleSaveChangesAsyncRowCount = 1;
        private readonly ILogger<MyChildEntityFrameworkDomainDataLayer> logger;
        private readonly EfPlaygroundDbContext entityDbContext;

        public MyChildEntityFrameworkDomainDataLayer(ILoggerFactory loggerFactory, EfPlaygroundDbContext context)
        {
            if (null == loggerFactory)
            {
                throw new ArgumentNullException(ErrorMessageILoggerFactoryWrapperIsNull, (Exception)null);
            }

            this.logger = loggerFactory.CreateLogger<MyChildEntityFrameworkDomainDataLayer>();
            this.entityDbContext = context ?? throw new ArgumentNullException(ErrorMessageBoardGamesDbContextIsNull, (Exception)null);
        }

        public async Task<IEnumerable<MyChildEntity>> GetAllAsync(CancellationToken token)
        {
            List<MyChildEntity> returnItems = await this.entityDbContext.MyChilds.AsNoTracking().ToListAsync(token);
            return returnItems;
        }

        public async Task<MyChildEntity> GetSingleAsync(Guid keyValue, CancellationToken token)
        {
            MyChildEntity returnItem = await this.entityDbContext.MyChilds.FindAsync(new object[] { keyValue }, token);
            return returnItem;
        }

        public async Task<MyChildEntity> AddAsync(MyChildEntity entity, CancellationToken token)
        {
            if (entity.UpdateDateStamp == DateTime.MinValue)
            {
                entity.UpdateDateStamp = DateTime.Now;
            }

            this.entityDbContext.MyChilds.Add(entity);
            int saveChangesAsyncValue = await this.entityDbContext.SaveChangesAsync(token);
            if (ExpectedAddSingleOrUpdateSingleOrDeleteSingleSaveChangesAsyncRowCount != saveChangesAsyncValue)
            {
                throw new ArgumentOutOfRangeException(string.Format(ErrorMsgExpectedSaveChangesAsyncRowCount, saveChangesAsyncValue), (Exception)null);
            }

            return entity;
        }

        public async Task<MyChildEntity> UpdateAsync(MyChildEntity entity, CancellationToken token)
        {
            int saveChangesAsyncValue = 0;
            MyChildEntity foundEntity = await this.entityDbContext.MyChilds.FirstOrDefaultAsync(item => item.MyChildKey == entity.MyChildKey, token);
            if (null != foundEntity)
            {
                foundEntity.MyParentUuidFk = entity.MyParentUuidFk;
                foundEntity.MyChildName = entity.MyChildName;
                foundEntity.MyChildMagicStatus = entity.MyChildMagicStatus;
                foundEntity.FavoriteColor = entity.FavoriteColor;
                foundEntity.UpdateDateStamp = entity.UpdateDateStamp == DateTime.MinValue ? DateTime.Now : entity.UpdateDateStamp;
                this.entityDbContext.Entry(foundEntity).State = EntityState.Modified;
                saveChangesAsyncValue = await this.entityDbContext.SaveChangesAsync(token);
                if (ExpectedAddSingleOrUpdateSingleOrDeleteSingleSaveChangesAsyncRowCount != saveChangesAsyncValue)
                {
                    throw new ArgumentOutOfRangeException(string.Format(ErrorMsgExpectedSaveChangesAsyncRowCount, saveChangesAsyncValue), (Exception)null);
                }
            }
            else
            {
                ArgumentOutOfRangeException argEx = new ArgumentOutOfRangeException(string.Format(ErrorMsgPrimaryEntityNotFound, entity.MyChildKey), (Exception)null);
                this.logger.LogError(argEx.Message, argEx);
                throw argEx;
            }

            return foundEntity;
        }

        public async Task<int> DeleteAsync(Guid keyValue, CancellationToken token)
        {
            int returnValue = 0;
            MyChildEntity foundEntity = await this.entityDbContext.MyChilds.FirstOrDefaultAsync(item => item.MyChildKey == keyValue, token);
            if (null != foundEntity)
            {
                this.entityDbContext.Remove(foundEntity);
                returnValue = await this.entityDbContext.SaveChangesAsync(token);
                if (ExpectedAddSingleOrUpdateSingleOrDeleteSingleSaveChangesAsyncRowCount != returnValue)
                {
                    throw new ArgumentOutOfRangeException(string.Format(ErrorMsgExpectedSaveChangesAsyncRowCount, returnValue), (Exception)null);
                }
            }
            else
            {
                ArgumentOutOfRangeException argEx = new ArgumentOutOfRangeException(string.Format(ErrorMsgPrimaryEntityNotFound, keyValue), (Exception)null);
                this.logger.LogError(argEx.Message, argEx);
                throw argEx;
            }

            return returnValue;
        }
    }
}
