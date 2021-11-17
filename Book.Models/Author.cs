using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_Author")]
    public class Author
    {
        [SugarColumn(IsIdentity =true,IsPrimaryKey =true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType ="varchar(20)")]
        public string Ids { get; set; }

        [SugarColumn(ColumnDataType ="varchar(50)")]
        public string AuthorName { get; set; }

        [SugarColumn(ColumnDataType ="varchar(100)")]
        public string Address { get; set; }

        [SugarColumn(ColumnDataType = "varchar(50)")]
        public string RealName { get; set; }    

        public DateTime BirthTime { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar(255)")]
        public string Introduce { get; set; }

        public int Age { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<Bookes> Bookes { get; set; }    
    }
}
