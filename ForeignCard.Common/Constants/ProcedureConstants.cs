using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Common.Constants
{
    public static class ProcedureConstants
    {
        #region Customer
        public const string SP_CustomerAdd = "Core.CUSTOMER_ADD";
        public const string SP_CustomerUpdate = "Core.CUSTOMER_UPDATE";
        public const string SP_CustomerDelete = "Core.CUSTOMER_DELETE";
        public const string SP_CustomerGet = "Core.CUSTOMER_GET";
        public const string SP_CustomerLogin = "Core.CUSTOMER_LOGIN";
        public const string SP_CustomerSetPassword = "Core.CUSTOMER_SETPASSWORD";
        #endregion

        #region Account
        public const string SP_AccountAdd = "Core.ACCOUNT_ADD";
        public const string SP_AccountUpdate = "Core.ACCOUNT_UPDATE";
        public const string SP_AccountGet = "Core.ACCOUNT_GET";
        public const string SP_AccountDeposit = "Core.ACCOUNT_DEPOSIT";
        public const string SP_AccountStatementGet = "Core.ACCOUNTSTATEMENT_GET";
        #endregion

        #region Card
        public const string SP_CardCreate = "Core.CARD_CREATE";
        public const string SP_CardUpdate = "Core.CARD_UPDATE";
        public const string SP_CardListForSimulation = "Core.CARDLISTFORSIMULATION";
        #endregion

        #region Transaction
        public const string SP_TransactionAdd = "Txn.TRANSACTIONS_ADD";
        public const string SP_TransactionList = "Txn.TRANSACTIONS_LIST";
        public const string SP_TransactionListByCustomer = "Txn.TRANSACTIONS_LISTBYCUSTOMER";
        #endregion

        #region ExchangeRate
        public const string SP_ExchangeRateSet = "Fx.EXCHANGERATE_SET";
        public const string SP_ExchangeRateList = "Fx.EXCHANGERATE_LIST";
        #endregion

        #region Country
        public const string SP_CountrySet = "Fx.COUNTRY_SET";
        public const string SP_CountryList = "Fx.COUNTRY_LIST";
        #endregion

        #region Log
        public const string SP_ApiLogAdd = "Hist.APILOG_ADD";
        #endregion

        #region Login
        public const string SP_LoginAttemptAdd = "Hist.LOGINATTEMPT_ADD";
        public const string SP_LoginAttemptCheckLock = "Hist.LOGINATTEMPT_CHECKLOCK";
        #endregion
    }
}
