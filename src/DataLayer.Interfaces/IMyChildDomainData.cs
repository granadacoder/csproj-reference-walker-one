using System;
using MyCompany.MyExamples.ProjectParser.Domain.Entities;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces
{
    public interface IMyChildDomainData : IDataRepository<Guid, MyChildEntity>
    {
    }
}
