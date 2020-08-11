using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MDS.Inventario.Api.Utils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestLimitDDOSAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public float Seconds { get; set; }
        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        public RequestLimitDDOSAttribute()
        {
            if (Seconds == 0)
            {
                Seconds = 1f;//VALOR POR DEFECTO
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = context.RouteData.Values["action"] as string;
            }

            var ipAddress = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            var memoryCacheKey = $"{Name}-{ipAddress}";

            if (!Cache.TryGetValue(memoryCacheKey, out bool entry))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Seconds));

                Cache.Set(memoryCacheKey, true, cacheEntryOptions);
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = $"Las solicitudes están limitadas.",
                };
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
            }
        }
    }
}
