using InformationSystemSecurity.domain;
using InformationSystemSecurity.Domain.Lsfr;
using System.Text.RegularExpressions;

namespace InformationSystemSecurity.tests;

public class LfsrTests
{
    [Theory]
    [InlineData("____", 0)]
    [InlineData("___А", 1)]
    [InlineData("__Б_", 0b_100_0000UL)]
    [InlineData("__БГ", 0b_100_0100UL)]
    public void BlockToBin(string input, ulong expected)
    {
        // Act
        var result = input.ToNum();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TapsToBin() {
        int[] input = [19, 16];
        ulong expected = 0b_1001_0000_0000_0000_0000UL;

        var result = input.ToBinary();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void LfsrPush() {
        var seed = "КУБА".ToNum();
        var t1 = 0b_1001_0000_0000_0000_0000UL;
        var expected1 = 0b_0001_0000_0110_1101_0100UL;
        var expected2 = "ЦЗГВ";

        var s = Lfsr.Push(seed, t1);
        var result2 = s.ToBlock();
        for (int i = 1; i < 10; i++)
        {
            s = Lfsr.Push(s, t1);
        }

        Assert.Equal(expected1, s);
        Assert.Equal(expected2, result2);
    }

    [Fact]
    public void LfsrNext()
    {
        var seed = "ОРИМ".ToNum();
        var t1 = 0b_1001_0000_0000_0000_0000UL;
        var t2 = 0b_0111_0000_0000_0000_1000UL;

        var tmp1 = Lfsr.GetNext(seed, t1);
        var tmp2 = Lfsr.GetNext(seed, t2);

        var expected1 = "ТЫБА";
        var expected2 = "ЖВЫП";

        var resultState1 = tmp1.State.ToBlock();
        var resultStream1 = tmp1.Stream.ToBlock();
        var resultState2 = tmp2.State.ToBlock();
        var resultStream2 = tmp2.Stream.ToBlock();

        Assert.Equal(expected1, resultState1);
        Assert.Equal(tmp1.State, tmp1.Stream);
        Assert.Equal(expected2, resultState2);
        Assert.Equal(expected1, resultStream1);
        Assert.Equal(expected2, resultStream2);
    }

    [Fact]
    public void AsLfsrPush()
    {
        var SSet = new ulong[]{
            "ЛЕРА".ToNum(),
            "КЛОН".ToNum(),
            "КОНЯ".ToNum()
        };
        var TSet = new ulong[]
        {
            0b_1001_0000_0000_0000_0000UL,
            0b_0111_0000_0000_0000_1000UL,
            0b_0010_0000_0100_0000_0000UL
        };

        var expectedStates1 = new ulong[]{
            0b_1100_0011_0100_0100_0010UL,
            0b_1011_0110_0011_1101_1101UL,
            0b_1011_0111_1011_1011_1111UL
        };
        var expectedStates2 = new ulong[]{
            0b_1000_0110_1000_1000_0101UL,
            0b_0110_1100_0111_1011_1011UL,
            0b_0110_1111_0111_0111_1111UL
        };
        var expectedStates3 = new ulong[]{
            0b_0000_1101_0001_0000_1011UL,
            0b_1101_1000_1111_0111_0111UL,
            0b_1101_1110_1110_1111_1110UL
        };
        ulong expectedStream3 = 0;

        var out1 = AsLfsr.Push(SSet, TSet);
        var out2 = AsLfsr.Push(out1.States, TSet);
        var result = AsLfsr.Push(out2.States, TSet);

        Assert.Equal(expectedStates1, out1.States);
        Assert.Equal(expectedStates2, out2.States);
        Assert.Equal(expectedStates3, result.States);
        Assert.Equal(expectedStream3, result.Stream);
    }

    [Fact]
    public void AsLfsrNext()
    {
        var SSet = new ulong[]{
            "ЛЕРА".ToNum(),
            "КЛОН".ToNum(),
            "КОНЯ".ToNum()
        };
        var TSet = new ulong[]
        {
            0b_1001_0000_0000_0000_0000UL,
            0b_0111_0000_0000_0000_1000UL,
            0b_0010_0000_0100_0000_0000UL
        };


        var expectedStates = new ulong[]{
            0b_0110_1100_1011_0010_1010UL,
            0b_1110_1100_0111_0110_0111UL,
            0b_1101_0100_1000_1101_0110UL
        };

        var expectedStream = 0b_1100_0100_1100_0100_0111UL;

        var result = AsLfsr.GetNext(SSet, TSet);

        Assert.Equal(expectedStates, result.States);
        Assert.Equal(expectedStream, result.Stream);
    }

    [Fact]
    public void AsLfsrWithCBlockNext()
    {
        var set10 = new int[] { 19, 18 };
        var set11 = new int[] { 18, 7 };
        var set12 = new int[] { 17, 3 };

        var set20 = new int[] { 19, 18 };
        var set21 = new int[] { 18, 7 };
        var set22 = new int[] { 16, 14, 13, 11 };

        var set30 = new int[] { 19, 18 };
        var set31 = new int[] { 18, 7 };
        var set32 = new int[] { 15, 13, 12, 10 };

        var set40 = new int[] { 19, 18 };
        var set41 = new int[] { 18, 7 };
        var set42 = new int[] { 14, 5, 3, 1 };
        var set = new ulong[4][]
        {
            [
                set10.ToBinary(),
                set11.ToBinary(),
                set12.ToBinary()
            ],
            [
                set20.ToBinary(),
                set21.ToBinary(),
                set22.ToBinary()
            ],
            [
                set30.ToBinary(),
                set31.ToBinary(),
                set32.ToBinary()
            ],
            [
                set40.ToBinary(),
                set41.ToBinary(),
                set42.ToBinary()
            ]
        };
        var seed = "АБВГДЕЖЗИЙКЛМНОП";

        var lfsr = new AsLfsrWithCBlock(seed, set);



        var expectedStream = "СХЫЫЛЮЕФЦИСМГУЮФ";

        var result = lfsr.GetNext();

        Assert.Equal(expectedStream, result.Stream);
    }
}

