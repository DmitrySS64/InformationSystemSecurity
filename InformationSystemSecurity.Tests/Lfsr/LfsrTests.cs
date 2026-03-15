using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.Tests.Lfsr;

public class LfsrTests
{
    [Fact]
    public void Push_ReturnsCorrectState() 
    {
        var seed = "КУБА".ToNum();
        const ulong t1 = 0b_1001_0000_0000_0000_0000UL;
        const ulong expectedState = 0b_0001_0000_0110_1101_0100UL;
        const string expectedResult = "ЦЗГВ";

        var state = Domain.Lsfr.Lfsr.Push(seed, t1);
        var result = state.ToBlock();
        for (var i = 1; i < 10; i++)
            state = Domain.Lsfr.Lfsr.Push(state, t1);

        Assert.Equal(expectedState, state);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNext_ReturnsCorrectStateAndStream()
    {
        var seed = "ОРИМ".ToNum();
        const ulong t1 = 0b_1001_0000_0000_0000_0000UL;
        const ulong t2 = 0b_0111_0000_0000_0000_1000UL;

        var tmp1 = Domain.Lsfr.Lfsr.GetNext(seed, t1);
        var tmp2 = Domain.Lsfr.Lfsr.GetNext(seed, t2);

        const string expected1 = "ТЫБА";
        const string expected2 = "ЖВЫП";

        var resultState1 = tmp1.State.ToBlock();
        var resultStream1 = tmp1.Stream.ToBlock();
        var resultState2 = tmp2.State.ToBlock();
        var resultStream2 = tmp2.Stream.ToBlock();

        Assert.Equal(expected1, resultState1);
        Assert.Equal(tmp1.State.ToBinaryString(), tmp1.Stream.ToBinaryString());
        Assert.Equal(expected2, resultState2);
        Assert.Equal(expected1, resultStream1);
        Assert.Equal(expected2, resultStream2);
    }
}

