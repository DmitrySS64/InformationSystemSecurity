using InformationSystemSecurity.domain;
using InformationSystemSecurity.Domain.Lsfr;

namespace InformationSystemSecurity.Tests.Lfsr;

public class AsLfsrTests
{
    [Fact]
    public void Push_ReturnsCorrectStatesAndStreams()
    {
        var sSet = new[]
        {
            "ЛЕРА".ToNum(),
            "КЛОН".ToNum(),
            "КОНЯ".ToNum()
        };
        
        var tapsSet = new[]
        {
            0b_1001_0000_0000_0000_0000UL,
            0b_0111_0000_0000_0000_1000UL,
            0b_0010_0000_0100_0000_0000UL
        };

        var expectedStates1 = new[]{
            0b_1100_0011_0100_0100_0010UL,
            0b_1011_0110_0011_1101_1101UL,
            0b_1011_0111_1011_1011_1111UL
        };
        var expectedStates2 = new[]{
            0b_1000_0110_1000_1000_0101UL,
            0b_0110_1100_0111_1011_1011UL,
            0b_0110_1111_0111_0111_1111UL
        };
        var expectedStates3 = new[]{
            0b_0000_1101_0001_0000_1011UL,
            0b_1101_1000_1111_0111_0111UL,
            0b_1101_1110_1110_1111_1110UL
        };
        const ulong expectedStream1 = 1;
        const ulong expectedStream2 = 1;
        const ulong expectedStream3 = 0;

        var out1 = AsLfsr.Push(sSet, tapsSet);
        var out2 = AsLfsr.Push(out1.States, tapsSet);
        var out3 = AsLfsr.Push(out2.States, tapsSet);

        Assert.Equal(expectedStates1, out1.States);
        Assert.Equal(expectedStream1, out1.Stream);

        Assert.Equal(expectedStates2, out2.States);
        Assert.Equal(expectedStream2, out2.Stream);
        
        Assert.Equal(expectedStates3, out3.States);
        Assert.Equal(expectedStream3, out3.Stream);
    }

    [Fact]
    public void GetNext_ReturnsCorrectStatesAndStreams()
    {
        var seedSet = new[]
        {
            "ЛЕРА".ToNum(),
            "КЛОН".ToNum(),
            "КОНЯ".ToNum()
        };
        var tapsSet = new[]
        {
            0b_1001_0000_0000_0000_0000UL,
            0b_0111_0000_0000_0000_1000UL,
            0b_0010_0000_0100_0000_0000UL
        };
        
        var expectedStates = new[]
        {
            0b_0110_1100_1011_0010_1010UL,
            0b_1110_1100_0111_0110_0111UL,
            0b_1101_0100_1000_1101_0110UL
        };

        const ulong expectedStream = 0b_1100_0100_1100_0100_0111UL;

        var result = AsLfsr.GetNext(seedSet, tapsSet);

        Assert.Equal(expectedStates, result.States);
        Assert.Equal(expectedStream.ToBinaryString(), result.Stream.ToBinaryString());
    }
}

