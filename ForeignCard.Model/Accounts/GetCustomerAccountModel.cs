using MediatR;

namespace ForeignCard.Model.Accounts;

public class GetCustomerAccountModel
{
    public class GetCustomerAccountRequest : IRequest<GetCustomerAccountResponse>
    {
        public int CustomerId { get; set; }
    }

    public class GetCustomerAccountResponse
    {
        public List<GetCustomerAccountItem> Accounts { get; set; } = new();
    }

    public class GetCustomerAccountItem
    {
        public int AccountId { get; set; }
        public string AccountNo { get; set; } = "";
        public string CurrencyCode { get; set; } = "";
        public decimal Balance { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}