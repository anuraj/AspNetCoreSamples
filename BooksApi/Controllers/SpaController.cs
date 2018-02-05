using System.Linq;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Controllers
{
    public class SpaController : Controller
    {
        public IActionResult SpaFallback()
        {
            return File("~/index.html", "text/html");
        }
    }
}