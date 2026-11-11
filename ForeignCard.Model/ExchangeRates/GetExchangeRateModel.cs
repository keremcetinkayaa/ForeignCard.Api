using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.ExchangeRates
{
    public class GetExchangeRateModel
    {
        public class GetExchangeRateRequest : IRequest<GetExchangeRateResponse>
        {
        }

        public class GetExchangeRateResponse
        {
            public List<GetExchangeRateItem> Rate { get; set; } = new();
        }
        public class GetExchangeRateItem
        {
            public string CurrencyCode { get; set; } = "";
            public decimal Rate { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}
