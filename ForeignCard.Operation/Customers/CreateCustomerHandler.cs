using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Customers;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Customers;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerModel.CustomerCreateRequest, CreateCustomerModel.CustomerCreateResponse>
{
    private readonly IDbConnectionFactory _factory;

    public CreateCustomerHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<CreateCustomerModel.CustomerCreateResponse> Handle(CreateCustomerModel.CustomerCreateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@FirstName", request.FirstName);
        p.Add("@LastName", request.LastName);
        p.Add("@NationalId", request.NationalId);
        p.Add("@PhoneNo", request.PhoneNo);
        p.Add("@Email", request.Email);
        p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerAdd, p, commandType: CommandType.StoredProcedure);

        var newId = p.Get<int>("@NewId");

        var pwd = new DynamicParameters();
        pwd.Add("@CustomerId", newId);
        pwd.Add("@PasswordHash", ForeignCard.Common.Helpers.PasswordHelper.Hash(request.Password));
        pwd.Add("@Role", "CUSTOMER");

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerSetPassword, pwd, commandType: CommandType.StoredProcedure);

        return new CreateCustomerModel.CustomerCreateResponse { Id = p.Get<int>("@NewId") };
    }
}