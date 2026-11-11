using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Accounts;
using MediatR;

namespace ForeignCard.Operation.Accounts;

public class GetCustomerAccountHandler
    : IRequestHandler<GetCustomerAccountModel.GetCustomerAccountRequest, GetCustomerAccountModel.GetCustomerAccountResponse>
{
    private readonly IDbConnectionFactory _factory;

    public GetCustomerAccountHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetCustomerAccountModel.GetCustomerAccountResponse> Handle(
        GetCustomerAccountModel.GetCustomerAccountRequest request,
        CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);

        var rows = await conn.QueryAsync<GetCustomerAccountModel.GetCustomerAccountItem>(
            ProcedureConstants.SP_AccountListByCustomer, p,
            commandType: CommandType.StoredProcedure);

        return new GetCustomerAccountModel.GetCustomerAccountResponse{ Accounts = rows.ToList() };
    }
}