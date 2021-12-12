using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Book.Models.ModelValidation
{
    public class RegexValidaProperty<T> : PropertyValidator<T, string>
    {
        public override string Name => "RegesValidaProperty";

        private string regestr;

        public RegexValidaProperty(string _regesStr)
        {
            regestr = _regesStr;
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("value is not empty or null string");
            if (string.IsNullOrEmpty(value))
                context.MessageFormatter.AppendArgument("RegesStr",regestr);
            Regex regex = new Regex(regestr);
            return regex.IsMatch(value);

        }

        protected override string GetDefaultMessageTemplate(string errorCode) => $"";
        
    }
}
