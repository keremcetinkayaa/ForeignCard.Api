using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Customers;
using MediatR;
using System.Data;
using static ForeignCard.Model.Customers.CreateCustomerModel;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Customers;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerModel.CustomerDeleteRequest, DeleteCustomerModel.CustomerDeleteResponse>
{
    private readonly IDbConnectionFactory _factory;

    public DeleteCustomerHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<DeleteCustomerModel.CustomerDeleteResponse> Handle(DeleteCustomerModel.CustomerDeleteRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerDelete, p, commandType: CommandType.StoredProcedure);

        return new DeleteCustomerModel.CustomerDeleteResponse
        {
            CustomerId = request.CustomerId,
            IsActive = false
        };
    }
}