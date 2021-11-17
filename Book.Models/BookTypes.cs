using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.Models
{
    [SugarTable("tb_BookType")]
   public class BookTypes
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int BookTypesId { get; set; }

        public int BookId { get; set; }

        public int TypeId { get; set; }
        [SugarColumn(ColumnDataType ="varchar(50)")]
        public string TypeNames { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<Bookes> Books { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<Type> Types { get; set; }
    }
}
