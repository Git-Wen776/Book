using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_Hobby")]
    public class Hobby
    {
        [SugarColumn(IsIdentity =true,IsPrimaryKey =true)]
        public int Id { get; set; } 

        public int UserId { get; set; }

        public int AuthorId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public User User { get; set; }

        [SugarColumn(IsIgnore =true)]
        public Author Author { get; set; }
    }
}
