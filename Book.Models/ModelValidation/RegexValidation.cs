using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;

namespace Book.Models.ModelValidation
{
    public static class RegexValidation
    {
        public static IRuleBuilderOptionsConditions<T,string> RegexValida<T>(this IRuleBuilderOptions<T,string> options,string regesstring)
        {
            return options.Custom((obj, context) =>
            {
                if (regesstring == null)
                    throw new ArgumentNullException(nameof(regesstring));
                if (obj.GetType() != typeof(string))
                    throw new ArgumentException("regesvalida regex must be string");
                Regex reges = new Regex(regesstring);
                if (!reges.IsMatch(obj.ToString()))
                    context.AddFailure("");
            });
        }
    }

}
