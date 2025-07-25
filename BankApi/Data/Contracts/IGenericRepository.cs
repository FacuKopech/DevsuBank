
namespace Data.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetEntityAsync(Guid Id);
        Task<bool> AddEntityAsync(T entity);
        Task<bool> ModifyEntityAsync(T entity);
        Task<bool> DeleteEntityAsync(Guid Id);
    }
}
