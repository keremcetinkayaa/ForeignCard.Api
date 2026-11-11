using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Accounts
{
    public class DepositAccountModel

    {
        public class DepositAccountRequest : IRequest<DepositAccountResponse>
        {
            public int AccountId { get; set; }
            public decimal Amount { get; set; }
            public string? Description { get; set; }
        }

        public class DepositAccountResponse
        {
            public int AccountId { get; set; }
            public decimal Amount { get; set; }
            public decimal NewBalance { get; set; }
        }
    }
}
