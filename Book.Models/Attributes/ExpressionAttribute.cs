using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property)]
    public class ExpressionAttribute:Attribute
    {
        public Logictype logictype { get; set; }//And 0r
        public Wheretype ExpressionName { get { return expre; } }//Equeal、Like
        private Wheretype expre { get; set; }
        public ExpressionAttribute(Wheretype wheretype, Logictype logictype1)
        {
            expre = wheretype;
            logictype = logictype1;
        }
        public int SwitchType()
        {
            if (expre == Wheretype.Equal && logictype == Logictype.And)
                return 1;
            if (expre == Wheretype.Equal && logictype == Logictype.Or)
                return 2;
            if (expre == Wheretype.Like && logictype == Logictype.And)
                return 3;
            if (expre == Wheretype.Like && logictype == Logictype.Or)
                return 4;
            return 0;
        }

        public enum Wheretype
        {
            Equal,
            Like
        }

        public enum Logictype
        {
            And,
            Or
        }
    }
}
