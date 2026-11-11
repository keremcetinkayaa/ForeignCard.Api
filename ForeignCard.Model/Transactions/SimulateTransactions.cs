using MediatR;

namespace ForeignCard.Model.Transactions;

public class SimulateTransactions
{
    public class SimulateTransactionsRequest : IRequest<SimulateTransactionsResponse>
    {
        public int Count { get; set; } = 10;
    }

    public class SimulateTransactionsResponse
    {
        public int Requested { get; set; }
        public int Succeeded { get; set; }
        public int Failed { get; set; }
        public List<CreateTransaction.CreateTransactionResponse> Transactions { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
}