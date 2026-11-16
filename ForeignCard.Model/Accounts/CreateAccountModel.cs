using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Accounts
{
    public class CreateAccountModel
    {
        public class AccountCreateRequest : IRequest<AccountCreateResponse>
        {
            public int CustomerId { get; set; }
            public string CurrencyCode { get; set; } = "";
        }

        public class AccountCreateResponse
        {
            public int Id { get; set; }
            public string AccountNo { get; set; } = "";
        }
    }
}
