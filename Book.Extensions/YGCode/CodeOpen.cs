using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.YGCode
{
    public class CodeOpen:ICodeOpen
    {
        public readonly string CodeNum = "1234567890";
        public readonly string Encode = "ABCDEFGHIJKLOVERTYUQOPZXMNabcdefghijklovnnyutrqwzx";

        private readonly CodeOptions options;
        public CodeOpen(CodeOptions option)
        {
            options=option;
        }

    }
}
