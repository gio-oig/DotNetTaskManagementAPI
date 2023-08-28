using System.Linq.Expressions;

namespace TaskManagement.Repository.IRepository
{
    public interface IRepository<T> where T : class 
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, object>>? include = null, Expression<Func<T, bool>>? filter = null, int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, object>>? include = null,  Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T entity);

        Task RemoveAsync(T entity);

        Task SaveAsync();
    }
}
