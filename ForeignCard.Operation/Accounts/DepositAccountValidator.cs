using FluentValidation;
using ForeignCard.Model.Accounts;
using static ForeignCard.Model.Accounts.DepositAccountModel;

namespace ForeignCard.Operation.Accounts;

public class DepositAccountValidator : AbstractValidator<DepositAccountRequest>
{
    public DepositAccountValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Description).MaximumLength(200);
    }
}