using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Cards;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Cards;

public class UpdateCardHandler : IRequestHandler<UpdateCardModel.UpdateCardRequest, UpdateCardModel.UpdateCardResponse>
{
    private readonly IDbConnectionFactory _factory;

    public UpdateCardHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<UpdateCardModel.UpdateCardResponse> Handle(UpdateCardModel.UpdateCardRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var status = request.Status.ToUpperInvariant();

        var p = new DynamicParameters();
        p.Add("@CardId", request.CardId);
        p.Add("@ExpiryDate", request.ExpiryDate, DbType.Date);
        p.Add("@Status", status);
        p.Add("@AllowForeignUse", request.AllowForeignUse);

        await conn.ExecuteAsync(ProcedureConstants.SP_CardUpdate, p, commandType: CommandType.StoredProcedure);

        return new UpdateCardModel.UpdateCardResponse
        {
            CardId = request.CardId,
            ExpiryDate = request.ExpiryDate,
            Status = status,
            AllowForeignUse = request.AllowForeignUse
        };
    }
}