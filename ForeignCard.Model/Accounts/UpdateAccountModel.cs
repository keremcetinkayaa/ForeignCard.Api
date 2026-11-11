using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Accounts
{
    public class UpdateAccountModel
    {
        public class AccountUpdateRequest : IRequest<AccountUpdateResponse>
        {
            public int AccountId { get; set; }
            public string Status { get; set; } = "";
        }

        public class AccountUpdateResponse
        {
            public int AccountId { get; set; }
            public string Status { get; set; } = "";
        }
    }
}
