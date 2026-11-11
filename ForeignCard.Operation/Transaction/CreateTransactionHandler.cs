using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Helpers;
using ForeignCard.Model.Transactions;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Transactions;

public class CreateTransactionHandler : IRequestHandler<CreateTransaction.CreateTransactionRequest, CreateTransaction.CreateTransactionResponse>
{
    private readonly IDbConnectionFactory _factory;

    public CreateTransactionHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<CreateTransaction.CreateTransactionResponse> Handle(CreateTransaction.CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var currencyCode = CurrencyCode.ToAlphaCode(request.CurrencyNumericCode);
        if (currencyCode == null)
            throw new InvalidOperationException("Desteklenmeyen para birimi kodu");

        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CardNo", request.CardNo);
        p.Add("@OriginalAmount", request.Amount);
        p.Add("@CurrencyCode", currencyCode);

        var response = await conn.QuerySingleAsync<CreateTransaction.CreateTransactionResponse>(ProcedureConstants.SP_TransactionAdd, p, commandType: CommandType.StoredProcedure);

        return response;
    }
}