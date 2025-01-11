using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Core.DataAccess
{
    public interface IEntityRepository<T>
        where T : class, IEntity
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);

        // Existing method signatures
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression = null);
        T Get(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);

        // Overloaded methods with includeProperties for eager loading
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression, string includeProperties);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression, string includeProperties);
        T Get(Expression<Func<T, bool>> expression, string includeProperties);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, string includeProperties);

        // Existing methods for paging and table search
        PagingResult<T> GetListForPaging(int page, string propertyName, bool asc, Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includeEntities);
        Task<PagingResult<T>> GetListForTableSearch(TableGlobalFilter globalFilter);

        // Additional utility methods
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IQueryable<T> Query();
        Task<int> Execute(FormattableString interpolatedQueryString);

        TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null);

        Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null);
        int GetCount(Expression<Func<T, bool>> expression = null);
    }
}