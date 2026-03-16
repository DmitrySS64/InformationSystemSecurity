using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.Tests.SpNet;

public class MagicSquareTests
{
    public static IEnumerable<object[]> TestDataForMagicSquareEncrypt()
    {
        const string @in = "АБВГДЕЖЗИЙКЛМНОП";
        yield return [@in, MagicSquare.Default1, "ПВБМДЙКЗИЕЖЛГОНА"];
        yield return [@in, MagicSquare.Default2, "ЖНГИЛАОЕМЗЙВБКДП"];
        yield return [@in, MagicSquare.Default3, "ГНОАИЖЕЛДКЙЗПБВМ"];
    }

    public static IEnumerable<object[]> TestDataForMagicSquareDecrypt()
    {
        const string @in = "АБВГДЕЖЗИЙКЛМНОП";
        yield return [@in, MagicSquare.Default1, MagicSquare.Encrypt(@in, MagicSquare.Default1)];
        yield return [@in, MagicSquare.Default2, MagicSquare.Encrypt(@in, MagicSquare.Default2)];
        yield return [@in, MagicSquare.Default3, MagicSquare.Encrypt(@in, MagicSquare.Default3)];
    }

    [Theory]
    [MemberData(nameof(TestDataForMagicSquareEncrypt))]
    public void Encrypt_ReturnsCorrectEncryptedString(string @in, int[][] square, string expected)
    {
        var result = MagicSquare.Encrypt(@in, square);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(TestDataForMagicSquareDecrypt))]
    public void Decrypt_ReturnsOriginalString(string expected, int[][] square, string @in)
    {
        var result = MagicSquare.Decrypt(@in, square);
        Assert.Equal(expected, result);
    }
}

