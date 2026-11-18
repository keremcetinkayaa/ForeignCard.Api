using ForeignCard.Common.Helpers;

namespace ForeignCard.Tests.Helpers;

public class PasswordHelperTests
{
    [Fact]
    public void Verify_DogruSifre_TrueDoner()
    {
        var hash = PasswordHelper.Hash("Sifre123!");

        Assert.True(PasswordHelper.Verify("Sifre123!", hash));
    }

    [Fact]
    public void Verify_YanlisSifre_FalseDoner()
    {
        var hash = PasswordHelper.Hash("Sifre123!");

        Assert.False(PasswordHelper.Verify("Yanlis123!", hash));
    }

    [Fact]
    public void Hash_AyniSifre_HerSeferindeFarkliSonucVerir()
    {
        // Her hash'te rastgele salt üretildiği için aynı şifre iki farklı hash verir
        var hash1 = PasswordHelper.Hash("Sifre123!");
        var hash2 = PasswordHelper.Hash("Sifre123!");

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_SaltVeHashNoktaIleAyrilir()
    {
        var hash = PasswordHelper.Hash("Sifre123!");
        var parts = hash.Split('.');

        Assert.Equal(2, parts.Length);
        Assert.Equal(16, Convert.FromBase64String(parts[0]).Length); // salt: 16 bayt
        Assert.Equal(32, Convert.FromBase64String(parts[1]).Length); // hash: 32 bayt
    }

    [Fact]
    public void Verify_BozukFormat_FalseDoner()
    {
        Assert.False(PasswordHelper.Verify("Sifre123!", "noktasiz-gecersiz-deger"));
    }
}