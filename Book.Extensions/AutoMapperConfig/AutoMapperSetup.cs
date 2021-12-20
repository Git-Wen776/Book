using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.AutoMapperConfig
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection service)
        {
            service.AddAutoMapper(p =>
            {
                p.AddProfile<UserProfile>();
            });
        }
    }
}
