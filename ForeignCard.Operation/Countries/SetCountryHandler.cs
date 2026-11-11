using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Countries;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Countries;

public class SetCountryHandler : IRequestHandler<SetCountryModel.SetCountryRequest, SetCountryModel.SetCountryResponse>
{
    private readonly IDbConnectionFactory _factory;

    public SetCountryHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<SetCountryModel.SetCountryResponse> Handle(SetCountryModel.SetCountryRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CountryCode", request.CountryCode);
        p.Add("@CountryName", request.CountryName);
        p.Add("@CurrencyCode", request.CurrencyCode);

        await conn.ExecuteAsync(ProcedureConstants.SP_CountrySet, p, commandType: CommandType.StoredProcedure);

        return new SetCountryModel.SetCountryResponse { CountryCode = request.CountryCode };
    }
}