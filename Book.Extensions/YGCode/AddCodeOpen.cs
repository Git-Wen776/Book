using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.YGCode
{
    public static class AddCodeOpen
    {
        public static IServiceCollection AddSingetionCode(this IServiceCollection service, Action<CodeOptions> code)
        {
            var options=new CodeOptions();
            code.Invoke(options);
            service.AddSingleton<ICodeOpen>(p=> new CodeOpen(options));
            return service;
        }
    }
}
