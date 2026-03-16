using InformationSystemSecurity.domain;
using InformationSystemSecurity.Domain.Lsfr;

namespace InformationSystemSecurity.Tests.SpNet;

public class SpNetTests
{
    [Theory]
    [InlineData("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "СБЖКЕНЙХЧЩЫЭЯТЬР")]
    public void RoundForward_ReturnsCorrectResult(string @in, string key, int round, string expected)
    {
        var spNet = new domain.SpNet();

        var result = spNet.RoundForward(@in, key, round);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0)]
    public void RoundInverse_ReturnsOriginalString(string @in, string key, int round)
    {
        var spNet = new domain.SpNet();

        var result = spNet.RoundForward(@in, key, round);
        result = spNet.RoundInverse(result, key, round);

        Assert.Equal(@in, result);
    }

    [Fact]
    public void Encrypt_WithLfsrSet_ReturnsExpectedEncryptedString()
    {
        const string @in = "КОРЫСТЬ_СЛОНА_ЭХ";
        const string key = "МТВ_ВСЕ_ЕЩЕ_ТЛЕН";
        const string expected = "ДЕДЭЫЯЩРЬАБЕЖЛЩЕ";

        var lfsr = new AsLfsrWithCBlock(key);
        var keysLc = lfsr.ProduceRoundKeys(8);
        var spNet = new domain.SpNet();

        var result = spNet.Encrypt(@in, keysLc, 8);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Decrypt_WithLfsrSet_ReturnsOriginalString()
    {
        const string @in = "КОРЫСТЬ_СЛОНА_ЭХ";
        const string key = "МТВ_ВСЕ_ЕЩЕ_ТЛЕН";
        const string expected = "ДЕДЭЫЯЩРЬАБЕЖЛЩЕ";

        var lfsr = new AsLfsrWithCBlock(key);
        var keysLc = lfsr.ProduceRoundKeys(8);
        var spNet = new domain.SpNet();

        var encrypted = spNet.Encrypt(@in, keysLc, 8);
        var result = spNet.Decrypt(encrypted, keysLc, 8);

        Assert.Equal(@in, result);
    }
}
