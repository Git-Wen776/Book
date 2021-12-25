using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    public class TaskQz
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType ="varchar(20)")]
        public string QzName { get; set; }

        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Jobkey { get; set; }

        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Groupkey { get; set; }

        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Triggertyoe { get; set; }
        public int Runtime { get; set; }

        [SugarColumn(ColumnDataType = "varchar(100)")]
        public string Info { get; set; }
    }
}
