using MediatR;

namespace ForeignCard.Model.Transactions;

#region CreateTransaction
public class CreateTransaction
{
    public class CreateTransactionRequest : IRequest<CreateTransactionResponse>
    {
        public string CardNo { get; set; } = "";
        public decimal Amount { get; set; }
        public string CurrencyNumericCode { get; set; } = "";
    }

    public class CreateTransactionResponse
    {
        public long TransactionId { get; set; }
        public string CurrencyCode { get; set; } = "";
        public decimal OriginalAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
#endregion

#region GetCustomerTransaction
public class GetCustomerTransactions
{
    public class GetCustomerTransactionsRequest : IRequest<GetCustomerTransactionsResponse>
    {
        public int CustomerId { get; set; }
        public int Top { get; set; } = 50;
    }

    public class GetCustomerTransactionsResponse
    {
        public List<CustomerTransactionItem> Transactions { get; set; } = new();
    }

    public class CustomerTransactionItem
    {
        public long TransactionId { get; set; }
        public int CardId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal OriginalAmount { get; set; }
        public string CurrencyCode { get; set; } = "";
        public decimal ExchangeRate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
#endregion

#region GetTransaction
public class GetTransactions
{
    public class GetTransactionsRequest : IRequest<GetTransactionsResponse>
    {
        public int Top { get; set; } = 50;
    }

    public class GetTransactionsResponse
    {
        public List<GetTransactionsItem> Transactions { get; set; } = new();
    }

    public class GetTransactionsItem
    {
        public long TransactionId { get; set; }
        public int CardId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal OriginalAmount { get; set; }
        public string CurrencyCode { get; set; } = "";
        public decimal ExchangeRate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
#endregion