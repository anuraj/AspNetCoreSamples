using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPageSample.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "Hello World - ASP.NET Core Pages";
        }

        public void OnPost()
        {
            Console.WriteLine("Reached the post method");
            Message = "Hello World - This is Post";
        }
    }
}