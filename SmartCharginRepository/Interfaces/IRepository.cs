namespace SmartCharginRepository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<bool> RemoveAsync(TEntity entit);
        Task<TEntity> GetByIdAsync(int id, params string[] includes);
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(params string[] includes);
        Task<TEntity> UpdateAsync(int id, TEntity entity);
    }
}
