using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Countries;
using MediatR;

namespace ForeignCard.Operation.Countries;

public class GetCountriesHandler : IRequestHandler<GetCountryModel.GetCountryRequest, GetCountryModel.GetCountryResponse>
{
    private readonly IDbConnectionFactory _factory;
    private readonly ICacheService _cache;

    public GetCountriesHandler(IDbConnectionFactory factory, ICacheService cache)
    {
        _factory = factory;
        _cache = cache;
    }

    public async Task<GetCountryModel.GetCountryResponse> Handle(GetCountryModel.GetCountryRequest request, CancellationToken cancellationToken)
    {
        // Önce Redis'e bak; yoksa veritabanından oku ve 24 saatliğine kaydet
        return await _cache.GetOrSetAsync(CacheKeyConstans.CountryList, async () =>
            {
                using var conn = _factory.CreateConnection();

                var rows = await conn.QueryAsync<GetCountryModel.GetCountryItem>(ProcedureConstants.SP_CountryList, commandType: CommandType.StoredProcedure);

                return new GetCountryModel.GetCountryResponse { Countries = rows.ToList() };
            },
            CacheDurations.Country);
    }
}