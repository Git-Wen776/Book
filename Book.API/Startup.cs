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
            #region �����ļ�
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })//�����Ļ�������ֱ�Ӷ�Ŀ¼���json�ļ��������� bin �ļ����µģ����Բ����޸ĸ�������
              .Build();
            services.AddSingleton(new ConfigHelper(Configuration));
            #endregion
            services.AddControllers();
            #region Swagger����
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book.API", Version = "v1" });
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme() {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In=ParameterLocation.Header,
                    Type=SecuritySchemeType.ApiKey
                });
            });
            #endregion
            #region sqlsugar����
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
            #region ��־�ļ�����
            services.AddNlog();
            #endregion
            #region ��������
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
            #region jwt����
            #region ��Ȩ����
            SecurityKey security = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetSection("Authen:SingingKey").Value));
            //����Ȩ--���ڽ�ɫ
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
            else//������Ȩ��˵����Ҳ�ǻ��ڽ�ɫ��
            {
                var authitem = new List<PermissionItem>();
                var permissionrequirement = new PermissonRequirement(
                     deniedAction: "",
                     permissions: authitem,
                     claimType: ClaimTypes.Role,//����jwt��claim��claimTypes.Role
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
            #region ��֤����

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
            //ע��jwthelper
            services.AddScoped<JwtHelper>(p=>new JwtHelper(config));

            services.AddScoped<IAuthorizationHandler, AuthHandler>();
            //ע��httpcontext������
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion
            #region automapper����
            #endregion
            #region redis����
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
        /// ע��ִ���
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
        /// ע���߼���
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceRely(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
