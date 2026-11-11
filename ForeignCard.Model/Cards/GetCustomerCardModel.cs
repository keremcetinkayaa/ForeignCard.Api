using MediatR;

namespace ForeignCard.Model.Cards;

public class GetCustomerCardModel
{
    public class GetCustomerCardRequest : IRequest<GetCustomerCardResponse>
    {
        public int CustomerId { get; set; }
    }

    public class GetCustomerCardResponse
    {
        public List<GetCustomerCardItem> Cards { get; set; } = new();
    }

    public class GetCustomerCardItem
    {
        public int CardId { get; set; }
        public string CardNo { get; set; } = "";
        public int AccountId { get; set; }
        public string AccountNo { get; set; } = "";
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = "";
        public bool AllowForeignUse { get; set; }
    }
}