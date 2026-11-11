using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Model.Customers;
using MediatR;
using System.Data;
using static ForeignCard.Model.Customers.CreateCustomerModel;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Customers;

public class GetCustomerInfoHandler : IRequestHandler<GetCustomerInfoModel.CustomerGetRequest, GetCustomerInfoModel.CustomerGetResponse?>
{
    private readonly IDbConnectionFactory _factory;

    public GetCustomerInfoHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<GetCustomerInfoModel.CustomerGetResponse?> Handle(GetCustomerInfoModel.CustomerGetRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);

        var customer = await conn.QuerySingleOrDefaultAsync<GetCustomerInfoModel.CustomerGetResponse>(ProcedureConstants.SP_CustomerGet, p, commandType: CommandType.StoredProcedure);

        if (customer != null && customer.NationalId.Length == 11)
            customer.NationalId = "*******" + customer.NationalId.Substring(7);

        return customer;
    }
}