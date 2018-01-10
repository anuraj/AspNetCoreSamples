using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebEssentials.AspNetCore.Pwa;

namespace HelloPWA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddMvc();
            var pwaoptions = new PwaOptions(){
                RoutesToPreCache = @"/lib/bootstrap/dist/css/bootstrap.css,
                        /lib/jquery/dist/jquery.js,
                        /lib/bootstrap/dist/js/bootstrap.js,
                        /css/site.css,
                        /js/site.js,
                        /images/banner1.svg,
                        /images/banner2.svg,
                        /images/banner3.svg,
                        /images/banner4.svg"
            };

            services.AddProgressiveWebApp(pwaoptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
