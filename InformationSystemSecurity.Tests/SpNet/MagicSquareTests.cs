using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.Tests.SpNet;

public class MagicSquareTests
{
    public static IEnumerable<object[]> TestDataForMagicSquareEncrypt()
    {
        var @in = "АБВГДЕЖЗИЙКЛМНОП";
        yield return new object[] { @in, MagicSquare.Default1, "ПВБМДЙКЗИЕЖЛГОНА" };
        yield return new object[] { @in, MagicSquare.Default2, "ЖНГИЛАОЕМЗЙВБКДП" };
        yield return new object[] { @in, MagicSquare.Default3, "ГНОАИЖЕЛДКЙЗПБВМ" };
    }

    public static IEnumerable<object[]> TestDataForMagicSquareDecrypt()
    {
        var @in = "АБВГДЕЖЗИЙКЛМНОП";
        yield return new object[] { @in, MagicSquare.Default1, MagicSquare.Encrypt(@in, MagicSquare.Default1) };
        yield return new object[] { @in, MagicSquare.Default2, MagicSquare.Encrypt(@in, MagicSquare.Default2) };
        yield return new object[] { @in, MagicSquare.Default3, MagicSquare.Encrypt(@in, MagicSquare.Default3) };
    }

    [Theory]
    [MemberData(nameof(TestDataForMagicSquareEncrypt))]
    public void Encrypt(string @in, int[][] square, string expected)
    {
        var result = MagicSquare.Encrypt(@in, square);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(TestDataForMagicSquareDecrypt))]
    public void Decrypt(string expected, int[][] square, string @in)
    {
        var result = MagicSquare.Decrypt(@in, square);
        Assert.Equal(expected, result);
    }
}

