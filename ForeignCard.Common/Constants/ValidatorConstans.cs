using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Common.Constants;

public static class  ValidatorConstans
{
    public const string EnterField = "Guncellenecek en az bir alan gonderilmelidir.";
    public const string StatusError = "Status sadece ACTIVE, BLOCKED veya CLOSED olabilir.";
    public const string InvalidCurrencyCode = "Para birimi 3 harfli olmalıdır!";
}