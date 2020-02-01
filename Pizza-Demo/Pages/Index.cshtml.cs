using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Pizza_Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public ActionResult OnGetChartData()
        {
            var pizza = new[]
            {
        new {Name = "Mushrooms", Count = 3},
        new {Name = "Onions", Count = 1},
        new {Name = "Olives", Count = 1},
        new {Name = "Zucchini", Count = 1},
        new {Name = "Pepperoni", Count = 2}
    };

            var json = pizza.ToGoogleDataTable()
                            .NewColumn(new Column(ColumnType.String, "Topping"), x => x.Name)
                            .NewColumn(new Column(ColumnType.Number, "Slices"), x => x.Count)
                            .Build()
                            .GetJson();

            return Content(json);
        }
    }
}
