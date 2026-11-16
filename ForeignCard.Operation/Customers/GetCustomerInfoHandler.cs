using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Customers;
using MediatR;

namespace ForeignCard.Operation.Customers;

public class GetCustomerInfoHandler : IRequestHandler<GetCustomerInfoModel.CustomerGetRequest, GetCustomerInfoModel.CustomerGetResponse?>
{
    private readonly IDbConnectionFactory _factory;
    private readonly ICacheService _cache;

    public GetCustomerInfoHandler(IDbConnectionFactory factory, ICacheService cache)
    {
        _factory = factory;
        _cache = cache;
    }

    public async Task<GetCustomerInfoModel.CustomerGetResponse?> Handle(GetCustomerInfoModel.CustomerGetRequest request, CancellationToken cancellationToken)
    {
        // Önce Redis'e bak; yoksa veritabanından oku ve 10 dakikalığına kaydet.
        // Bulunamayan müşteri (null) cache'lenmez.
        return await _cache.GetOrSetAsync(
            CacheKeyConstans.Customer(request.CustomerId),
            async () =>
            {
                using var conn = _factory.CreateConnection();

                var p = new DynamicParameters();
                p.Add("@CustomerId", request.CustomerId);

                var customer = await conn.QuerySingleOrDefaultAsync<GetCustomerInfoModel.CustomerGetResponse>(
                    ProcedureConstants.SP_CustomerGet, p,
                    commandType: CommandType.StoredProcedure);

                // TC maskesi cache'e yazılmadan önce uygulanır
                if (customer != null && customer.NationalId.Length == 11)
                    customer.NationalId = "*******" + customer.NationalId.Substring(7);

                return customer;
            },
            CacheDurations.Customer);
    }
}