using System.Linq.Expressions;

namespace PexitaMVC.Core.Repositories
{
    public interface IGetRepository<T> where T : class
    {
        T GetByID(int id);
        Task<T> GetByIDAsync(int id);
    }

    public interface IGetWithRelationsRepository<T> where T : class
    {
        T GetWithRelations(int id, params Expression<Func<T, object>>[] expressions);
        Task<T> GetWithRelationsAsync(int id, params Expression<Func<T, object>>[] expressions);
    }

    public interface IAddRepository<T> where T : class
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
    }

    public interface IUpdateRepository<T> where T : class
    {
        void Update(T entity);
        Task UpdateAsync(T entity);
    }

    public interface IDeleteRepository<T> where T : class
    {
        void Delete(T Entity);
        Task DeleteAsync(T Entity);
    }
}
