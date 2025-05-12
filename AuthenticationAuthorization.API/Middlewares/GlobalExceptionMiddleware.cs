using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.API.Helpers;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Enable buffering so we can read request body multiple times
            context.Request.EnableBuffering();

            string requestBody = "";
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // Capture response body by swapping the stream
            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Copy the contents back to the original stream
            await responseBodyStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            // Log successful requests
            await LoggerHelper.LogRequestAsync(context, "Success", requestBody, responseBody);
        }
        catch (Exception ex)
        {
            await LoggerHelper.LogRequestAsync(context, "Error", "", "", ex);

            // Optionally, you can customize the response here:
            // context.Response.StatusCode = 500;
            // await context.Response.WriteAsync("An unexpected error occurred.");

            throw; // rethrow to let server handle it or customize as above
        }
    }
}
