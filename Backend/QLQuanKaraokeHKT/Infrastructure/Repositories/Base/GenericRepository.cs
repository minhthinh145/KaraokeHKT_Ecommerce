using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;
using QLQuanKaraokeHKT.Infrastructure.Data;
using System.Linq.Expressions;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Base
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly QlkaraokeHktContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(QlkaraokeHktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TEntity>();
        }

        #region Basic CRUD Operations

        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception)
            {
                return new List<TEntity>();
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return new List<TEntity>();
            }
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(TKey id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null)
                    return false;

                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Query Operations

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            try
            {
                return predicate == null
                    ? await _dbSet.CountAsync()
                    : await _dbSet.CountAsync(predicate);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region Pagination

        public virtual async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool ascending = true)
        {
            try
            {
                if (pageNumber <= 0) pageNumber = 1;
                if (pageSize <= 0) pageSize = 10;

                var query = _dbSet.AsQueryable();

                // Apply filter
                if (predicate != null)
                    query = query.Where(predicate);

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply sorting
                if (orderBy != null)
                {
                    query = ascending
                        ? query.OrderBy(orderBy)
                        : query.OrderByDescending(orderBy);
                }

                // Apply pagination
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch (Exception)
            {
                return (new List<TEntity>(), 0);
            }
        }

        #endregion  

        #region Transaction Support

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public virtual async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            try
            {
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            await transaction.RollbackAsync();
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}