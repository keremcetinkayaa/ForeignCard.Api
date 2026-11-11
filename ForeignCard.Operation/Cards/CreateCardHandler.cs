using System.Data;
using System.Data.Common;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Common.Helpers;
using ForeignCard.Model.Cards;
using MediatR;

namespace ForeignCard.Operation.Cards;

public class CreateCardHandler : IRequestHandler<CreateCardModel.CreateCardRequest, CreateCardModel.CreateCardResponse>
{
    private const int MaxAttempts = 5;
    private readonly IDbConnectionFactory _factory;

    public CreateCardHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<CreateCardModel.CreateCardResponse> Handle(CreateCardModel.CreateCardRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        for (int attempt = 1; ; attempt++)
        {
            var cardNo = CardNumberGenerator.Generate();

            var p = new DynamicParameters();
            p.Add("@AccountId", request.AccountId);
            p.Add("@CardNo", cardNo);
            p.Add("@AllowForeignUse", request.AllowForeignUse);
            p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@NewExpiryDate", dbType: DbType.Date, direction: ParameterDirection.Output);

            try
            {
                await conn.ExecuteAsync(ProcedureConstants.SP_CardCreate, p, commandType: CommandType.StoredProcedure);

                return new CreateCardModel.CreateCardResponse
                {
                    Id = p.Get<int>("@NewId"),
                    CardNo = cardNo,
                    ExpiryDate = p.Get<DateTime>("@NewExpiryDate")
                };
            }
            catch (Exception ex) when (IsDuplicate(ex) && attempt < MaxAttempts)
            {
                // Aynı numara tesadüfen üretildi, yenisini dene
            }
        }
    }

    // Veritabanı "bu kayıt zaten var" hatası mı? (2627 / 2601)
    private static bool IsDuplicate(Exception ex)
    {
        if (ex is not DbException) return false;
        var number = ex.GetType().GetProperty("Number")?.GetValue(ex);
        return number is 2627 or 2601;
    }
}