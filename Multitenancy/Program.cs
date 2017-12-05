using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using webmarks.xyz.Models;

namespace webmarks.xyz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<WebMarksDbContext>();
                context.Database.EnsureCreated();
                if (!context.Tenants.Any())
                {
                    context.Tenants.Add(new Tenant() { Name = "Hello", Host = "hello", Style = "red.css" });
                    context.Tenants.Add(new Tenant() { Name = "Sample", Host = "sample", Style = "blue.css" });
                    context.SaveChanges();
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
