using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Accounts
{
    public class GetAccountStatementModel
    {
        public class GetAccountStatementRequest : IRequest<GetAccountStatementResponse?>
        {
            public int AccountId { get; set; }
        }

        public class GetAccountStatementResponse
        {
            public int AccountId { get; set; }
            public decimal CurrentBalance { get; set; }
            public List<GetAccountStatementMovement> Movements { get; set; } = new();
        }

        public class GetAccountStatementMovement
        {
            public long MovementId { get; set; }
            public DateTime MovementDate { get; set; }
            public string Direction { get; set; } = "";
            public decimal Amount { get; set; }
            public decimal BalanceAfter { get; set; }
            public string? Description { get; set; }
        }
    }
}
