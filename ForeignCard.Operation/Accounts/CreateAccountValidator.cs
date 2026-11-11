using FluentValidation;
using ForeignCard.Model.Accounts;

namespace ForeignCard.Operation.Accounts;

public class CreateAccountValidator : AbstractValidator<CreateAccountModel.AccountCreateRequest>
{
    public CreateAccountValidator() 
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.AccountNo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CurrencyCode).NotEmpty().Length(3);
    }
}