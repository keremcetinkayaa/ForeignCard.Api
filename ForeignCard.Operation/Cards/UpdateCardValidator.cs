using FluentValidation;
using ForeignCard.Common.Enums;
using ForeignCard.Model.Cards;

namespace ForeignCard.Operation.Cards;

public class UpdateCardValidator : AbstractValidator<UpdateCardModel.UpdateCardRequest>
{
    public UpdateCardValidator()
    {
        RuleFor(x => x.CardId).GreaterThan(0);

        RuleFor(x => x.Status).Must(s => Enum.TryParse<EntityStatus>(s, true, out _)).WithMessage("Status sadece ACTIVE, BLOCKED veya CLOSED olabilir.").When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Status) || x.AllowForeignUse.HasValue).WithMessage("Guncellenecek en az bir alan gonderilmelidir.");
    }
}