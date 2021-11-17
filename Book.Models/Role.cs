using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_Role")]
    public class Role
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Rid { get; set; }

        [SugarColumn(ColumnDataType ="varchar(20)")]
        public string RoleName { get; set; }

        [SugarColumn(ColumnDataType ="varchar(200)")]
        public string Describe { get; set; }

        public int UserId { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<Modular> Modulars { get; set; }
    }
}
