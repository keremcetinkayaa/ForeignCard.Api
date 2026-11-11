using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Accounts;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Accounts;

public class GetAccountStatementHandler : IRequestHandler<GetAccountStatementModel.GetAccountStatementRequest, GetAccountStatementModel.GetAccountStatementResponse?>
{
    private readonly IDbConnectionFactory _factory;

    public GetAccountStatementHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetAccountStatementModel.GetAccountStatementResponse?> Handle(GetAccountStatementModel.GetAccountStatementRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@AccountId", request.AccountId);

        using var multi = await ((Microsoft.Data.SqlClient.SqlConnection)conn).QueryMultipleAsync(ProcedureConstants.SP_AccountStatementGet, p, commandType: CommandType.StoredProcedure);

        var response = await multi.ReadSingleOrDefaultAsync<GetAccountStatementModel.GetAccountStatementResponse>();
        if (response == null)
            return null;

        var movements = await multi.ReadAsync<GetAccountStatementModel.GetAccountStatementMovement>();
        response.Movements = movements.ToList();

        return response;
    }
}