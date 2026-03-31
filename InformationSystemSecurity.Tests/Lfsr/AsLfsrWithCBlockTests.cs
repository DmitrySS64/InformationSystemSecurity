using InformationSystemSecurity.Domain.Lsfr;
using InformationSystemSecurity.Domain.Utils;

namespace InformationSystemSecurity.Tests.Lfsr;

public class AsLfsrWithCBlockTests
{
    [Fact]
    public void GetNext_ReturnsCorrectStatesAndStreams()
    {
        var first = new[] { 19, 18 }.ToBinary();
        var second = new[] { 18, 7 }.ToBinary();

        var set = new ulong[][]
        {
            [first, second, new[] { 17, 3 }.ToBinary()],
            [first, second, new[] { 16, 14, 13, 11 }.ToBinary()],
            [first, second, new[] { 15, 13, 12, 10 }.ToBinary()],
            [first, second, new[] { 14, 5, 3, 1 }.ToBinary()]
        };
        
        const string seed = "АБВГДЕЖЗИЙКЛМНОП";

        var lfsr = new AsLfsrWithCBlock(seed, set);
        
        const string expectedStream = "СХЫЫЛЮЕФЦИСМГУЮФ";
        const ulong expectedState00 = 0b_1110_1101_1011_1100_1100uL;

        var result = lfsr.GetNext();

        Assert.Equal(expectedState00.ToBinaryString(), result.State[0][0].ToBinaryString());
        Assert.Equal(expectedStream, result.Stream);
    }
    
    [Fact]
    public void ProduceRoundKeys_ReturnsExpectedKeys()
    {
        const string key = "ПОЛИМАТ_ТЕХНОБОГ";
        var expected = new[] 
        {
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
}