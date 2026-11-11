using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Cards;
using MediatR;

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

        // Boş gönderilen durum "değiştirme" sayılır; doluysa büyük harfe çevrilir
        var status = string.IsNullOrWhiteSpace(request.Status) ? null: request.Status.Trim().ToUpperInvariant();

        var p = new DynamicParameters();
        p.Add("@CardId", request.CardId);
        p.Add("@Status", status);
        p.Add("@AllowForeignUse", request.AllowForeignUse);

        // Procedure güncelleyip kartın güncel halini döndürüyor
        var current = await conn.QuerySingleOrDefaultAsync<UpdateCardModel.UpdateCardResponse>(ProcedureConstants.SP_CardUpdate, p, commandType: CommandType.StoredProcedure);

        return current ?? new UpdateCardModel.UpdateCardResponse
        {
            CardId = request.CardId
        };
    }
}