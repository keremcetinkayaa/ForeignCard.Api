using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Transactions;
using MediatR;

namespace ForeignCard.Operation.Transactions;

public class GetCustomerTransactionsHandler
    : IRequestHandler<GetCustomerTransactions.GetCustomerTransactionsRequest, GetCustomerTransactions.GetCustomerTransactionsResponse>
{
    private readonly IDbConnectionFactory _factory;

    public GetCustomerTransactionsHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetCustomerTransactions.GetCustomerTransactionsResponse> Handle(GetCustomerTransactions.GetCustomerTransactionsRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);
        p.Add("@Top", request.Top);

        var rows = await conn.QueryAsync<GetCustomerTransactions.CustomerTransactionItem>(ProcedureConstants.SP_TransactionListByCustomer, p, commandType: CommandType.StoredProcedure);

        return new GetCustomerTransactions.GetCustomerTransactionsResponse { Transactions = rows.ToList() };
    }
}