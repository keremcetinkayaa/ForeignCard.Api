using FluentValidation;
using ForeignCard.Model.Cards;

namespace ForeignCard.Operation.Cards;

public class CreateCardValidator : AbstractValidator<CreateCardModel.CreateCardRequest>
{
    public CreateCardValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0);
        RuleFor(x => x.CardNo).NotEmpty().Length(16).Matches("^[0-9]+$");
        RuleFor(x => x.ExpiryDate).GreaterThan(DateTime.Today);
    }
}