using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Accounts;
using MediatR;
using System.Data;
using static ForeignCard.Model.Accounts.DepositAccountModel;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Accounts;

public class DepositAccountHandler : IRequestHandler<DepositAccountRequest, DepositAccountResponse>
{
    private readonly IDbConnectionFactory _factory;

    public DepositAccountHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<DepositAccountResponse> Handle(DepositAccountRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@AccountId", request.AccountId);
        p.Add("@Amount", request.Amount);
        p.Add("@Description", request.Description);

        return await conn.QuerySingleAsync<DepositAccountModel.DepositAccountResponse>(ProcedureConstants.SP_AccountDeposit, p, commandType: CommandType.StoredProcedure);
    }
}