using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Cards
{
    public class CreateCardModel
    {
        public class CreateCardRequest : IRequest<CreateCardResponse>
        {
            public int AccountId { get; set; }
            public string CardNo { get; set; } = "";
            public DateTime ExpiryDate { get; set; }
            public bool AllowForeignUse { get; set; }
        }

        public class CreateCardResponse
        {
            public int Id { get; set; }
        }
    }
}
