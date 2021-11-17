using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
   [SugarTable("tb_Book")]
   public class Bookes
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int BookId { get; set; }

        public int Id { get; set; }

        public string BookIds { get; set; }
        [SugarColumn(ColumnDataType ="varchar(20)")]
        public string Auther { get; set; }

        [SugarColumn(ColumnDataType = "varchar(50)")]
        public string BookName { get; set; }

        public DateTime Begintime { get; set; }

        [SugarColumn(IsNullable =true)]
        public DateTime? EndTime { get; set; }

        public int Chapter { get; set; }

        [SugarColumn(IsNullable =true)]
        public int UserId { get; set; }

        public int AuthorId { get; set; }
    }
}
