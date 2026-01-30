using System.Diagnostics;
using System.Text;

namespace ReservationService.Infrastructure.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestBody = await ReadRequestBodyAsync(context.Request);
        
        _logger.LogInformation(
            "HTTP Request: {Method} {Path} {QueryString} | Body: {RequestBody} | IP: {RemoteIpAddress}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            requestBody,
            context.Connection.RemoteIpAddress);

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            var responseBodyContent = await ReadResponseBodyAsync(context.Response);
            
            _logger.LogInformation(
                "HTTP Response: {StatusCode} | {Method} {Path} | Elapsed: {ElapsedMilliseconds}ms | Response: {ResponseBody}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds,
                responseBodyContent);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        if (!request.ContentLength.HasValue || request.ContentLength.Value == 0)
        {
            return string.Empty;
        }

        request.EnableBuffering();
        
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var bodyAsText = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        
        return bodyAsText.Length > 1000 
            ? bodyAsText[..1000] + "... (truncated)" 
            : bodyAsText;
    }

    private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        
        
        return text.Length > 1000 
            ? text[..1000] + "... (truncated)" 
            : text;
    }
}