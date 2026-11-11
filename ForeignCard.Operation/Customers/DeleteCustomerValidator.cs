using FluentValidation;
using ForeignCard.Model.Customers;
using static ForeignCard.Model.Customers.CreateCustomerModel;

namespace ForeignCard.Operation.Customers;

public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerModel.CustomerDeleteRequest>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
    }
}