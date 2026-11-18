using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Customers;
using MediatR;
using System.Data;

namespace ForeignCard.Operation.Customers;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerModel.CustomerDeleteRequest, DeleteCustomerModel.CustomerDeleteResponse>
{
    private readonly IDbConnectionFactory _factory;
    private readonly ICacheService _cache;

    public DeleteCustomerHandler(IDbConnectionFactory factory, ICacheService cache)
    {
        _factory = factory;
        _cache = cache;
    }

    public async Task<DeleteCustomerModel.CustomerDeleteResponse> Handle(DeleteCustomerModel.CustomerDeleteRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerDelete, p, commandType: CommandType.StoredProcedure);

        await _cache.RemoveAsync(CacheKeyConstans.Customer(request.CustomerId));

        return new DeleteCustomerModel.CustomerDeleteResponse
        {
            CustomerId = request.CustomerId,
            IsActive = false
        };
    }
}