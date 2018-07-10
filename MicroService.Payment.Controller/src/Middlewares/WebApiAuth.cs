using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MicroService.Payment.Controller.Middlewares
{
    internal class WebApiAuth
    {
        private readonly RequestDelegate _next;

        public WebApiAuth(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var value = httpContext.Request.Query[Constants.WebApiKey];

            if(value != "9raAbgk9$%QxVeYa$28dum2C2-hQw5AP")
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("");
                return;
            }
                
            
            await _next(httpContext);
        }

    }
}