using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_ApiUrl")]
    public class ApiUrl
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int ApiId { get; set; }

        public string Url { get; set; }
        [SugarColumn(ColumnDataType ="varchar(100)")]
        public string Describe { get; set; }
        public int ModularId { get; set; }
        public int IsDeleted { get; set; }
    }
}
