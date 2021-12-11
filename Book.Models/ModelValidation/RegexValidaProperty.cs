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
            regestr= _regesStr;
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Equals(""))
                throw new ArgumentException("value is not empty string");
            Regex regex=new Regex(regestr);
            return regex.IsMatch(value);

        }
    }
}
