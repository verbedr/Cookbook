using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Common.Repository;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Swashbuckle.AspNetCore.Swagger;
using Cookbook.Api.Infrastructure;
using System.IO;
using Cookbook.Api.Infrastructure.Log4net;

namespace Cookbook.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public IConfigurationRoot Configuration { get; }

        #region ConfigureServices
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigureConnectionOptions(services);
            ConfigureFrameworkService(services);
            ConfigureSwaggerService(services);
            ConfigurePrincipalService(services);

            return services.ConfigureDependencyInjection(Configuration);
        }

        #region Custom Service Configuration
        private void ConfigureConnectionOptions(IServiceCollection services)
        {
            services.AddSingleton(new ConnectionOptions
            {
                ConnectionString = Configuration.GetConnectionString("Cookbook.Repository.CookbookDbContext"),
                EnableDbContextMigration = Configuration.GetValue<bool>("Settings:EnableDbContextMigration")
            });
        }

        private void ConfigureFrameworkService(IServiceCollection services)
        {
            services
                .AddMvc(o =>
                {
                    o.Filters.Add(new ApiExceptionFilterAttribute(!_env.IsDevelopment()));
                })
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                    o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
        }

        private void ConfigureSwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Mijn Orbis", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
                c.DescribeAllEnumsAsStrings();
            });
        }

        private void ConfigurePrincipalService(IServiceCollection services)
        {
            services.AddTransient<IPrincipal>(p => p.GetService<IHttpContextAccessor>().HttpContext.User);
        }
        #endregion

        #endregion

        #region Configure
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureLogging(loggerFactory);
            ConfigureMvc(app);
            ConfigureSwagger(app);
            ConfigureSpaRoot(app);
        }

        #region Custom Configure
        private void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddLog4Net(Configuration.GetSection("Settings").GetValue<string>("Log4NetConfig"));
            loggerFactory.AddDebug();
        }

        private void ConfigureMvc(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }

        private void ConfigureSpaRoot(IApplicationBuilder app)
        {
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mijn Orbis V1");
            });
        }
        #endregion
        #endregion
    }
}
