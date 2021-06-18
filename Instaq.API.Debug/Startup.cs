namespace Instaq.API.Debug
{
    using System;
    using Instaq.Contract;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql;
    using Instaq.Database.Storage.Mysql.Generated;
    using Instaq.DiskFileHandling;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup( IConfiguration configuration )
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded    = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });
            
            services.AddControllers();
            services.AddCors();

            var dbConnection = Configuration.GetConnectionString("HashtagDatabase");
            dbConnection = dbConnection.Replace("[server]", Environment.GetEnvironmentVariable("instatagger_mysql_ip"));
            dbConnection = dbConnection.Replace("[user]", Environment.GetEnvironmentVariable("instatagger_mysql_user"));
            dbConnection = dbConnection.Replace("[pw]", Environment.GetEnvironmentVariable("instatagger_mysql_pw"));
            dbConnection = dbConnection.Replace("[db]", Environment.GetEnvironmentVariable("instatagger_mysql_db"));
            services.AddDbContext<InstaqContext>(options => options.UseMySql(dbConnection));

            services.AddTransient<IDebugStorage, MysqlDebugStorage>();
            services.AddTransient<IFileHandler, DiskFileHander>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Instaq Debug", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instaq Debug API v1"); });

            app.UseCors(options => options.WithOrigins(
                "http://localhost:4201",
                "http://instaq-debug.innocliq.de"
            ).AllowAnyMethod());

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
