using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Accounts;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Accounts;

public class UpdateAccountHandler : IRequestHandler<UpdateAccountModel.AccountUpdateRequest, UpdateAccountModel.AccountUpdateResponse>
{
    private readonly IDbConnectionFactory _factory;

    public UpdateAccountHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<UpdateAccountModel.AccountUpdateResponse> Handle(UpdateAccountModel.AccountUpdateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var status = request.Status.ToUpperInvariant();

        var p = new DynamicParameters();
        p.Add("@AccountId", request.AccountId);
        p.Add("@Status", status);

        await conn.ExecuteAsync(ProcedureConstants.SP_AccountUpdate, p, commandType: CommandType.StoredProcedure);

        return new UpdateAccountModel.AccountUpdateResponse
        {
            AccountId = request.AccountId,
            Status = status
        };
    }
}