using MagicVillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : IEntity
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task<T> GetLastAsync();
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity); 
        Task SaveAsync();
    }
}
