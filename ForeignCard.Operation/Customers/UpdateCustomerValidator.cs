using FluentValidation;
using ForeignCard.Model.Customers;
using static ForeignCard.Model.Customers.CreateCustomerModel;

namespace ForeignCard.Operation.Customers;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerModel.CustomerUpdateRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.PhoneNo).MaximumLength(20);
    }
}