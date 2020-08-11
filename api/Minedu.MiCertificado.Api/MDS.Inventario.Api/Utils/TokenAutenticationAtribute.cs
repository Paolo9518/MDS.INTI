using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net;

namespace MDS.Inventario.Api.Utils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TokenAutenticationAtribute : ActionFilterAttribute
    {
        private string Name { get => "TOKEN"; }
        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());
        //TRUE = CREARÁ TOKEN, 
        //FALSE = EVALUARÁ TOKEN
        public bool CreateToken { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            var memoryCacheKey = $"{Name}-{ipAddress}";

            if (CreateToken == true) //CREAMOS TOKEN:
            {
                //ELIMINAMOS LA CACHE
                Cache.Remove(memoryCacheKey);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));//60 SEGUNDOS EXISTIRÁ TOKEN PARA ESE IP

                //CREAMOS LA CACHE CON 60 SEGUNDOS DE EXPIRACIÓN
                Cache.Set(memoryCacheKey, true, cacheEntryOptions);
            }
            else //EVALUAMOS TOKEN:
            {
                if (Cache.TryGetValue(memoryCacheKey, out bool entry))
                {
                    //BORRAMOS LA CACHE PARA QUE NO VUELVA HACER UTILIZADA
                    Cache.Remove(memoryCacheKey);
                }
                else
                {
                    context.Result = new ContentResult
                    {
                        Content = $"TOKEN INVÁLIDO",
                    };
                    //511 Network Authentication Required
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NetworkAuthenticationRequired;
                }
            }
        }

    }
}
