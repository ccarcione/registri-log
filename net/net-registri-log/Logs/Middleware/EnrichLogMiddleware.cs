using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Linq;
using System.Threading.Tasks;

namespace net_registri_log.Logs.Middleware
{
    public class EnrichLogMiddleware
    {
        private readonly RequestDelegate next;

        public EnrichLogMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            LogContext.PushProperty("UserName", string.Concat(
                context.User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                " ",
                context.User.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value));

            return next(context);
        }
    }
}
