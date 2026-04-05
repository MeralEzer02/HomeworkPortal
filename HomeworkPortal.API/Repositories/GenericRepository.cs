using Microsoft.EntityFrameworkCore;
using HomeworkPortal.API.Data;
using HomeworkPortal.API.Models;
using System.Linq.Expressions;

namespace HomeworkPortal.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(expression);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
    }
}