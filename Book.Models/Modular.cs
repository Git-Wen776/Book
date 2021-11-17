using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_Modular")]
    public class Modular
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int ModularId { get; set; }

        [SugarColumn(ColumnDataType ="varchar(20)")]
        public string ModularName { get; set;}

        [SugarColumn(ColumnDataType ="varchar(100)",IsNullable =true)]
        public string ModularDescription { get; set;}

        public DateTime CreateTime { get; set; } = DateTime.Now;
        [SugarColumn(ColumnDataType = "varchar(20)", IsNullable = true)]
        public string Creater { get; set; }

        public int RId { get; set; }

        public int IsDeleted { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<ApiUrl> ApiUrls { get; set; }   
    }
}
