using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.SugarDb
{
   public class UnitWork:IUnitWork
    {
        private ISqlSugarClient db;
        private ILogger<UnitWork> logger; 

        public UnitWork(ISqlSugarClient _clinet,ILogger<UnitWork> _logger)
        {
            db = _clinet;
            logger = _logger;
        }

        public void Begintran()
        {
            db.Ado.BeginTran();
        }

        public void CommitTran()
        {
            db.Ado.CommitTran();
        }

        public void CreateTables(int setDefaultstring=200,bool isback = false, params Type[] types)
        {
            db.CodeFirst.SetStringDefaultLength(setDefaultstring);
            
            if (db == null)
            {
                logger.LogWarning("数据库未连接");
                return;
            }
            logger.LogInformation("数据库连接成功");
            if (isback)
            {
                db.CodeFirst.BackupTable().InitTables(types);
            }
            else
            {
                db.CodeFirst.InitTables(types);
            }
        }

        public ISqlSugarClient GetClient()
        {
            return db;
        }

        public void RollbackTran()
        {
            try
            {
                CommitTran();
            }
            catch (Exception)
            {

                db.Ado.RollbackTran();
            }
        }
    }
}
