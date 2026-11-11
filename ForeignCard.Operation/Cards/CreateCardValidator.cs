using FluentValidation;
using ForeignCard.Model.Cards;

namespace ForeignCard.Operation.Cards;

public class CreateCardValidator : AbstractValidator<CreateCardModel.CreateCardRequest>
{
    public CreateCardValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0);
    }
}