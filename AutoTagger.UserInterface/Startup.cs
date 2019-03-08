namespace AutoTagger.UserInterface
{
    using AutoTagger.Contract;
    using AutoTagger.Contract.Storage;
    using AutoTagger.Database.Storage.Mysql;
    using AutoTagger.Evaluation.Standard;
    using AutoTagger.FileHandling.Standard;
    using AutoTagger.ImageProcessor.Standard;
    using AutoTagger.UserInterface.Controllers.FIlter;
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(
                c =>
                {
                });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instaq API v1");
                });

            app.UseCors(options => options.WithOrigins(
                "http://localhost:4200",
                "http://localhost:80",
                "http://instaq.innocliq.de"
                ).AllowAnyMethod());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            services.AddTransient<IEvaluationStorage, MysqlEvaluationStorage>();
            services.AddTransient<ILogStorage, MysqlLogStorage>();
            services.AddTransient<ITaggingProvider, GcpVision>();
            services.AddTransient<IFileHandler, DiskFileHander>();
            services.AddTransient<IEvaluation, Evaluation>();

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Instaq Extern", Version = "v1" });
                    c.OperationFilter<FileOperation>();
                });
        }
    }
}
