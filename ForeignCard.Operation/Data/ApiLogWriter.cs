using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;


namespace ForeignCard.Operation.Data;

public class ApiLogWriter : IApiLogWriter
{
    private readonly IDbConnectionFactory _factory;

    public ApiLogWriter(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task WriteAsync(string httpMethod, string path, string? request, string? response, int statusCode)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@HttpMethod", httpMethod);
        p.Add("@Path", path);
        p.Add("@Request", request);
        p.Add("@Response", response);
        p.Add("@StatusCode", statusCode);

        await conn.ExecuteAsync(ProcedureConstants.SP_ApiLogAdd, p, commandType: CommandType.StoredProcedure);
    }
}