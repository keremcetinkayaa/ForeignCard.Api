using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Cards;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Cards;

public class CreateCardHandler : IRequestHandler<CreateCardModel.CreateCardRequest, CreateCardModel.CreateCardResponse>
{
    private readonly IDbConnectionFactory _factory;

    public CreateCardHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<CreateCardModel.CreateCardResponse> Handle(CreateCardModel.CreateCardRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@AccountId", request.AccountId);
        p.Add("@CardNo", request.CardNo);
        p.Add("@ExpiryDate", request.ExpiryDate, DbType.Date);
        p.Add("@AllowForeignUse", request.AllowForeignUse);
        p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(ProcedureConstants.SP_CardCreate, p, commandType: CommandType.StoredProcedure);

        return new CreateCardModel.CreateCardResponse { Id = p.Get<int>("@NewId") };
    }
}