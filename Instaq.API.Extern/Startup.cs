namespace Instaq.API.Extern
{
    using global::API.Services;

    using Instaq.API.Extern.Helpers;
    using Instaq.API.Extern.Middleware;
    using Instaq.API.Extern.Services;
    using Instaq.API.Extern.Services.Interfaces;
    using Instaq.Contract;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql;
    using Instaq.Database.Storage.Mysql.Generated;
    using Instaq.Evaluation.Standard;
    using Instaq.FileHandling.Standard;
    using Instaq.ImageProcessor.Standard.GcpVision;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            services.AddHealthChecks().AddCheck<HealthService>("IsDbConnectionHealthy");

            var dbConnection = Configuration.GetConnectionString("HashtagDatabase");
            services.AddDbContext<InstaqProdContext>(options => options.UseMySql(dbConnection));

            services.AddTransient<IEvaluationService, EvaluationService>();
            services.AddTransient<ILoggingService, LoggingService>();

            services.AddTransient<IEvaluationStorage, MysqlEvaluationStorage>();
            services.AddTransient<ILogUploadsStorage, MysqlLogUploadsStorage>();
            services.AddTransient<IFeedbackStorage, MysqlFeedbackStorage>();
            services.AddTransient<ILogHashtagSearchStorage, MysqlLogHashtagSearchStorage>();
            services.AddTransient<ICustomerStorage, MysqlCustomerStorage>();
            services.AddTransient<IDebugStorage, MysqlDebugStorage>();

            services.AddTransient<ITaggingProvider, GcpVision>();
            services.AddTransient<IFileHandler, DiskFileHander>();
            services.AddTransient<IEvaluation, Evaluation>();
            
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Instaq Extern", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instaq API v1"); });
            GlobalSettings.Environment = env.EnvironmentName;

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
