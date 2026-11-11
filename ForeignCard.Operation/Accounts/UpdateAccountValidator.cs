using FluentValidation;
using ForeignCard.Common.Enums;
using ForeignCard.Model.Accounts;

namespace ForeignCard.Operation.Accounts;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountModel.AccountUpdateRequest>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0);
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => Enum.TryParse<EntityStatus>(s, true, out _))
            .WithMessage("Status sadece ACTIVE, BLOCKED veya CLOSED olabilir.");
    }
}