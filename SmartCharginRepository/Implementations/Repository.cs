using Microsoft.EntityFrameworkCore;
using SmartCharginModels.Entities;
using SmartCharginRepository.DatabaseContext;
using SmartCharginRepository.Interfaces;

namespace SmartCharginRepository.Implementations
{
    public class Repository<TEntity>(SmartCharginDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly SmartCharginDbContext _context = context;

        public async Task<TEntity> GetByIdAsync(int id, params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(int id, TEntity entity)
        {
            if (id != entity.Id)
            {
                throw new ArgumentException("Entity id does not match the provided id");
            }

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
