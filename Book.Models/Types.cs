using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_Type")]
   public class Types
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int TypeId { get; set; }

        [SugarColumn(ColumnDataType ="varchar(20)")]
        public string Typename { get; set; }
    }
}
