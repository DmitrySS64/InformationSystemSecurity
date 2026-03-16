using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.Tests.SpNet;

public class PBlockCipherTests
{
    [Theory]
    [InlineData("ЗОЛОТАЯ_СЕРЕДИНА", 1, "ПЯУЦШВГЖ_СПВЕЖЧШ")]
    [InlineData("ЗОЛОТАЯ_СЕРЕДИНА", 2, "ЛДОИНЗСЯАЕТРЕ_АО")]
    [InlineData("ЗОЛОТАЯ_СЕРЕДИНА", 4, "ОЩКЬРСВПИЗАТВЬЛЧ")]
    public void pBlockEncrypt(string @in, int round, string expected)
    {
        var result = PBlockCipher.Encrypt(@in, round);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("ЗОЛОТАЯ_СЕРЕДИНА", 1, "ПЯУЦШВГЖ_СПВЕЖЧШ")]
    [InlineData("ЗОЛОТАЯ_СЕРЕДИНА", 2, "ЛДОИНЗСЯАЕТРЕ_АО")]
    [InlineData("ЗОЛОТАЯ_СЕРЕДИНА", 4, "ОЩКЬРСВПИЗАТВЬЛЧ")]
    public void pBlockDecrypt(string expected, int round, string @in)
    {
        var result = PBlockCipher.Decrypt(@in, round);

        Assert.Equal(expected, result);
    }
}
