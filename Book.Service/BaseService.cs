using Book.IRepository;
using Book.IService;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        protected IBaseRepository<TEntity> _repository { get; set; }
        public Task<bool> AddAsync(TEntity entity)
        {
            return _repository.AddAsync(entity);
        }

        public Task AddBatchAsync(List<TEntity> entities)
        {
            return _repository.AddBatchAsync(entities);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task DeleteBatchAsync(List<TEntity> entities)
        {
            return _repository.DeleteBatchAsync(entities);
        }

        public Task<TEntity> FindAsync(int id)
        {
            return _repository.FindAsync(id);
        }

        public Task<List<TEntity>> QueryAsync()
        {
            return _repository.QueryAsync();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _repository.QueryAsync(expression);
        }

        public Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total)
        {
            return _repository.QueryAsync(page, size, total);
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression, int page, int size, RefAsync<int> total)
        {
            return _repository.QueryAsync(expression, page, size, total);
        }

        public Task<List<TEntity>> QueryAsync(string sql, List<SugarParameter> parameters)
        {
            return _repository.QueryAsync(sql, parameters);
        }

        public Task<List<TResult>> QueryAsync<T, T2, TResult>(Expression<Func<T, T2, object[]>> joinexpre, Expression<Func<T, T2, TResult>> selectexpre,
            Expression<Func<TResult, bool>> whereExpre, int page, int size, RefAsync<int> total)
        {
            return _repository.QueryAsync(joinexpre,selectexpre,whereExpre,page,size,total);
        }

        public Task<List<TEntity>> QueryAsync(string oderbyfiled,
            Expression<Func<TEntity, object>> expression, 
            TEntity entity, OrderByType type, int page, int size, RefAsync<int> total)
        {
            return _repository.QueryAsync(oderbyfiled, expression, entity, type, page, size, total);
        }

        public Task<List<TEntity>> QueryAsync(string oderbyfiled, Expression<Func<TEntity, bool>> whereExpr,
            OrderByType type, int page, int size, RefAsync<int> total)
        {
            return _repository.QueryAsync(oderbyfiled,whereExpr, type, page, size, total);
        }

        public Task<List<TResult>> QueryAsync<T, T2, T3, TResult>(string orderfiled,
            Expression<Func<T, T2, T3, object[]>> joinExpre,
            Expression<Func<T, T2, T3, TResult>> selectExpre,
            Expression<Func<TResult, bool>> whereExpre, int page, int size, RefAsync<int> total)
        {
            return _repository.QueryAsync(orderfiled,joinExpre,selectExpre,whereExpre,page,size,total);
        }

        public Task<bool> RemoveAsync(TEntity entity)
        {
            return _repository.RemoveAsync(entity);
        }

        public Task<int> SugarCommandsAsync(string sql, SugarParameter[] parameters)
        {
            return _repository.SugarCommandsAsync(sql, parameters);
        }

        public Task<bool> UpAsync(TEntity entity)
        {
            return _repository.UpAsync(entity);
        }

        public Task UpBatchAsync(List<TEntity> entities)
        {
            return _repository.UpBatchAsync(entities);
        }

        public Task<List<TEntity>> UseProcAsync(string procname, List<SugarParameter> parameters)
        {
            return _repository.UseProcAsync(procname, parameters);
        }
    }
}
