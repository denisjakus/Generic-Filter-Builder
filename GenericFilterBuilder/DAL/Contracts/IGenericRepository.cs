using GenericFilterBuilder.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericFilterBuilder.DAL.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(Guid id);
        Task<T> GetOneAsync(Guid id);
        Task<T> GetByNameAsync(string name);
        Task<Guid> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid entityId);
        Task<IReadOnlyList<T>> GetAllPagedAndFilteredAsync(Expression<Func<T, bool>> filter = null, int? page = null, int? recordsPerPage = null, string orderBy = null,
         OrderDirection orderDirection = OrderDirection.Desc);
    }
}
