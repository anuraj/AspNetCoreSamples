using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BreadcrumbDemo.Models;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;

namespace BreadcrumbDemo.Controllers;

[DefaultBreadcrumb]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Breadcrumb(FromAction = "Index", Title = "Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Breadcrumb("ViewData.Title")]
    public IActionResult About()
    {
        return View();
    }

    public IActionResult Article(int id)
    {
        var articlesPage = new MvcBreadcrumbNode("ArticleIndex", "Home", "Articles");
        var articlePage = new MvcBreadcrumbNode("Article", "Home", $"Article - {id}") { Parent = articlesPage };
        ViewData["BreadcrumbNode"] = articlePage;
        ViewData["Title"] = $"Article - {id}";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
