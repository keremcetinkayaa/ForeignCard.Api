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

        // 1. Kilit kontrolü: kilitliyse şifreye hiç bakma
        var lockParams = new DynamicParameters();
        lockParams.Add("@NationalId", request.NationalId);

        var lockInfo = await conn.QuerySingleAsync<LockRow>(
            ProcedureConstants.SP_LoginAttemptCheckLock, lockParams,
            commandType: CommandType.StoredProcedure);

        if (lockInfo.IsLocked && lockInfo.LockedUntil.HasValue)
        {
            var minutesLeft = (int)Math.Ceiling((lockInfo.LockedUntil.Value - DateTime.UtcNow).TotalMinutes);
            throw new AccountLockedException(Math.Max(minutesLeft, 1));
        }

        // 2. Kullanıcıyı getir
        var p = new DynamicParameters();
        p.Add("@NationalId", request.NationalId);

        var user = await conn.QuerySingleOrDefaultAsync<LoginModel.LoginUserRow>(
            ProcedureConstants.SP_CustomerLogin, p,
            commandType: CommandType.StoredProcedure);

        // 3. Şifre kontrolü; başarısızsa denemeyi kaydet
        var isValid = user != null
                      && !string.IsNullOrEmpty(user.PasswordHash)
                      && PasswordHelper.Verify(request.Password, user.PasswordHash);

        await AddAttemptAsync(conn, request.NationalId, user?.CustomerId, isValid);

        if (!isValid)
            return null;

        // 4. Başarılı giriş: token üret
        var fullName = user!.FirstName + " " + user.LastName;
        var token = _tokenService.CreateToken(user.CustomerId, fullName, user.Role.Trim());

        return new LoginModel.LoginResponse
        {
            Token = token,
            CustomerId = user.CustomerId,
            FullName = fullName,
            Role = user.Role.Trim()
        };
    }

    private static async Task AddAttemptAsync(IDbConnection conn, string nationalId, int? customerId, bool isSuccess)
    {
        var p = new DynamicParameters();
        p.Add("@NationalId", nationalId);
        p.Add("@CustomerId", customerId);
        p.Add("@IsSuccess", isSuccess);

        await conn.ExecuteAsync(ProcedureConstants.SP_LoginAttemptAdd, p,
            commandType: CommandType.StoredProcedure);
    }

    private class LockRow
    {
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public int FailCount { get; set; }
    }
}