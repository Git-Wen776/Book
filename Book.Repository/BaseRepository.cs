using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Book.IRepository;
using SqlSugar;
using Book.Extensions.SugarDb;


namespace Book.Repository
{
    public class BaseRepository<TEntity> : SimpleClient<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly ISqlSugarClient db;
        private readonly IUnitWork work;
        public BaseRepository(IUnitWork _work,ISqlSugarClient context = null) : base(context) {
            if (base.Context is null) {
                work = _work;
                base.Context = work.GetClient();
                db = work.GetClient();
            }
        }
        
        public Task<bool> AddAsync(TEntity entity)
        {
            return base.InsertAsync(entity);
        }

        public Task AddBatchAsync(List<TEntity> entities)
        {
            return base.InsertRangeAsync(entities);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return base.DeleteByIdAsync(id);
        }

        public async Task DeleteBatchAsync(List<TEntity> entities)
        {
            try
            {
                work.Begintran();
                foreach (var item in entities) {
                    await base.DeleteAsync(item);
                }
                work.CommitTran();
            }
            catch (Exception)
            {
                work.RollbackTran();
                throw;
            }
        }

        public Task<TEntity> FindAsync(int id)
        {
            return base.GetByIdAsync(id);
        }

        public Task<List<TEntity>> QueryAsync()
        {
            return db.Queryable<TEntity>().ToListAsync();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return db.Queryable<TEntity>().Where(expression).ToListAsync();
        }

        public Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total)
        {
            return db.Queryable<TEntity>().ToPageListAsync(page,size,total);
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression, int page, int size, RefAsync<int> total)
        {
            return db.Queryable<TEntity>().Where(expression).ToPageListAsync(page,size,total);
        }

        public Task<List<TEntity>> QueryAsync(string sql, List<SugarParameter> parameters)
        {
            return db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 两表表分页查询
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
        public async Task<List<TResult>> QueryAsync<T, T2, TResult>(Expression<Func<T, T2, object[]>> joinexpre,
            Expression<Func<T, T2, TResult>> selectexpre,
            Expression<Func<TResult, bool>> whereExpre,
            int page, int size, RefAsync<int> total)
        {
            if (whereExpre != null)
                return await db.Queryable(joinexpre)
                    .Select(selectexpre)
                    .Where(whereExpre)
                    .ToPageListAsync(page, size, total);
            return await db.Queryable(joinexpre).Select(selectexpre).ToPageListAsync(page, size, total);
        }
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
        public Task<List<TEntity>> QueryAsync(string oderbyfiled,
            Expression<Func<TEntity, object>> expression,
            TEntity entity, OrderByType type, int page, int size, RefAsync<int> total)
        {
            if (expression == null || string.IsNullOrEmpty(oderbyfiled))
                return db.Queryable<TEntity>().OrderBy(oderbyfiled).WhereClass(entity).ToPageListAsync(page, size, total);
            return db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(oderbyfiled), expression, type).WhereClass(entity).ToPageListAsync(page, size, total);
        }
        /// <summary>
        /// 单表排序自定义查询
        /// </summary>
        /// <param name="oderbyfiled"></param>
        /// <param name="expression"></param>
        /// <param name="whereExpr"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAsync(string oderbyfiled,
            Expression<Func<TEntity, bool>> whereExpr,
            OrderByType type, int page, int size, RefAsync<int> total)
        {
            if (whereExpr != null)
                return await db.Queryable<TEntity>().Where(whereExpr).ToPageListAsync(page, size, total);
            return await this.QueryAsync(page, size, total);
        }
        /// <summary>
        /// 三表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="orderfiled"></param>
        /// <param name="joinExpre"></param>
        /// <param name="whereExpre"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<TResult>> QueryAsync<T, T2, T3, TResult>(string orderfiled,
            Expression<Func<T, T2, T3, object[]>> joinExpre,
            Expression<Func<T, T2, T3, TResult>> selectExpre,
            Expression<Func<TResult, bool>> whereExpre,
            int page, int size, RefAsync<int> total)
        {
            return  db.Queryable(joinExpre).
                OrderByIF(!string.IsNullOrEmpty(orderfiled), orderfiled).
                Select(selectExpre).
                Where(whereExpre).
                ToPageListAsync(page, size, total);
        }

        public Task<bool> RemoveAsync(TEntity entity)
        {
            return base.DeleteAsync(entity);
        }

        public Task<int>SugarCommandsAsync(string sql, SugarParameter[] parameters)
        {
            return db.Ado.ExecuteCommandAsync(sql,parameters);
        }

        public Task<bool> UpAsync(TEntity entity)
        {
            return base.UpdateAsync(entity);
        }

        public Task UpBatchAsync(List<TEntity> entities)
        {
            return base.UpdateRangeAsync(entities);
        }

        public Task<List<TEntity>> UseProcAsync(string procname, List<SugarParameter> parameters)
        {
            return db.Ado.UseStoredProcedure().SqlQueryAsync<TEntity>(procname, parameters);
        }
    }
}
