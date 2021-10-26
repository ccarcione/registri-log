using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace net_registri_log.Shared.ExtensionMethods
{
    public static class HttpContextExtension
    {
        public static async Task<string> GetRequestRawBodyAsync(this HttpContext context, Encoding encoding = null)
        {
            context.Request.EnableBuffering();

            context.Request.Body.Position = 0;

            var reader = new StreamReader(context.Request.Body, encoding ?? Encoding.UTF8);

            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            context.Request.Body.Position = 0;

            return body;
        }
    }
}
