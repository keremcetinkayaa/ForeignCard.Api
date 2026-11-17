using FluentValidation;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Accounts;

namespace ForeignCard.Operation.Accounts;

public class CreateAccountValidator : AbstractValidator<CreateAccountModel.AccountCreateRequest>
{
    public CreateAccountValidator() 
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.CurrencyCode).NotEmpty().Length(3).WithMessage(ValidatorConstans.InvalidCurrencyCode);
    }
}