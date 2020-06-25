using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.MyExamples.ProjectParser.DomainDataLayer.Interfaces
{
    public interface IDataRepository<TKey, TEntity> where TKey : struct
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token);

        Task<TEntity> GetSingleAsync(TKey keyValue, CancellationToken token);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken token);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token);

        Task<int> DeleteAsync(TKey keyValue, CancellationToken token);
    }
}