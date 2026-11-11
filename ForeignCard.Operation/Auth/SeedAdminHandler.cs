using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Helpers;
using ForeignCard.Model.Auth;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Auth;

public class SeedAdminHandler : IRequestHandler<SeedAdminModel.AdminRequest, SeedAdminModel.AdminResponse>
{
    private readonly IDbConnectionFactory _factory;

    public SeedAdminHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    private class UserRow
    {
        public int CustomerId { get; set; }
        public string? PasswordHash { get; set; }
    }

    public async Task<SeedAdminModel.AdminResponse> Handle(SeedAdminModel.AdminRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@NationalId", request.NationalId);

        var user = await conn.QuerySingleOrDefaultAsync<UserRow>(ProcedureConstants.SP_CustomerLogin, p, commandType: CommandType.StoredProcedure);

        if (user != null && !string.IsNullOrEmpty(user.PasswordHash))
            return new SeedAdminModel.AdminResponse { Created = false };

        int customerId;
        if (user != null)
        {
            customerId = user.CustomerId;
        }
        else
        {
            var add = new DynamicParameters();
            add.Add("@FirstName", "Sistem");
            add.Add("@LastName", "Yonetici");
            add.Add("@NationalId", request.NationalId);
            add.Add("@PhoneNo", null);
            add.Add("@Email", null);
            add.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync(ProcedureConstants.SP_CustomerAdd, add, commandType: CommandType.StoredProcedure);
            customerId = add.Get<int>("@NewId");
        }

        var pwd = new DynamicParameters();
        pwd.Add("@CustomerId", customerId);
        pwd.Add("@PasswordHash", PasswordHelper.Hash(request.Password));
        pwd.Add("@Role", "ADMIN");

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerSetPassword, pwd, commandType: CommandType.StoredProcedure);

        return new SeedAdminModel.AdminResponse { Created = true };
    }
}