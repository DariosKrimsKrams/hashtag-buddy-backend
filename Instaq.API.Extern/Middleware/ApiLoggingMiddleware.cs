namespace Instaq.API.Extern.Middleware
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Instaq.API.Extern.Models.Dtos;
    using Instaq.API.Extern.Services.Interfaces;

    // from
    // https://exceptionnotfound.net/using-middleware-to-log-requests-and-responses-in-asp-net-core/
    // https://salslab.com/a/safely-logging-api-requests-and-responses-in-asp-net-core

    public class RequestResponseLoggingMiddleware
    {
        private readonly ILoggingService loggingService;
        private readonly RequestDelegate next;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
        {
            this.next = next;
            this.loggingService = loggingService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;
            //if (!request.Path.StartsWithSegments(new PathString("/api")))
            //{
            //    await next.Invoke(httpContext);
            //    return;
            //}
            var stopWatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;
            var requestBodyContent = await ReadRequestBody(request);
            var originalBodyStream = httpContext.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                var response = httpContext.Response;
                response.Body = responseBody;
                await next.Invoke(httpContext);
                stopWatch.Stop();

                var responseBodyContent = await ReadResponseBody(response);
                await responseBody.CopyToAsync(originalBodyStream);

                SafeLog(requestTime,
                    stopWatch.ElapsedMilliseconds,
                    response.StatusCode,
                    request.Method,
                    request.Path,
                    request.QueryString.ToString(),
                    requestBodyContent,
                    responseBodyContent);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private void SafeLog(DateTime requestTime,
            long responseMillis,
            int statusCode,
            string method,
            string path,
            string queryString,
            string requestBody,
            string responseBody)
        {
            if (path.ToLower().Contains("login"))
            {
                requestBody = "(Request logging disabled for /api/login)";
                responseBody = "(Response logging disabled for /api/login)";
            }

            if (responseBody.Length > 300)
            {
                responseBody = $"(Truncated to 300 chars) {responseBody.Substring(0, 300)}";
            }

            loggingService.LogRequest(new ApiLogItem
            {
                RequestTime = requestTime,
                ResponseMillis = responseMillis,
                StatusCode = statusCode,
                Method = method,
                Path = path,
                QueryString = queryString,
                RequestBody = requestBody,
                ResponseBody = responseBody
            });
        }

    }
}
