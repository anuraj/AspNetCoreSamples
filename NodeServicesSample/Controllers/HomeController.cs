using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SampleApp.Models;
using Microsoft.AspNetCore.NodeServices;
using IO = System.IO;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add([FromServices] INodeServices nodeServices)
        {
            var num1 = 10;
            var num2 = 20;
            var result = await nodeServices.InvokeAsync<int>("AddModule.js", num1, num2);
            ViewData["ResultFromNode"] = $"Result of {num1} + {num2} is {result}";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateUrlPreview([FromServices] INodeServices nodeServices)
        {
            var url = Request.Form["Url"].ToString();
            var fileName = System.IO.Path.ChangeExtension(DateTime.UtcNow.Ticks.ToString(), "png");
            var file = await nodeServices.InvokeAsync<string>("UrlPreviewModule.js", url, IO.Path.Combine("PreviewImages", fileName));

            return Content($"<a class=\"btn btn-default\" target=\"_blank\" href=\"/Home/Download?img={fileName}\">Download image</a>");
        }

        public IActionResult Download()
        {
            var image = Request.Query["img"].ToString();
            var fileName = IO.Path.Combine("PreviewImages", image);
            var isExists = IO.File.Exists(fileName);

            if (isExists)
            {
                Response.Headers.Add($"Content-Disposition", "attachment; filename=\"" + image + "\"");
                var bytes = IO.File.ReadAllBytes(fileName);
                return File(bytes, "image/png");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [AjaxOnly]
        public IActionResult HelloWorld()
        {
            return Json(new { Name = "Anuraj", Email = "anuraj.p@example.com" });
        }
        [AjaxOnly]
        public IActionResult HtmlContent()
        {
            return Content("<b>This is bold</b><button>Hello World</button>");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }
    }
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            return routeContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}
