using FluentValidation;
using ForeignCard.Model.Customers;

namespace ForeignCard.Operation.Customers;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerModel.CustomerCreateRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.NationalId).NotEmpty().Length(11).Matches("^[0-9]+$");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.PhoneNo).MaximumLength(20);
        RuleFor(x =>x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}