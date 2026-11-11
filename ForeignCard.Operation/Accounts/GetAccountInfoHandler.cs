using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Accounts;
using MediatR;
using System.Data;
using static ForeignCard.Model.Accounts.GetAccountInfoModel;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Accounts;

public class GetAccountInfoHandler : IRequestHandler<GetAccountInfoModel.GetAccountInfoRequest, GetAccountInfoModel.GetAccountInfoResponse?>
{
    private readonly IDbConnectionFactory _factory;

    public GetAccountInfoHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetAccountInfoModel.GetAccountInfoResponse?> Handle(GetAccountInfoModel.GetAccountInfoRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@AccountId", request.AccountId);

        return await conn.QuerySingleOrDefaultAsync<GetAccountInfoModel.GetAccountInfoResponse>(ProcedureConstants.SP_AccountGet, p, commandType: CommandType.StoredProcedure);
    }
}