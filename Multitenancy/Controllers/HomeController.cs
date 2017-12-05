using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webmarks.xyz.Infrastructure;
using webmarks.xyz.Models;

namespace webmarks.xyz.Controllers
{
    [ServiceFilter(typeof(TenantAttribute))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var tenant = RouteData.Values.SingleOrDefault(r => r.Key == "tenant");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
