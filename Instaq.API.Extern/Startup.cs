﻿namespace Instaq.API.Extern
{
    using System;
    using Instaq.API.Extern.Helpers;
    using Instaq.API.Extern.Middleware;
    using Instaq.API.Extern.Services;
    using Instaq.API.Extern.Services.Interfaces;
    using Instaq.Contract;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql;
    using Instaq.Database.Storage.Mysql.Generated;
    using Instaq.DiskFileHandling;
    using Instaq.Evaluation;
    using Instaq.ImageProcessor.Standard.GcpVision;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly ILoggerFactory loggerFactory;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var logFilePath = Configuration.GetValue<string>("LogFilePath");
            var logger = new LoggerConfiguration()
                .WriteTo.RollingFile(logFilePath + "/api-extern.txt")
                .CreateLogger();
            Log.Logger = logger;
            Log.Logger.Information("Backend started :) -> welcome back ^.^");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            services.AddHealthChecks().AddCheck<HealthService>("IsDbConnectionHealthy");

            var dbConnection = Configuration.GetConnectionString("HashtagDatabase");
            dbConnection = dbConnection.Replace("[server]", Environment.GetEnvironmentVariable("instatagger_mysql_ip"));
            dbConnection = dbConnection.Replace("[user]", Environment.GetEnvironmentVariable("instatagger_mysql_user"));
            dbConnection = dbConnection.Replace("[pw]", Environment.GetEnvironmentVariable("instatagger_mysql_pw"));
            dbConnection = dbConnection.Replace("[db]", Environment.GetEnvironmentVariable("instatagger_mysql_db"));
            services.AddDbContext<InstaqContext>(options =>
            {
                options.UseMySql(dbConnection);
                options.UseLoggerFactory(loggerFactory);
            });

            services.AddTransient<IEvaluationService, EvaluationService>();

            services.AddTransient<IEvaluationStorage, MysqlEvaluationStorage>();
            services.AddTransient<ILogUploadsStorage, MysqlLogUploadsStorage>();
            services.AddTransient<IFeedbackStorage, MysqlFeedbackStorage>();
            services.AddTransient<ILogHashtagSearchStorage, MysqlLogHashtagSearchStorage>();
            services.AddTransient<ICustomerStorage, MysqlCustomerStorage>();
            services.AddTransient<IDebugStorage, MysqlDebugStorage>();

            services.AddTransient<ITaggingProvider, GcpVision>();
            services.AddTransient<IFileHandler, DiskFileHander>();
            services.AddTransient<IEvaluation, Evaluation>();

            // https://stackoverflow.com/questions/48590579/cannot-resolve-scoped-service-from-root-provider-net-core-2
            services.AddSingleton<RequestResponseLoggingMiddleware>();
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddScoped<ILogSystem, MysqlLogSystem>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Instaq Extern", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILoggerFactory loggerfactory,
                              IApplicationLifetime appLifetime)
        {
            app.UseDeveloperExceptionPage();

            loggerfactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instaq API v1"); });
            GlobalSettings.Environment = env.EnvironmentName;

            app.UseCors(options => options.WithOrigins(
                    "http://instaq-api.innocliq.de",
                    "https://instaq-api.innocliq.de",
                    "http://instaq-api-dev.innocliq.de",
                    "https://instaq-api-dev.innocliq.de"
                )
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("info/health");
                endpoints.MapControllers();
            });
        }
    }
}
