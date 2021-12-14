using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Extensions;
using Book.Extensions.Config;
using Book.Extensions.ConfigSetting;
using Microsoft.Extensions.Configuration.Json;
using Swashbuckle.AspNetCore;
using Book.Extensions.SugarDb;
using Book.IRepository;
using Book.Repository;
using Book.Service;
using Book.IService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Book.API.AuthonCommon;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Book.Extensions.ExpressionExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Book.Extensions.RedisHelper;

namespace Book.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string CorePolicy = "Blog.Api.Cores";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 配置文件
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
              .Build();
            services.AddSingleton(new ConfigHelper(Configuration));
            #endregion
            services.AddControllers();
            #region Swagger配置
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book.API", Version = "v1" });
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme() {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In=ParameterLocation.Header,
                    Type=SecuritySchemeType.ApiKey
                });
            });
            #endregion
            #region sqlsugar配置
            services.AddSingleton<ISqlSugarClient,SqlSugarScope>(p =>
            {
                return new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = Configuration.GetSection("ConnectionStrings")["BookDB"]
                }) ;
            });
            services.AddScoped<IUnitWork,UnitWork>();
            services.AddRepositroyRely();
            services.AddServiceRely();
            services.AddExpressionSetup();
            #endregion
            #region 日志文件配置
            services.AddNlog();
            #endregion
            #region 跨域配置
            var ports =new List<string>();
            config.Bind("AllowPorts", ports);
            services.AddCors(o =>
            {
                o.AddPolicy(CorePolicy, options =>
                {
                    options.WithOrigins(ports.ToArray()).
                    AllowAnyHeader().
                    AllowAnyMethod();
                });
            });
            #endregion
            #region jwt配置
            #region 授权方案
            SecurityKey security = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetSection("Authen:SingingKey").Value));
            //简单授权--基于角色
            bool openrole = false;
            if (openrole)
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("User", p => p.RequireRole("User").Build());
                    options.AddPolicy("Admin", p => p.RequireRole("Admin").Build());
                    options.AddPolicy("Systemer", p => p.RequireRole("Systemer").Build());
                    options.AddPolicy("SysOrAdmin", p => p.RequireRole("Systemer", "Admin"));
                    options.AddPolicy("SysAndAdmin", p => p.RequireRole("Systemer").RequireRole("Admin").Build());
                });
            }
            else//复杂授权（说到底也是基于角色）
            {
                var authitem = new List<PermissionItem>();
                var permissionrequirement = new PermissonRequirement(
                     deniedAction: "",
                     permissions: authitem,
                     claimType: ClaimTypes.Role,//基于jwt的claim的claimTypes.Role
                     issuer: config.GetSection("Authen:Isusser").Value,
                     audience: config.GetSection("Authen:Audience").Value,
                     signingCredentials: new SigningCredentials(security, SecurityAlgorithms.HmacSha256),
                     expiration: TimeSpan.FromSeconds(60 * 2)
                    );
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("BookPolicy", p => p.Requirements.Add(permissionrequirement));
                });
            }
            #endregion
            #region 认证方案

            services.AddAuthentication(s=> {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = security,
                        ValidateIssuer = true,
                        ValidIssuer = config.GetSection("Authen:Isusser").Value,
                        ValidateAudience = true,
                        ValidAudience = config.GetSection("Authen:Audience").Value,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                    };
                });
            #endregion
            //注入jwthelper
            services.AddScoped<JwtHelper>(p=>new JwtHelper(config));

            services.AddScoped<IAuthorizationHandler, AuthHandler>();
            //注入httpcontext上下文
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion
            #region automapper配置
            #endregion
            #region redis配置
            services.AddRedisSetup();
            services.AddSerializeSetup();
            #endregion
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book.API v1");
                    c.DefaultModelExpandDepth(-1);
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(CorePolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class RelyExtensions
    {
        /// <summary>
        /// 注入仓储层
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositroyRely(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepositroy>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<IBookTypeRepsitory, BookTypeRepository>();
            services.AddScoped<IRoleRepositroy,RoleRepositroy>();
            services.AddScoped<IModularRepository, ModularRepository>();
            services.AddScoped<IApiUrlRepositroy, ApiUrlRepositroy>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IHobbyRepository, HobbyRepository>();
        }
        /// <summary>
        /// 注入逻辑层
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceRely(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
