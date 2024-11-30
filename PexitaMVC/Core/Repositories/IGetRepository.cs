namespace PexitaMVC.Core.Repositories
{
    public interface IGetRepository<T> where T : class
    {
        T GetByID(int id);
        Task<T> GetByIDAsync(int id);
    }

    public interface IAddRepository<T> where T : class
    {
        void Add(T entity);
        Task AddAsync(T entity);
    }

    public interface IUpdateRepository<T> where T : class
    {
        T Update(T entity);
        Task<T> UpdateAsync(T entity);
    }

    public interface IDeleteRepository<T> where T : class
    {
        T Delete(int id);
        Task<T> DeleteAsync(int id);
    }
}
