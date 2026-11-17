using FluentValidation;
using ForeignCard.Common.Constants;
using ForeignCard.Model.Customers;

namespace ForeignCard.Operation.Customers;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerModel.CustomerUpdateRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);

        RuleFor(x => x.FirstName).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.Email).EmailAddress().MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNo).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.PhoneNo));

        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.FirstName)|| !string.IsNullOrWhiteSpace(x.LastName)|| !string.IsNullOrWhiteSpace(x.PhoneNo)|| !string.IsNullOrWhiteSpace(x.Email)|| x.IsActive != null).WithMessage(ValidatorConstans.EnterField);
    }
}