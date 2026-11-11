using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Customers;
using MediatR;
using System.Data;
using static ForeignCard.Model.Customers.CreateCustomerModel;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Customers;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerModel.CustomerUpdateRequest, UpdateCustomerModel.CustomerUpdateResponse>
{
    private readonly IDbConnectionFactory _factory;

    public UpdateCustomerHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<UpdateCustomerModel.CustomerUpdateResponse> Handle(UpdateCustomerModel.CustomerUpdateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);
        p.Add("@FirstName", request.FirstName);
        p.Add("@LastName", request.LastName);
        p.Add("@PhoneNo", request.PhoneNo);
        p.Add("@Email", request.Email);
        p.Add("@IsActive", request.IsActive);

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerUpdate, p, commandType: CommandType.StoredProcedure);

        return new UpdateCustomerModel.CustomerUpdateResponse
        {
            CustomerId = request.CustomerId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNo = request.PhoneNo,
            Email = request.Email,
            IsActive = request.IsActive
        };
    }
}