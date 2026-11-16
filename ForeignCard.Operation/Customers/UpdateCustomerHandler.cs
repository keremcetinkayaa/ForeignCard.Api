using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Customers;
using MediatR;

namespace ForeignCard.Operation.Customers;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerModel.CustomerUpdateRequest, UpdateCustomerModel.CustomerUpdateResponse>
{
    private readonly IDbConnectionFactory _factory;
    private readonly ICacheService _cache;

    public UpdateCustomerHandler(IDbConnectionFactory factory, ICacheService cache)
    {
        _factory = factory;
        _cache = cache;
    }

    public async Task<UpdateCustomerModel.CustomerUpdateResponse> Handle(UpdateCustomerModel.CustomerUpdateRequest request, CancellationToken cancellationToken)
    {
        using var conn = _factory.CreateConnection();

        // 1. Güncelle (boş ya da gönderilmeyen alanlar null gider, procedure eskisini korur)
        var p = new DynamicParameters();
        p.Add("@CustomerId", request.CustomerId);
        p.Add("@FirstName", Clean(request.FirstName));
        p.Add("@LastName", Clean(request.LastName));
        p.Add("@PhoneNo", Clean(request.PhoneNo));
        p.Add("@Email", Clean(request.Email));
        p.Add("@IsActive", request.IsActive);

        await conn.ExecuteAsync(ProcedureConstants.SP_CustomerUpdate, p,
            commandType: CommandType.StoredProcedure);

        // 2. Bilgi değişti: eski hali cache'ten sil
        await _cache.RemoveAsync(CacheKeyConstans.Customer(request.CustomerId));

        // 3. Güncel hali oku ve döndür
        var getParams = new DynamicParameters();
        getParams.Add("@CustomerId", request.CustomerId);

        var current = await conn.QuerySingleOrDefaultAsync<UpdateCustomerModel.CustomerUpdateResponse>(
            ProcedureConstants.SP_CustomerGet, getParams,
            commandType: CommandType.StoredProcedure);

        return current ?? new UpdateCustomerModel.CustomerUpdateResponse
        {
            CustomerId = request.CustomerId
        };
    }

    // Boş ya da sadece boşluk olan metni "gönderilmedi" say
    private static string? Clean(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}