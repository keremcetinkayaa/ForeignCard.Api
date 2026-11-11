using FluentValidation;
using ForeignCard.Common.Enums;
using ForeignCard.Model.Cards;

namespace ForeignCard.Operation.Cards;

public class UpdateCardValidator : AbstractValidator<UpdateCardModel.UpdateCardRequest>
{
    public UpdateCardValidator()
    {
        RuleFor(x => x.CardId).GreaterThan(0);
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => Enum.TryParse<EntityStatus>(s, true, out _))
            .WithMessage("Status sadece ACTIVE, BLOCKED veya CLOSED olabilir.");
    }
}