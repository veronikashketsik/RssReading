using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RssReading.Data;
using RssReading.Data.Repositories;
using RssReading.Domain;
using RssReading.Domain.Contracts;
using RssReading.Domain.Data;
using AutoMapper;
using RssReading.Web.MapperProfiles;

namespace EFDataApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Mapper.Initialize(config => 
            {
                config.AddProfiles(typeof(RssRepository).Assembly);
                config.AddProfiles(typeof(FilterProfile).Assembly);
            });

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RssContext>(options => options.UseSqlServer(connection));
            services.AddMvc();
            this.ConfigureRssServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "rss", action = "index" });
            });
        }

        private void ConfigureRssServices(IServiceCollection services)
        {
            services.AddScoped<IRssService, RssService>();
            services.AddScoped<IRssRepository, RssRepository>();
        }
    }
}