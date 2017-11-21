using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData.Builder;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleODataApp.Models;

namespace SampleODataApp
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
            services.AddDbContext<SampleODataDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
            });

            services.AddOData();
            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>(nameof(Person));

            app.UseMvc(routebuilder =>
            {
                routebuilder.MapODataRoute("odata", builder.GetEdmModel());
            });
        }
    }
}
