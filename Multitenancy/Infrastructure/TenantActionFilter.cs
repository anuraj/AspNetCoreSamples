using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webmarks.xyz.Models;
using System.Linq;
using System;

namespace webmarks.xyz.Infrastructure
{
    public class TenantAttribute : ActionFilterAttribute
    {
        private readonly WebMarksDbContext _webMarksDbContext;

        public TenantAttribute(WebMarksDbContext webMarksDbContext)
        {
            _webMarksDbContext = webMarksDbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            var fullAddress = actionExecutingContext.HttpContext?.Request?
                .Headers?["Host"].ToString()?.Split('.');
            if (fullAddress.Length < 2)
            {
                actionExecutingContext.Result = new StatusCodeResult(404);
                base.OnActionExecuting(actionExecutingContext);
            }
            else
            {
                var subdomain = fullAddress[0];
                var tenant = _webMarksDbContext.Tenants
                    .SingleOrDefault(t => string.Equals(t.Host, subdomain, StringComparison.OrdinalIgnoreCase));
                if (tenant != null)
                {
                    actionExecutingContext.RouteData.Values.Add("tenant", tenant);
                    base.OnActionExecuting(actionExecutingContext);
                }
                else
                {
                    actionExecutingContext.Result = new StatusCodeResult(404);
                    base.OnActionExecuting(actionExecutingContext);
                }
            }
        }
    }
}