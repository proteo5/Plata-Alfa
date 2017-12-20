using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;


namespace PlataAlfa.core
{
    public class PlataAlfaMiddleware
    {
        private readonly RequestDelegate _next;

        public PlataAlfaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "POST")
            {
                context.Request.EnableRewind();
                string jsonData = new StreamReader(context.Request.Body).ReadToEnd();
                JsonObject jsonDoc = (JsonObject)JsonObject.Parse(jsonData);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonData);
            }
            else
            {
                await this._next(context);
            }
        }
    }
}
