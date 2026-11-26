using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Common.Exceptions;
using ForeignCard.Common.Helpers;
using ForeignCard.Model.Auth;
using MediatR;

namespace ForeignCard.Operation.Auth;

public class LoginHandler : IRequestHandler<LoginModel.LoginRequest, LoginModel.LoginResponse?>
{
    private readonly IDbConnectionFactory _factory;
    private readonly ITokenService _tokenService;

    public LoginHandler(IDbConnectionFactory factory, ITokenService tokenService)
    {
        _factory = factory;
        _tokenService = tokenService;
    }

    public async Task<LoginModel.LoginResponse?> Handle(LoginModel.LoginRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        // 1. Kilit kontrolü: kilitliyse şifreye hiç bakma (admin ve müşteri için ortak)
        var lockParams = new DynamicParameters();
        lockParams.Add("@NationalId", request.NationalId);

        var lockInfo = await conn.QuerySingleAsync<LockRow>(ProcedureConstants.SP_LoginAttemptCheckLock, lockParams, commandType: CommandType.StoredProcedure);

        if (lockInfo.IsLocked && lockInfo.LockedUntil.HasValue)
        {
            var minutesLeft = (int)Math.Ceiling((lockInfo.LockedUntil.Value - DateTime.UtcNow).TotalMinutes);
            throw new AccountLockedException(Math.Max(minutesLeft, 1));
        }

        var p = new DynamicParameters();
        p.Add("@NationalId", request.NationalId);

        // 2. Önce admin tablosuna bak
        var admin = await conn.QuerySingleOrDefaultAsync<LoginModel.AdminUserRow>(ProcedureConstants.SP_AdminLogin, p, commandType: CommandType.StoredProcedure);

        if (admin != null)
        {
            var adminValid = !string.IsNullOrEmpty(admin.PasswordHash) && PasswordHelper.Verify(request.Password, admin.PasswordHash);

            await AddAttemptAsync(conn, request.NationalId, null, adminValid);

            if (!adminValid)
                return null;

            return CreateResponse(admin.AdminId, admin.FirstName + " " + admin.LastName, RoleConstants.Admin);
        }

        // 3. Admin değilse müşteri tablosuna bak
        var user = await conn.QuerySingleOrDefaultAsync<LoginModel.LoginUserRow>(ProcedureConstants.SP_CustomerLogin, p, commandType: CommandType.StoredProcedure);

        var isValid = user != null && !string.IsNullOrEmpty(user.PasswordHash) && PasswordHelper.Verify(request.Password, user.PasswordHash);

        await AddAttemptAsync(conn, request.NationalId, user?.CustomerId, isValid);

        if (!isValid)
            return null;

        return CreateResponse(user!.CustomerId, user.FirstName + " " + user.LastName, RoleConstants.Customer);
    }

    // Token üretip cevabı hazırlar (admin ve müşteri için ortak)
    private LoginModel.LoginResponse CreateResponse(int userId, string fullName, string role)
    {
        return new LoginModel.LoginResponse
        {
            Token = _tokenService.CreateToken(userId, fullName, role),
            UserId = userId,
            FullName = fullName,
            Role = role
        };
    }

    private static async Task AddAttemptAsync(IDbConnection conn, string nationalId, int? customerId, bool isSuccess)
    {
        var p = new DynamicParameters();
        p.Add("@NationalId", nationalId);
        p.Add("@CustomerId", customerId);
        p.Add("@IsSuccess", isSuccess);

        await conn.ExecuteAsync(ProcedureConstants.SP_LoginAttemptAdd, p, commandType: CommandType.StoredProcedure);
    }

    private class LockRow
    {
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public int FailCount { get; set; }
    }
}