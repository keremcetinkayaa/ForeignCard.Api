using System.Data.Common;
using System.Text.Json;
using ForeignCard.Common.Exceptions;

namespace ForeignCard.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
                throw;

            var (statusCode, message) = Map(ex);

            Console.WriteLine(">>> Yakalanan hata: " + ex.Message);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(new { statusCode, message });
            await context.Response.WriteAsync(body);
        }
    }

    private static (int, string) Map(Exception ex)
    {
        if (ex is AccountLockedException)
            return (429, ex.Message);

        var sqlNumber = FindSqlErrorNumber(ex);

        return sqlNumber switch
        {
            2627 or 2601 => (409, "Bu kayit zaten mevcut."),
            547 => (422, "Iliskili kayit bulunamadi."),
            _ => (500, "Beklenmeyen bir hata olustu.")
        };
    }

    private static int? FindSqlErrorNumber(Exception? ex)
    {
        while (ex != null)
        {
            if (ex is DbException)
            {
                var number = ex.GetType().GetProperty("Number")?.GetValue(ex);
                if (number is int n)
                    return n;
            }
            ex = ex.InnerException;
        }
        return null;
    }
}