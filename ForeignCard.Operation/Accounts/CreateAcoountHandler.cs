using System.Data;
using System.Data.Common;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Common.Helpers;
using ForeignCard.Model.Accounts;
using MediatR;

namespace ForeignCard.Operation.Accounts;

public class CreateAccountHandler : IRequestHandler<CreateAccountModel.AccountCreateRequest, CreateAccountModel.AccountCreateResponse>
{
    private const int MaxAttempts = 5;
    private readonly IDbConnectionFactory _factory;

    public CreateAccountHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<CreateAccountModel.AccountCreateResponse> Handle(CreateAccountModel.AccountCreateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        for (int attempt = 1; ; attempt++)
        {
            var accountNo = IbanGenerator.Generate();

            var p = new DynamicParameters();
            p.Add("@CustomerId", request.CustomerId);
            p.Add("@AccountNo", accountNo);
            p.Add("@CurrencyCode", request.CurrencyCode.Trim().ToUpperInvariant());
            p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                await conn.ExecuteAsync(ProcedureConstants.SP_AccountAdd, p, commandType: CommandType.StoredProcedure);

                return new CreateAccountModel.AccountCreateResponse
                {
                    Id = p.Get<int>("@NewId"),
                    AccountNo = accountNo
                };
            }
            catch (Exception ex) when (IsDuplicate(ex) && attempt < MaxAttempts)
            {
                // Aynı IBAN tesadüfen üretildi, yenisini dene
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