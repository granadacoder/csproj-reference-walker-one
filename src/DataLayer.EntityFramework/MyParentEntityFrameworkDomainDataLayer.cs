namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.EntityFramework
{
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

    public class MyParentEntityFrameworkDomainDataLayer : IMyParentDomainData
    {
        public const string ErrorMessageILoggerFactoryWrapperIsNull = "ILoggerFactoryWrapper is null";
        public const string ErrorMessageBoardGamesDbContextIsNull = "BoardGamesDbContext is null";
        public const string ErrorMsgPrimaryEntityNotFound = "MyParentEntity not found. (MyParentKey='{0}')";
        public const string ErrorMsgExpectedSaveChangesAsyncRowCount = "SaveChangesAsync expected return value was not equal to 1. (SaveChangesAsync.ReturnValue='{0}')";
        public const int ExpectedAddSingleOrUpdateSingleOrDeleteSingleSaveChangesAsyncRowCount = 1;
        private readonly ILogger<MyParentEntityFrameworkDomainDataLayer> logger;
        private readonly EfPlaygroundDbContext entityDbContext;

        public MyParentEntityFrameworkDomainDataLayer(ILoggerFactory loggerFactory, EfPlaygroundDbContext context)
        {
            if (null == loggerFactory)
            {
                throw new ArgumentNullException(ErrorMessageILoggerFactoryWrapperIsNull, (Exception)null);
            }

            this.logger = loggerFactory.CreateLogger<MyParentEntityFrameworkDomainDataLayer>();
            this.entityDbContext = context ?? throw new ArgumentNullException(ErrorMessageBoardGamesDbContextIsNull, (Exception)null);
        }

        public async Task<IEnumerable<MyParentEntity>> GetAllAsync(CancellationToken token)
        {
            List<MyParentEntity> returnItems = await this.entityDbContext.MyParents.AsNoTracking().ToListAsync(token);
            return returnItems;
        }

        //// ********************************************************

        public async Task<IEnumerable<MyParentEntity>> PerformDemoIQueryableWithReusedPocoObject(TimeSpan cutOffTimeSpan, ICollection<int> magicStatusValues, CancellationToken token)
        {
            DateTime cutOffDate = DateTime.Now.Add(cutOffTimeSpan);

            /* START ANONYMOUS TYPE */

            /*
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'where ([anonymousPerParentMaxUpdateDate].MaxPerParentUpdateDateStamp < __cutOffDate_1)' co
                 not be translated and will be evaluated locally.
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'where ([chd].MyParentUUID == [par].MyParentKey)' could not be translated and will be evalu
                d locally.
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'Any()' could not be translated and will be evaluated locally.
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'where ({from MyChildEntity chd in value(Microsoft.EntityFrameworkCore.Query.Internal.Entit             
             */

            ////var anonymousPerParentMaxUpdateDates = this.entityDbContext.MyChilds.GroupBy(i => i.MyParentUUID).Select(g => new
            ////{
            ////    MyParentUUID = g.Key,
            ////    MaxPerParentUpdateDateStamp = g.Max(row => row.UpdateDateStamp)
            ////});

            ////IQueryable<MyChildEntity> filteredChildren = from chd in this.entityDbContext.MyChilds
            ////                                             join anonymousPerParentMaxUpdateDate in anonymousPerParentMaxUpdateDates
            ////                                             on new { a = chd.UpdateDateStamp, b = chd.MyParentUUID } 
            ////                                             equals 
            ////                                             new { a = anonymousPerParentMaxUpdateDate.MaxPerParentUpdateDateStamp, b = anonymousPerParentMaxUpdateDate.MyParentUUID }
            ////                                             where !magicValues.Contains(chd.MyChildMagicStatus)
            ////                                             && anonymousPerParentMaxUpdateDate.MaxPerParentUpdateDateStamp < cutOffDate
            ////                                             select chd;
            /* END ANONYMOUS TYPE */

            /////* ChildMaxHolder START */

            /////*
            ////    [WRN] The LINQ expression 'where ([perParentMaxUpdateDate].MaxPerParentUpdateDateStamp < __cutOffDate_1)' could not be translated a
            ////    [WRN] The LINQ expression 'where ([chd].MyParentUUID == [par].MyParentKey)' could not be translated and will be evaluated locally.
            ////    [WRN] The LINQ expression 'Any()' could not be translated and will be evaluated locally.
            ////    [WRN] The LINQ expression 'where ({from MyChildEntity chd in 

            //// */

            ////IQueryable<ChildMaxHolder> childMaxHolders = this.entityDbContext.MyChilds.GroupBy(i => i.MyParentUUID).Select(g => new
            ////ChildMaxHolder
            ////{
            ////    MyParentUUID = g.Key,
            ////    MaxChildUpdateDateStamp = g.Max(row => row.UpdateDateStamp)
            ////});

            /////* below using childMaxHolders */
            ////IQueryable<MyChildEntity> filteredChildren = from chd in this.entityDbContext.MyChilds
            ////                                             join perParentMaxUpdateDate in childMaxHolders
            ////                                             on new { a = chd.UpdateDateStamp, b = chd.MyParentUUID }
            ////                                             equals
            ////                                             new { a = perParentMaxUpdateDate.MaxChildUpdateDateStamp, b = perParentMaxUpdateDate.MyParentUUID }
            ////                                             where !magicValues.Contains(chd.MyChildMagicStatus)
            ////                                             && perParentMaxUpdateDate.MaxChildUpdateDateStamp < cutOffDate
            ////                                             select chd;
            /////* ChildMaxHolder END */

            /* WORKING MyChildEntity */
            ////IQueryable<MyChildEntity> pocoPerParentMaxUpdateDates = this.entityDbContext.MyChilds.GroupBy(i => i.MyParentUUID).Select(g => new MyChildEntity
            ////{
            ////    MyParentUUID = g.Key,
            ////    UpdateDateStamp = g.Max(row => row.UpdateDateStamp)
            ////});

            ////IQueryable<MyChildEntity> filteredChildren = from chd in this.entityDbContext.MyChilds

            ////                                      join perParentMaxUpdateDatePoco in pocoPerParentMaxUpdateDates
            ////                                      on new { a = chd.UpdateDateStamp, b = chd.MyParentUUID } equals new { a = perParentMaxUpdateDatePoco.UpdateDateStamp, b = perParentMaxUpdateDatePoco.MyParentUUID }

            ////                                       where 1 == 1
            ////                                       && !magicValues.Contains(chd.MyChildMagicStatus)
            ////                                       && perParentMaxUpdateDatePoco.UpdateDateStamp < cutOffDate
            ////                                       select chd;
            /* END WORKING MyChildEntity */

            /* WORKING piggyback on existing MyChildEntity START */
            IQueryable<MyChildEntity> reuseMyChildEntityWithOnlyMaxUpdateDateStampChildEntities = this.entityDbContext.MyChilds.GroupBy(i => i.MyParentUuidFk).Select(g =>
                new MyChildEntity
                {
                    MyParentUuidFk = g.Key,
                    UpdateDateStamp = g.Max(row => row.UpdateDateStamp)
                });

            IQueryable<MyChildEntity> filteredChildren = from chd in this.entityDbContext.MyChilds
                                                         join reuseMyChildEntityWithOnlyMaxUpdateDateStampChild in reuseMyChildEntityWithOnlyMaxUpdateDateStampChildEntities
                                                         on new { a = chd.UpdateDateStamp, b = chd.MyParentUuidFk }
                                                         equals
                                                         new { a = reuseMyChildEntityWithOnlyMaxUpdateDateStampChild.UpdateDateStamp, b = reuseMyChildEntityWithOnlyMaxUpdateDateStampChild.MyParentUuidFk }
                                                         where !magicStatusValues.Contains(chd.MyChildMagicStatus)
                                                         && reuseMyChildEntityWithOnlyMaxUpdateDateStampChild.UpdateDateStamp < cutOffDate
                                                         select chd;
            /* WORKING piggyback on existing MyChildEntity END */

            IQueryable<MyParentEntity> matchingParents = from par in this.entityDbContext.MyParents
                                                         where filteredChildren.Any(fc => fc.MyParentUuidFk == par.MyParentKey)
                                                         ||
                                                         !par.MyChilds.Any()
                                                         select par;

            this.logger.LogInformation(new string('*', 1000));

            IEnumerable<MyParentEntity> returnItems = await matchingParents.AsNoTracking().ToListAsync(token);

            this.logger.LogInformation(new string('*', 1000));

            return returnItems;
        }

        public async Task<IEnumerable<MyParentEntity>> PerformDemoIQueryableWithAnonymousClass(TimeSpan cutOffTimeSpan, ICollection<int> magicStatusValues, CancellationToken token)
        {
            DateTime cutOffDate = DateTime.Now.Add(cutOffTimeSpan);

            /* THIS WORKS
             * BUT this "style" generates 
             * "not be translated and will be evaluated locally"
             * warnings.  and is a performance k1ll3r.
             * Search for "will be evaluated locally" in the "MyCompany.MyExamples.ProjectParser.ConsoleOne.log*.txt" file.  (Where * is the date stamp)
             * */

            /* START ANONYMOUS TYPE */

            /*
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'where ([anonymousPerParentMaxUpdateDate].MaxPerParentUpdateDateStamp < __cutOffDate_1)' co
                 not be translated and will be evaluated locally.
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'where ([chd].MyParentUUID == [par].MyParentKey)' could not be translated and will be evalu
                d locally.
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'Any()' could not be translated and will be evaluated locally.
                warn: Microsoft.EntityFrameworkCore.Query[20500]
                      The LINQ expression 'where ({from MyChildEntity chd in value(Microsoft.EntityFrameworkCore.Query.Internal.Entit             
             */

            var anonymousPerParentMaxUpdateDates = this.entityDbContext.MyChilds.GroupBy(i => i.MyParentUuidFk).Select(g => new
            {
                MyAnonymousMyParentUUID = g.Key,
                MyAnonymousMaxPerParentUpdateDateStamp = g.Max(row => row.UpdateDateStamp)
            });

            IQueryable<MyChildEntity> filteredChildren = from chd in this.entityDbContext.MyChilds
                                                         join anonymousPerParentMaxUpdateDate in anonymousPerParentMaxUpdateDates
                                                         on new { a = chd.UpdateDateStamp, b = chd.MyParentUuidFk }
                                                         equals
                                                         new { a = anonymousPerParentMaxUpdateDate.MyAnonymousMaxPerParentUpdateDateStamp, b = anonymousPerParentMaxUpdateDate.MyAnonymousMyParentUUID }
                                                         where !magicStatusValues.Contains(chd.MyChildMagicStatus)
                                                         && anonymousPerParentMaxUpdateDate.MyAnonymousMaxPerParentUpdateDateStamp < cutOffDate
                                                         select chd;
            /* END ANONYMOUS TYPE */

            IQueryable<MyParentEntity> matchingParents = from par in this.entityDbContext.MyParents
                                                         where filteredChildren.Any(fc => fc.MyParentUuidFk == par.MyParentKey)
                                                         ||
                                                         !par.MyChilds.Any()
                                                         select par;

            this.logger.LogInformation(new string('*', 1000));

            IEnumerable<MyParentEntity> returnItems = await matchingParents.AsNoTracking().ToListAsync(token);

            this.logger.LogInformation(new string('*', 1000));

            return returnItems;
        }

        public async Task<IEnumerable<MyParentEntity>> PerformDemoIQueryableWithPrivateClassHolderObject(TimeSpan cutOffTimeSpan, ICollection<int> magicStatusValues, CancellationToken token)
        {
            DateTime cutOffDate = DateTime.Now.Add(cutOffTimeSpan);

            IQueryable<ChildMaxHolder> childMaxHolders = this.entityDbContext.MyChilds.GroupBy(i => i.MyParentUuidFk).Select(g => new
            ChildMaxHolder
            {
                MyPrivateClassParentUuid = g.Key,
                MyPrivateClassMaxChildUpdateDateStamp = g.Max(row => row.UpdateDateStamp)
            });

            /* below using childMaxHolders */
            IQueryable<MyChildEntity> filteredChildren = from chd in this.entityDbContext.MyChilds
                                                         join perParentMaxUpdateDate in childMaxHolders
                                                         on new { a = chd.UpdateDateStamp, b = chd.MyParentUuidFk }
                                                         equals
                                                         new { a = perParentMaxUpdateDate.MyPrivateClassMaxChildUpdateDateStamp, b = perParentMaxUpdateDate.MyPrivateClassParentUuid }
                                                         where !magicStatusValues.Contains(chd.MyChildMagicStatus)
                                                         && perParentMaxUpdateDate.MyPrivateClassMaxChildUpdateDateStamp < cutOffDate
                                                         select chd;
            /* ChildMaxHolder END */

            IQueryable<MyParentEntity> matchingParents = from par in this.entityDbContext.MyParents
                                                         where filteredChildren.Any(fc => fc.MyParentUuidFk == par.MyParentKey)
                                                         ||
                                                         !par.MyChilds.Any()
                                                         select par;

            this.logger.LogInformation(new string('*', 1000));

            IEnumerable<MyParentEntity> returnItems = await matchingParents.AsNoTracking().ToListAsync(token);

            this.logger.LogInformation(new string('*', 1000));

            return returnItems;
        }

        //// ********************************************************

        public async Task<MyParentEntity> GetSingleAsync(Guid keyValue, CancellationToken token)
        {
            MyParentEntity returnItem = await this.entityDbContext.MyParents.FindAsync(new object[] { keyValue }, token);
            return returnItem;
        }

        public async Task<MyParentEntity> AddAsync(MyParentEntity entity, CancellationToken token)
        {
            if (entity.UpdateDateStamp == DateTime.MinValue)
            {
                entity.UpdateDateStamp = DateTime.Now;
            }

            this.entityDbContext.MyParents.Add(entity);
            int saveChangesAsyncValue = await this.entityDbContext.SaveChangesAsync(token);
            if (ExpectedAddSingleOrUpdateSingleOrDeleteSingleSaveChangesAsyncRowCount != saveChangesAsyncValue)
            {
                throw new ArgumentOutOfRangeException(string.Format(ErrorMsgExpectedSaveChangesAsyncRowCount, saveChangesAsyncValue), (Exception)null);
            }

            return entity;
        }

        public async Task<MyParentEntity> UpdateAsync(MyParentEntity entity, CancellationToken token)
        {
            int saveChangesAsyncValue = 0;
            MyParentEntity foundEntity = await this.entityDbContext.MyParents.FirstOrDefaultAsync(item => item.MyParentKey == entity.MyParentKey, token);
            if (null != foundEntity)
            {
                foundEntity.MyParentName = entity.MyParentName;
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
                ArgumentOutOfRangeException argEx = new ArgumentOutOfRangeException(string.Format(ErrorMsgPrimaryEntityNotFound, entity.MyParentKey), (Exception)null);
                this.logger.LogError(argEx.Message, argEx);
                throw argEx;
            }

            return foundEntity;
        }

        public async Task<int> DeleteAsync(Guid keyValue, CancellationToken token)
        {
            int returnValue = 0;
            MyParentEntity foundEntity = await this.entityDbContext.MyParents.FirstOrDefaultAsync(item => item.MyParentKey == keyValue, token);
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

        private class ChildMaxHolder
        {
            public Guid MyPrivateClassParentUuid { get; set; }

            public DateTime MyPrivateClassMaxChildUpdateDateStamp { get; set; }
        }
    }
}
