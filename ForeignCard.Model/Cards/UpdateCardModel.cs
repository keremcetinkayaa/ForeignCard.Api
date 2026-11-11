using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Cards
{
    public class UpdateCardModel
    {
        public class UpdateCardRequest : IRequest<UpdateCardResponse>
        {
            public int CardId { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string Status { get; set; } = "";
            public bool AllowForeignUse { get; set; }
        }

        public class UpdateCardResponse
        {
            public int CardId { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string Status { get; set; } = "";
            public bool AllowForeignUse { get; set; }
        }
    }
}
