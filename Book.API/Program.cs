using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog.Config;
using NLog.Web;
using Book.Extensions.SugarDb;
using Book.Models;

namespace Book.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Debug("init main");
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope()) 
            {
                var log=scope.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
                try
                {
                    var work = scope.ServiceProvider.GetService<IUnitWork>();
                    Type[] types = new Type[] { typeof(ApiUrl), typeof(Bookes), typeof(BookTypes), typeof(Modular), 
                        typeof(Role), typeof(Types), typeof(User),typeof(Author),typeof(Hobby),typeof(TaskQz) };
                    work.CreateTables(200, false, types);
                    log.LogInformation("初始化数据库表成功");
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "Stopped program because of exception");
                    log.LogError(exception, "初始化数据库表失败");
                    throw;
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(
                context => {
                    context.ClearProviders();
                    context.SetMinimumLevel(LogLevel.Trace);
                    context.AddFilter("microsoft",LogLevel.Error);
                }
                ).UseNLog();
    }
}
