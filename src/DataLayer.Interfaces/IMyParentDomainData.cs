namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MyCompany.MyExamples.ProjectParser.Domain.Entities;

    public interface IMyParentDomainData : IDataRepository<Guid, MyParentEntity>
    {
        Task<IEnumerable<MyParentEntity>> PerformDemoIQueryableWithReusedPocoObject(TimeSpan cutOffTimeSpan, ICollection<int> magicStatusValues, CancellationToken token);

        Task<IEnumerable<MyParentEntity>> PerformDemoIQueryableWithAnonymousClass(TimeSpan cutOffTimeSpan, ICollection<int> magicStatusValues, CancellationToken token);

        Task<IEnumerable<MyParentEntity>> PerformDemoIQueryableWithPrivateClassHolderObject(TimeSpan cutOffTimeSpan, ICollection<int> magicStatusValues, CancellationToken token);
    }
}
