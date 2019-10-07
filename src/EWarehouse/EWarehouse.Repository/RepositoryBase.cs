using EWarehouse.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EWarehouse.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _dbContext = null;
        private readonly DbSet<T> _dbSet = null;

        private readonly ILogger _logger;

        public RepositoryBase(DbContext context, ILogger logger)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
            _logger = logger;
        }

        public T GetById(object id)
        {
            _logger.LogInformation($"GetById - {typeof(T)}");
            return _dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object Id)
        {
            _logger.LogInformation($"GetByIdAsync - {typeof(T)}");
            return await _dbSet.FindAsync(Id);
        }

        public IQueryable<T> GetAll()
        {
            _logger.LogInformation($"GetAll - {typeof(T)} ");
            return _dbSet.AsNoTracking();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation($"GetAllAsync - {typeof(T)}");
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> condition, string includeProperties = "")
        {
            _logger.LogInformation($"Get - {typeof(T)}");
            IQueryable<T> query = _dbSet;
            if (condition != null)
            {
                query = query.Where(condition);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.AsQueryable();
        }

        public void Create(T entity)
        {
            _logger.LogInformation($"Create - {typeof(T)}");
            _dbSet.Add(entity);
        }

        public async Task CreateAsync(T entity)
        {
            _logger.LogInformation($"CreateAsync - {typeof(T)}");
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _logger.LogInformation($"Update - {typeof(T)}");
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Object Id)
        {
            _logger.LogInformation($"Delete - {typeof(T)}");
            T getObjById = _dbSet.Find(Id);
            if (getObjById != null)
            {
                Delete(getObjById);
            }
        }

        public void Delete(T entity)
        {
            _logger.LogInformation($"Delete - {typeof(T)}");
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
    }
}
