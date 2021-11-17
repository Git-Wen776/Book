using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.SugarDb
{
   public interface IUnitWork
    {
        void Begintran();
        void CommitTran();
        void RollbackTran();
        ISqlSugarClient GetClient();
        void CreateTables(int setDefaultstring=200,bool isback=false, params Type[] types);
    }
}
