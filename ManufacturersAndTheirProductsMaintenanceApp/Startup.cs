using System;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManufacturersAndTheirProductsMaintenanceApp
{
    public class Startup
    {
        public static readonly Guid UserGuid = Guid.NewGuid();

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MFRsAndProductsContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("MFRsAndProductsDbConnectionString"));
            });

            services.AddTransient<MFRsAndProductsSeeder>();

            services.AddScoped<IMFRsAndProductsRepository, MFRsAndProductsRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MFRsAndProductsSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" }
                );
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                seeder = scope.ServiceProvider.GetService<MFRsAndProductsSeeder>();
                seeder.Seed();
            }
        }
    }
}
