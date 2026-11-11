using System.Text.RegularExpressions;
using ForeignCard.Common.Abstractions;

namespace ForeignCard.Api.Middleware;

public class ApiLogMiddleware
{
    private readonly RequestDelegate _next;

    public ApiLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IApiLogWriter logWriter)
    {
        var path = context.Request.Path.Value ?? "";

        if (!path.StartsWith("/api"))
        {
            await _next(context);
            return;
        }

        context.Request.EnableBuffering();
        string requestBody;
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var originalBody = context.Response.Body;
        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        Exception? error = null;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            error = ex;
            throw;
        }
        finally
        {
            buffer.Position = 0;
            var responseBody = await new StreamReader(buffer).ReadToEndAsync();
            buffer.Position = 0;
            await buffer.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            var statusCode = error != null ? 500 : context.Response.StatusCode;
            var logResponse = error != null ? "HATA: " + error.Message : responseBody;

            try
            {
                await logWriter.WriteAsync(
                    context.Request.Method,
                    path,
                    string.IsNullOrWhiteSpace(requestBody) ? null : Mask(requestBody),
                    string.IsNullOrWhiteSpace(logResponse) ? null : Mask(logResponse),
                    statusCode);
            }
            catch (Exception logEx)
            {
                Console.WriteLine(">>> Log yazma hatasi: " + logEx.Message);
            }
        }
    }

    private static string Mask(string text)
    {
        // 16 haneli kart numarası: sadece son 4 hane görünsün
        text = Regex.Replace(text, @"\b\d{12}(\d{4})\b", "************$1");

        // "password" içeren alanların değeri gizlensin (password, newPassword vb.)
        text = Regex.Replace(text, "(\"\\w*password\"\\s*:\\s*\")[^\"]*(\")", "$1***$2", RegexOptions.IgnoreCase);

        return text;
    }
}