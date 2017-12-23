using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Reflection;
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
            if (context.Request.Method == "POST" && context.Request.ContentType == "application/json")
            {
                try
                {
                    context.Request.EnableRewind();
                    string jsonData = new StreamReader(context.Request.Body).ReadToEnd();
                    JsonObject jsonDoc = (JsonObject)JsonObject.Parse(jsonData);

                    var entity = Program.Entities.Where(x => x.FullName == "PlataAlfa.api.V1_0.Users");
                    if (entity.Count() != 0)
                    {
                        MethodInfo action = entity.FirstOrDefault().GetMethod("GetAll");

                        if (action != null)
                        {
                            object result = null;
                            ParameterInfo[] parameters = action.GetParameters();
                            object classInstance = Activator.CreateInstance(entity.FirstOrDefault(), null);

                            if (parameters.Length == 0)
                            {
                                result = action.Invoke(classInstance, null);
                            }
                            else
                            {
                                object[] parametersArray = new object[] { jsonData };           
                                result = action.Invoke(classInstance, parametersArray);
                            }

                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync((string)result);

                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync("Resource action not found!");
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Resource entity not found!");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(ex.Message);
                }
               
            }
            else
            {
                await this._next(context);
            }
        }
    }
}
