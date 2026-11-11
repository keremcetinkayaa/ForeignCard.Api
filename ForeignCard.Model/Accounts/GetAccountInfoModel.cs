using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Accounts
{
    public class GetAccountInfoModel
    {
        public class GetAccountInfoRequest : IRequest<GetAccountInfoResponse>
        {
            public int AccountId { get; set; }
        }
        public class GetAccountInfoResponse
        {
            public int AccountId { get; set; }
            public int CustomerId { get; set; }
            public string AccountNo { get; set; } = "";
            public string CurrencyCode { get; set; } = "";
            public decimal Balance { get; set; }
            public string Status { get; set; } = "";
        }
    }
}
