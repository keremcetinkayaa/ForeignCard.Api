namespace ForeignCard.Common.Abstractions;

public interface IApiLogWriter
{
    Task WriteAsync(string httpMethod, string path, string? request, string? response, int statusCode);
}