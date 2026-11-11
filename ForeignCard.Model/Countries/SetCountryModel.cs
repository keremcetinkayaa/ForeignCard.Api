using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Countries
{
    public class SetCountryModel
    {
        public class SetCountryRequest : IRequest<SetCountryResponse>
        {
            public string CountryCode { get; set; } = "";
            public string CountryName { get; set; } = "";
            public string CurrencyCode { get; set; } = "";
        }

        public class SetCountryResponse
        {
            public string CountryCode { get; set; } = "";
        }
    }
}
