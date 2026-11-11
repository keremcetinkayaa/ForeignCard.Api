using FluentValidation;
using ForeignCard.Model.Transactions;

namespace ForeignCard.Operation.Transactions;

public class CreateTransactionValidator : AbstractValidator<CreateTransaction.CreateTransactionRequest>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.CardNo).NotEmpty().Length(16).Matches("^[0-9]+$");
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.CurrencyNumericCode).NotEmpty().Length(3).Matches("^[0-9]+$");
    }
}