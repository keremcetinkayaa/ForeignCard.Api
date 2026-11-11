using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.ExchangeRates
{
    public class SetExchangeRateModel
    {
        public class SetExchangeRateRequest : IRequest<SetExchangeRateResponse>
        {
            public string CurrencyCode { get; set; } = "";
            public decimal Rate { get; set; }
        }

        public class SetExchangeRateResponse
        {
            public string CurrencyCode { get; set; } = "";
            public decimal Rate { get; set; }
        }

    }
}
