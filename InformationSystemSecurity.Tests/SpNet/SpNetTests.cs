using InformationSystemSecurity.domain;
using InformationSystemSecurity.Domain.Lsfr;

namespace InformationSystemSecurity.Tests.SpNet;

public class SpNetTests
{
    [Theory]
    [InlineData("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "СБЖКЕНЙХЧЩЫЭЯТЬР")]
    public void RoundForward(string @in, string key, int round, string expected)
    {
        var _spNet = new domain.SpNet();

        var result = _spNet.RoundForward(@in, key, round);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0)]
    public void RoundInverse(string @in, string key, int round)
    {
        var _spNet = new domain.SpNet();

        var result = _spNet.RoundForward(@in, key, round);
        result = _spNet.RoundInverse(result, key, round);

        Assert.Equal(@in, result);
    }

    [Fact]
    public void ProduceRoundKeysLfsrSet()
    {
        var key = "ПОЛИМАТ_ТЕХНОБОГ";
        var expected = new string[] {
            "ФУБЧЖЙЗХЛ_ОЭУРВО",
            "ЯЯРСКПЬВУСЫВЧГГД",
            "ДКЙЛОС_С_ЕОЗФСИБ",
            "МУМФОТМОЯДЕЦЦУЧЛ",
            "БИХЖЩФЗЦРИУАЭАДЛ",
            "БЬЙЬЮЯФРЮЗЯ_ТГФЕ"
        };

        var lfsr = new AsLfsrWithCBlock(key);
        
        var result = lfsr.ProduceRoundKeys(6);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EncryptAndDecryptWithLfsrSet()
    {
        var @in = "КОРЫСТЬ_СЛОНА_ЭХ";
        var key = "МТВ_ВСЕ_ЕЩЕ_ТЛЕН";
        var expected = "ДЕДЭЫЯЩРЬАБЕЖЛЩЕ";

        var lfsr = new AsLfsrWithCBlock(key);

        var keysLC = lfsr.ProduceRoundKeys(8);

        var spNet = new domain.SpNet();

        var result = spNet.Encrypt(@in, keysLC, 8);

        Assert.Equal(expected, result);

        result = spNet.Decrypt(result, keysLC, 8);

        Assert.Equal(@in, result);
    }
}
