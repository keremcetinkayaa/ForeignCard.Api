using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model;
using ForeignCard.Model.ExchangeRates;
using MediatR;
using System.Data;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.ExchangeRates;

public class SetExchangeRateHandler : IRequestHandler<SetExchangeRateModel.SetExchangeRateRequest, SetExchangeRateModel.SetExchangeRateResponse>
{
    private readonly IDbConnectionFactory _factory;

    public SetExchangeRateHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<SetExchangeRateModel.SetExchangeRateResponse> Handle(SetExchangeRateModel.SetExchangeRateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var code = request.CurrencyCode.ToUpperInvariant();

        var p = new DynamicParameters();
        p.Add("@CurrencyCode", code);
        p.Add("@Rate", request.Rate);

        await conn.ExecuteAsync(ProcedureConstants.SP_ExchangeRateSet, p, commandType: CommandType.StoredProcedure);

        return new SetExchangeRateModel.SetExchangeRateResponse
        {
            CurrencyCode = code,
            Rate = request.Rate
        };
    }
}