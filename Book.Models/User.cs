using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
   [SugarTable("tb_User")]
   public class User
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
        public int Id { get; set; }

        public string UserIds { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(20)")]
        public string Name { get; set; }

        public DateTime Birtime { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(20)")]
        public string Password { get; set; }

        [SugarColumn(ColumnDataType ="Nvarchar(20)")]
        public string Account { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<Bookes> Books { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<Role> Roles { get; set; }   
    }
}
