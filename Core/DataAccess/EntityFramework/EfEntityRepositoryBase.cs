using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Concrete;
using Core.Enums;
using Core.Extensions;
using Core.Utilities.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public TEntity Add(TEntity entity)
        {
            using var context = new TContext();
            var addedEntity = context.Add(entity).Entity;
            context.SaveChanges();
            return addedEntity;
        }

        public TEntity Update(TEntity entity)
        {
            using var context = new TContext();
            context.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            using var context = new TContext();
            context.Remove(entity);
            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            using var context = new TContext();
            return context.Set<TEntity>().FirstOrDefault(expression);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null)
        {
            using var context = new TContext();
            return expression == null
                ? context.Set<TEntity>().AsNoTracking().ToList()
                : context.Set<TEntity>().Where(expression).AsNoTracking().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            using var context = new TContext();
            return expression == null
                ? await context.Set<TEntity>().ToListAsync()
                : await context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public PagingResult<TEntity> GetListForPaging(int page, string propertyName, bool asc, Expression<Func<TEntity, bool>> expression = null, params Expression<Func<TEntity, object>>[] includeEntities)
        {
            using var context = new TContext();
            var list = context.Set<TEntity>().AsQueryable();

            if (includeEntities.Length > 0)
                list = list.IncludeMultiple(includeEntities);

            if (expression != null)
                list = list.Where(expression);

            list = asc ? list.AscOrDescOrder(ESort.ASC, propertyName) : list.AscOrDescOrder(ESort.DESC, propertyName);
            int totalCount = list.Count();

            var start = (page - 1) * 10;
            list = list.Skip(start).Take(10);

            return new PagingResult<TEntity>(list.ToList(), totalCount, true, $"{totalCount} records listed.");
        }

        public async Task<PagingResult<TEntity>> GetListForTableSearch(TableGlobalFilter globalFilter)
        {
            using var context = new TContext();

            if (globalFilter == null)
            {
                var count = await context.Set<TEntity>().CountAsync();
                return new PagingResult<TEntity>(await context.Set<TEntity>().ToListAsync(), count, true, $"{count} records listed.");
            }

            var parameterOfExpression = Expression.Parameter(typeof(TEntity), "x");
            var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[] { });

            if (globalFilter.PropertyField.Count > 0)
            {
                var containMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchedValue = Expression.Constant(globalFilter.SearchText.ToLower(), typeof(string));

                var globalFilterPropertyField = Expression.PropertyOrField(parameterOfExpression, globalFilter.PropertyField[0]);
                Expression finalExpression = Expression.Call(Expression.Call(globalFilterPropertyField, toLowerMethod), containMethod, searchedValue);

                for (int i = 1; i < globalFilter.PropertyField.Count; i++)
                {
                    var propertyName = globalFilter.PropertyField[i];
                    globalFilterPropertyField = Expression.PropertyOrField(parameterOfExpression, propertyName);
                    var globalFilterConstant = Expression.Call(Expression.Call(globalFilterPropertyField, toLowerMethod), containMethod, searchedValue);

                    finalExpression = Expression.Or(finalExpression, globalFilterConstant);
                }

                var list = context.Set<TEntity>()
                    .Where(Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameterOfExpression));

                list = list.AscOrDescOrder(globalFilter.SortOrder == 1 ? ESort.ASC : ESort.DESC, globalFilter.SortField)
                           .Skip(globalFilter.First)
                           .Take(globalFilter.Rows);

                var totalCountForFilter = await list.CountAsync();
                return new PagingResult<TEntity>(await list.ToListAsync(), totalCountForFilter, true, $"{totalCountForFilter} records listed.");
            }

            var totalCount = await context.Set<TEntity>().CountAsync();
            return new PagingResult<TEntity>(await context.Set<TEntity>().Skip(globalFilter.First).Take(globalFilter.Rows).ToListAsync(), totalCount, true, $"{totalCount} records listed.");
        }

        public int SaveChanges()
        {
            using var context = new TContext();
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            using var context = new TContext();
            return await context.SaveChangesAsync();
        }

        public IQueryable<TEntity> Query()
        {
            var context = new TContext();
            return context.Set<TEntity>();
        }

        public Task<int> Execute(FormattableString interpolatedQueryString)
        {
            using var context = new TContext();
            return context.Database.ExecuteSqlInterpolatedAsync(interpolatedQueryString);
        }

        public TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null)
        {
            using var context = new TContext();
            var result = default(TResult);

            try
            {
                if (context.Database.ProviderName.EndsWith("InMemory"))
                {
                    result = action();
                    context.SaveChanges();
                }
                else
                {
                    using var tx = context.Database.BeginTransaction();
                    try
                    {
                        result = action();
                        context.SaveChanges();
                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                }

                successAction?.Invoke();
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
            }

            return result;
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            using var context = new TContext();
            return expression == null ? await context.Set<TEntity>().CountAsync() : await context.Set<TEntity>().CountAsync(expression);
        }

        public int GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            using var context = new TContext();
            return expression == null ? context.Set<TEntity>().Count() : context.Set<TEntity>().Count(expression);
        }
    }
}