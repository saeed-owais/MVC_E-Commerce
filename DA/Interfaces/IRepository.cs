using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        IQueryable<T> GetQueryable();


        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Remove(T entity);
    }
}
