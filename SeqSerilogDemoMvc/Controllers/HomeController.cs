using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeqSerilogDemoMvc.Models;

namespace SeqSerilogDemoMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Hello from the Index action!");
        // if (new Random().Next(0, 10) > 5)
        // {
        //     throw new Exception("Random exception");
        // }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // var exceptionHandlerPathFeature =
        //     HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        // if (exceptionHandlerPathFeature != null)
        // {
        //     _logger.LogError(exceptionHandlerPathFeature.Error, "An error occurred at {Path}", exceptionHandlerPathFeature.Path);
        // }
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
