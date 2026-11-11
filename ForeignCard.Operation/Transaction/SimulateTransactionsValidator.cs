using FluentValidation;
using ForeignCard.Model.Transactions;

namespace ForeignCard.Operation.Transactions;

public class SimulateTransactionsValidator : AbstractValidator<SimulateTransactions.SimulateTransactionsRequest>
{
    public SimulateTransactionsValidator()
    {
        RuleFor(x => x.Count).InclusiveBetween(1, 100);
    }
}