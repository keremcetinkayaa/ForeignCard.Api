using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Transactions;
using MediatR;
using System.Data;
using ForeignCard.Common.Constants;
using static ForeignCard.Model.Transactions.CreateTransaction;

namespace ForeignCard.Operation.Transactions;

public class GetTransactionsHandler : IRequestHandler<GetTransactions.GetTransactionsRequest, GetTransactions.GetTransactionsResponse>
{
    private readonly IDbConnectionFactory _factory;

    public GetTransactionsHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetTransactions.GetTransactionsResponse> Handle(GetTransactions.GetTransactionsRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@Top", request.Top);

        var rows = await conn.QueryAsync<GetTransactions.GetTransactionsItem>(ProcedureConstants.SP_TransactionList, p, commandType: CommandType.StoredProcedure);

        return new GetTransactions.GetTransactionsResponse { Transactions = rows.ToList() };
    }
}