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
    private readonly ICacheService _cache;

    public GetExchangeRatesHandler(IDbConnectionFactory factory, ICacheService cache)
    {
        _factory = factory;
        _cache = cache;
    }

    public async Task<GetExchangeRateModel.GetExchangeRateResponse> Handle(GetExchangeRateModel.GetExchangeRateRequest request, CancellationToken cancellationToken)
    {
        // Önce Redis'e bak; yoksa veritabanından oku ve 60 dakikalığına kaydet
        return await _cache.GetOrSetAsync(CacheKeyConstans.ExchangeRateList,
            async () =>
            {
                using var conn = _factory.CreateConnection();

                var rows = await conn.QueryAsync<GetExchangeRateModel.GetExchangeRateItem>(ProcedureConstants.SP_ExchangeRateList, commandType: CommandType.StoredProcedure);

                return new GetExchangeRateModel.GetExchangeRateResponse { Rate = rows.ToList() };
            },
            CacheDurations.ExchangeRate);
    }
}