using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Countries;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Countries;

public class GetCountriesHandler : IRequestHandler<GetCountryModel.GetCountryRequest, GetCountryModel.GetCountryResponse>
{
    private readonly IDbConnectionFactory _factory;

    public GetCountriesHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetCountryModel.GetCountryResponse> Handle(GetCountryModel.GetCountryRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var rows = await conn.QueryAsync<GetCountryModel.GetCountryItem>(ProcedureConstants.SP_CountryList, commandType: CommandType.StoredProcedure);

        return new GetCountryModel.GetCountryResponse { Countries = rows.ToList() };
    }
}