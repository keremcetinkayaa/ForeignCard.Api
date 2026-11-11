using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Common.Constants
{
    public static class TableConstants
    {
        #region Tables
        public const string Customer = "Core.CUSTOMER";
        public const string Account = "Core.ACCOUNT";
        public const string AccountMovement = "Core.ACCOUNTMOVEMENT";
        public const string Card = "Core.CARD";
        public const string Transactions = "Txn.TRANSACTIONS";
        public const string ExchangeRate = "Fx.EXCHANGERATE";
        public const string Country = "Fx.COUNTRY";
        #endregion

        #region History Tables
        public const string CustomerHistory = "Hist.CUSTOMERHISTORY";
        public const string AccountHistory = "Hist.ACCOUNTHISTORY";
        public const string CardHistory = "Hist.CARDHISTORY";
        public const string ApiLog = "Hist.APILOG";
        #endregion
    }
}
