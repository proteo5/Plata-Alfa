using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;

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
                    dynamic pk = JsonConvert.DeserializeObject(jsonData);

                    string version = pk.Version;
                    string area = pk.Area == null ? string.Empty : $"{pk.Area}.";
                    string entity = pk.Entity;
                    string action = pk.Action;
                    var data = pk.Data == null ? null : pk.Data;
                    string path = $"PlataAlfa.api.V{version.ToString().Replace('.', '_')}.";
                    path += $"{area}{entity}";

                    var entityObj = Program.Entities.Where(x => x.FullName == path);
                    if (entityObj.Count() != 0)
                    {
                        MethodInfo actionMethod = entityObj.FirstOrDefault().GetMethod(action);

                        if (actionMethod != null)
                        {
                            Envelope result = null;
                            ParameterInfo[] parameters = actionMethod.GetParameters();
                            object classInstance = Activator.CreateInstance(entityObj.FirstOrDefault(), null);

                            if (parameters.Length == 0)
                            {
                                result = (Envelope) actionMethod.Invoke(classInstance, null);
                            }
                            else
                            {
                                object[] parametersArray = new object[] { data };
                                result = (Envelope) actionMethod.Invoke(classInstance, parametersArray);
                            }

                            string json = JsonConvert.SerializeObject(result);

                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(json);

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
