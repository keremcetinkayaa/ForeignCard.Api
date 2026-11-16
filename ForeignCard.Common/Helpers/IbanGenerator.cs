using System.Security.Cryptography;
using System.Text;

namespace ForeignCard.Common.Helpers;

public static class IbanGenerator
{
    // Projeye ait hayali banka kodu (gerçek bir bankaya ait değil)
    private const string BankCode = "99999";

    // TR + 2 kontrol hanesi + 5 banka kodu + 1 rezerv (0) + 16 hesap no = 26 karakter
    public static string Generate()
    {
        var account = new StringBuilder(16);
        for (int i = 0; i < 16; i++)
            account.Append(RandomNumberGenerator.GetInt32(0, 10));

        var bban = BankCode + "0" + account;          // 22 hane
        var checkDigits = CalculateCheckDigits(bban);

        return "TR" + checkDigits + bban;
    }

    // IBAN mod-97: BBAN'ın sonuna "TR00" eklenir, harfler sayıya çevrilir
    // (T=29, R=27), kalan hesaplanır, kontrol = 98 - kalan
    private static string CalculateCheckDigits(string bban)
    {
        var numeric = bban + "29" + "27" + "00";

        int remainder = 0;
        foreach (var ch in numeric)
            remainder = (remainder * 10 + (ch - '0')) % 97;

        return (98 - remainder).ToString("D2");
    }
}