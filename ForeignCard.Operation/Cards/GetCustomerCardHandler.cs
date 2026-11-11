using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Cards;
using MediatR;

namespace ForeignCard.Operation.Cards;

public class GetCustomerCardHandler
    : IRequestHandler<GetCustomerCardModel.GetCustomerCardRequest, GetCustomerCardModel.GetCustomerCardResponse>
{
    private readonly IDbConnectionFactory _factory;

    public GetCustomerCardHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetCustomerCardModel.GetCustomerCardResponse> Handle(
        GetCustomerCardModel.GetCustomerCardRequest request,
        CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);

        var rows = await conn.QueryAsync<GetCustomerCardModel.GetCustomerCardItem>(
            ProcedureConstants.SP_CardListByCustomer, p,
            commandType: CommandType.StoredProcedure);

        return new GetCustomerCardModel.GetCustomerCardResponse { Cards = rows.ToList() };
    }
}