using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForeignCard.Common.Constants;

namespace ForeignCard.Common.Helpers
{
    public static class MoneyHelper
    {
        public static decimal ToTry(decimal amount, decimal rate)
            =>Math.Round(amount * rate, 4);
        public static decimal CalculateCommission(decimal amount)
            => Math.Round(amount * CommissionConstants.Rate, 4);

    }
}
