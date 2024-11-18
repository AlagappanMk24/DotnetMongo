namespace DotnetMongo.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
