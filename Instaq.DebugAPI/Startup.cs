namespace Instaq.API.Debug
{
    using AutoTagger.Contract.Storage;
    using AutoTagger.Database.Storage.Mysql;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
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
                "http://localhost:4201",
                "http://instaq-debug.innocliq.de"
            ).AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute( name: "default", template: "{controller=Home}/{action=Index}/{id?}" );
                } );
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded    = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                } );

            services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );

            services.AddTransient<IDebugStorage, MysqlDebugStorage>();

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Instaq Debug", Version = "v1" });
                });
        }
    }
}
