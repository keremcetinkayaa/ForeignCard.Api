using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.ExchangeRates;
using MediatR;

namespace ForeignCard.Operation.ExchangeRates;

public class GetExchangeRatesHandler : IRequestHandler<GetExchangeRateModel.GetExchangeRateRequest, GetExchangeRateModel.GetExchangeRateResponse>
{
    private readonly IDbConnectionFactory _factory;

    public GetExchangeRatesHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetExchangeRateModel.GetExchangeRateResponse> Handle(GetExchangeRateModel.GetExchangeRateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var rows = await conn.QueryAsync<GetExchangeRateModel.GetExchangeRateItem>(ProcedureConstants.SP_ExchangeRateList, commandType: CommandType.StoredProcedure);

        return new GetExchangeRateModel.GetExchangeRateResponse { Rate = rows.ToList() };
    }
}