using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksApi.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace BooksApi
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
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddDbContext<BooksApiDbContext>(options => options.UseInMemoryDatabase("BooksDb"));
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IAntiforgery antiforgery, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(next => context =>
            {
                if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(context.Request.Path.Value, "/create", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions() { HttpOnly = false });
                }

                return next(context);
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Angular Routing",
                    template: "{*url}",
                    defaults: new { controller = "Spa", action = "SpaFallback" }
                );

                routes.MapRoute(
                    name: "DefaultApi",
                    template: "api/{controller}"
                );
            });
        }
    }
}
