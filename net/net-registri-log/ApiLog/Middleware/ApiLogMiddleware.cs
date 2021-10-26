using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using net_registri_log.ApiLog.Models;
using net_registri_log.Shared.ExtensionMethods;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace net_registri_log.ApiLog.Middleware
{
    /// <summary>
    /// Middleware traccia alcune info http api.
    /// </summary>
    public class ApiLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLogMiddleware> _logger;

        public ApiLogMiddleware(RequestDelegate next, ILogger<ApiLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RegistriLogDbContext registriLogDbContext)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogDebug("ApiLogMiddleware invoked.");

            // add request log
            ApiObject apiObject = new ApiObject()
            {
                Date = DateTime.Now,
                Method = context.Request.Method,
                Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                QueryString = context.Request.QueryString.Value
            };
            apiObject.RequestBody = ValidaECorreggiStringToJson(await context.GetRequestRawBodyAsync());
            if (!string.IsNullOrWhiteSpace(apiObject.RequestBody))
            {
                apiObject.RequestSize = (float)context.Request.Body.Length / 1024;
            }

            // save for get id
            registriLogDbContext.ApiLogs.Add(apiObject);
            await registriLogDbContext.SaveChangesAsync();

            //_logger.LogInformationPushProperty(
            //    "Request tracked on EventApi table.",
            //    data: new {Id = apiObject.Id},
            //    operazione: OperazioneLogsEnum.AuditApi.Name());

            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            try
            {
                await _next(context);
            }
            finally
            {
                newBody.Seek(0, SeekOrigin.Begin);
                apiObject.ResponseBody = ValidaECorreggiStringToJson(await new StreamReader(context.Response.Body).ReadToEndAsync());

                if (!string.IsNullOrWhiteSpace(apiObject.ResponseBody))
                {
                    apiObject.ResponseSize = (float)context.Response.Body.Length / 1024;
                }
                apiObject.UserId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }

            watch.Stop();
            apiObject.ElapsedMilliseconds = watch.ElapsedMilliseconds;

            // update apiLog
            registriLogDbContext.ApiLogs.Update(apiObject);
            await registriLogDbContext.SaveChangesAsync();
        }

        private static string ValidaECorreggiStringToJson(string str)
        {
            try
            {
                JsonConvert.DeserializeObject(str);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                str = JsonConvert.SerializeObject(new { Body = str });
            }

            return str;
        }
    }
}
