using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Common.Constants
{
    #region Country Constants
    public static class CountryConstants
    {
        public static readonly (string Code, string Name, string Currency)[] Countries =
        {
            ("USA", "ABD", "USD"),
            ("DEU", "Almanya", "EUR"),
            ("FRA", "Fransa", "EUR"),
            ("ITA", "İtalya", "EUR"),
            ("ESP", "İspanya", "EUR"),
            ("NLD", "Hollanda", "EUR"),
            ("BEL", "Belçika", "EUR"),
            ("AUT", "Avusturya", "EUR"),
            ("GRC", "Yunanistan", "EUR"),
            ("PRT", "Portekiz", "EUR"),
            ("IRL", "İrlanda", "EUR"),
            ("FIN", "Finlandiya", "EUR"),
            ("GBR", "Birleşik Krallık", "GBP"),
            ("AUS", "Avustralya", "AUD"),
            ("DNK", "Danimarka", "DKK"),
            ("CHE", "İsviçre", "CHF"),
            ("SWE", "İsveç", "SEK"),
            ("CAN", "Kanada", "CAD"),
            ("KWT", "Kuveyt", "KWD"),
            ("NOR", "Norveç", "NOK"),
            ("SAU", "Suudi Arabistan", "SAR"),
            ("JPN", "Japonya", "JPY"),
            ("BGR", "Bulgaristan", "BGN"),
            ("ROU", "Romanya", "RON"),
            ("RUS", "Rusya", "RUB"),
            ("CHN", "Çin", "CNY"),
            ("PAK", "Pakistan", "PKR"),
            ("QAT", "Katar", "QAR"),
            ("KOR", "Güney Kore", "KRW"),
            ("AZE", "Azerbaycan", "AZN"),
            ("ARE", "Birleşik Arap Emirlikleri", "AED")
        };
    }
    #endregion
}
