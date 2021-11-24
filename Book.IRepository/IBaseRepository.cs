using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.IRepository
{
   public interface IBaseRepository<TEntity> where TEntity:class,new()
    {
        Task<TEntity> FindAsync(int id);
        Task<bool> UpAsync(TEntity entity);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity);
        Task<List<TEntity>> QueryAsync();
        Task<bool> DeleteAsync(int id);
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression);

        Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total);
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression, int page, int size, RefAsync<int> total);
        Task AddBatchAsync(List<TEntity> entities);
        Task UpBatchAsync(List<TEntity> entities);
        Task DeleteBatchAsync(List<TEntity> entities);
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(string sql, List<SugarParameter> parameters);
        /// <summary>
        /// sql语句增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> SugarCommandsAsync(string sql, SugarParameter[] parameters);
        /// <summary>
        /// sql存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> UseProcAsync(string procname, List<SugarParameter> parameters);
        /// <summary>
        /// 两表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinexpre"></param>
        /// <param name="selectexpre"></param>
        /// <param name="whereExpre"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TResult>> QueryAsync<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinexpre,
            Expression<Func<T, T2, TResult>> selectexpre,
            Expression<Func<TResult, bool>> whereExpre,
            int page,
            int size,
            RefAsync<int> total);
        /// <summary>
        /// 实体分页排序查询
        /// </summary>
        /// <param name="oderbyfiled"></param>
        /// <param name="expression"></param>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(string oderbyfiled,
            Expression<Func<TEntity, object>> expression,
            TEntity entity,
            OrderByType type,
            int page, int size, RefAsync<int> total);
        /// <summary>
        /// 单表分页排序查询
        /// </summary>
        /// <param name="oderbyfiled"></param>
        /// <param name="whereExpr"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(string oderbyfiled,
            Expression<Func<TEntity, bool>> whereExpr,
            OrderByType type,
            int page, int size, RefAsync<int> total);
        /// <summary>
        /// 两表分页排序查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="orderfiled"></param>
        /// <param name="joinExpre"></param>
        /// <param name="whereExpre"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TResult>> QueryAsync<T, T2, T3, TResult>(string orderfiled,
            Expression<Func<T, T2, T3, object[]>> joinExpre,
            Expression<Func<T, T2, T3, TResult>> selectExpre,
            Expression<Func<TResult, bool>> whereExpre,
            int page, int size, RefAsync<int> total);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
    }
}
