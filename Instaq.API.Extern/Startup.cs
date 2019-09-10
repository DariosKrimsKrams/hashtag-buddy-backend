namespace Instaq.UserInterface
{
    using Instaq.Contract;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql;
    using Instaq.Evaluation.Standard;
    using Instaq.FileHandling.Standard;
    using Instaq.ImageProcessor.Standard;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instaq API v1");
                });

            app.UseCors(options => options.WithOrigins(
                "http://localhost:4200",
                "http://instaq.innocliq.de"
                ).AllowAnyMethod());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            services.AddTransient<IEvaluationStorage, MysqlEvaluationStorage>();
            services.AddTransient<ILogStorage, MysqlLogStorage>();
            services.AddTransient<IFeedbackStorage, MysqlFeedbackStorage>();
            services.AddTransient<ICustomerStorage, MysqlCustomerStorage>();
            services.AddTransient<IDebugStorage, MysqlDebugStorage>();

            services.AddTransient<ITaggingProvider, GcpVision>();
            services.AddTransient<IFileHandler, DiskFileHander>();
            services.AddTransient<IEvaluation, Evaluation>();

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Instaq Extern", Version = "v1" });
                });
        }
    }
}
