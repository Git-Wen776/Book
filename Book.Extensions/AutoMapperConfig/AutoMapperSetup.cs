using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var profiles = Assembly.GetExecutingAssembly().GetTypes()
                .Where(a => a.Name.EndsWith("Profile"));
                foreach(var profile in profiles)
                    p.AddProfile(profile);
            });
        }
    }
}
