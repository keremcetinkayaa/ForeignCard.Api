using System.Security.Cryptography;

namespace ForeignCard.Common.Helpers;

public static class CardNumberGenerator
{
    // 16 haneli, Luhn kontrol haneli kart numarası üretir
    public static string Generate()
    {
        var digits = new int[16];
        digits[0] = 4; // Visa tarzı başlangıç

        for (int i = 1; i < 15; i++)
            digits[i] = RandomNumberGenerator.GetInt32(0, 10);

        digits[15] = CalculateCheckDigit(digits);

        return string.Concat(digits);
    }

    // Luhn: sağdan başlayarak her ikinci haneyi 2 ile çarp, 9'dan büyükse 9 çıkar, topla
    private static int CalculateCheckDigit(int[] digits)
    {
        int sum = 0;
        for (int i = 14; i >= 0; i--)
        {
            int d = digits[i];
            if ((14 - i) % 2 == 0)
            {
                d *= 2;
                if (d > 9) d -= 9;
            }
            sum += d;
        }
        return (10 - (sum % 10)) % 10;
    }
}