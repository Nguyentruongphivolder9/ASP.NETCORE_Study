using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CheckAccessMiddleware
{
    private readonly RequestDelegate _next;
    public CheckAccessMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/textxxx")
        {
            Console.WriteLine("CheckAccessMiddleware: Cấm truy cập");
            await Task.Run(
                async () =>
                {
                    string html = "<h1>CẤM KHÔNG ĐƯỢC TRUY CẬP</h1>";
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync(html);
                }
            );
        }
        else
        {
            context.Response.Headers.Add("throughCheckAccessMiddleware", new[] { DateTime.Now.ToString() });
            Console.WriteLine("CheckAccessMiddleware: Cho phép truy cập");
            // Chuyển Middleware tiếp theo trong pipeline
            await _next(context);
        }

    }
}