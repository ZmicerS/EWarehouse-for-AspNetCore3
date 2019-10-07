using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EWarehouse.Repository.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Get(Expression<Func<T, bool>> condition, string includeProperties = "");
        void Create(T entity);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(Object Id);
        void Delete(T entity);
    }

}
