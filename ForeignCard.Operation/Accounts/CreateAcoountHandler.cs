using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Accounts;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Accounts;

public class CreateAccountHandler : IRequestHandler<CreateAccountModel.AccountCreateRequest, CreateAccountModel.AccountCreateResponse>
{
    private readonly IDbConnectionFactory _factory;

    public CreateAccountHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<CreateAccountModel.AccountCreateResponse> Handle(CreateAccountModel.AccountCreateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);
        p.Add("@AccountNo", request.AccountNo);
        p.Add("@CurrencyCode", request.CurrencyCode);
        p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(ProcedureConstants.SP_AccountAdd, p, commandType: CommandType.StoredProcedure);

        return new CreateAccountModel.AccountCreateResponse { Id = p.Get<int>("@NewId") };
    }
}