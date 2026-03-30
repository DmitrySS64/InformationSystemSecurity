using InformationSystemSecurity.Domain.Utils;

namespace InformationSystemSecurity.Tests;

public class BinaryConverterTests
{
    [Theory]
    [InlineData("АГАТ", "ТАГА", "СДДС")]
    [InlineData("КОЛЕНЬКА", "МТВ_ТЛЕН", "ЕЬОЕЭПМО")]
    [InlineData("ТОРТ_ХОЧЕТ_ГОРКУ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", "ЮЬСТВГИЧ_ИЕГЬЭМЩ")]
    public void TextXor_ReturnsCorrectResult(string inA, string inB, string expected)
    {
        var result = BinaryConverter.TextXor(inA, inB);

        Assert.Equal(expected, result);

        //Обратная операция
        var result2 = BinaryConverter.TextXor(result, inB);

        Assert.Equal(inA, result2);
    }

    [Theory]
    [InlineData("ГОЛД", 1, "СЖХБ")]
    [InlineData("ЯРУС", 1, "ОЧЩИ")]
    public void Shift_WithUlong_ReturnsCorrectResult(string @in, int shift, string expected)
    {
        var result = @in.ToNum();
        result.Shift(shift, @in.Length * 5);
        Assert.Equal(expected.ToNum().ToBinaryString(), result.ToBinaryString());

        //Обратная операция
        result.Shift(-shift, @in.Length * 5);
        Assert.Equal(@in.ToNum().ToBinaryString(), result.ToBinaryString());
    }

    [Theory]
    [InlineData("ГОЛД", 1, "СЖХБ")]
    [InlineData("ЯРУС", 1, "ОЧЩИ")]
    public void Shift_WithBigInteger_ReturnsCorrectResult(string @in, int shift, string expected)
    {
        var num = @in.ToBigInteger();
        var bitLength = @in.Length * 5;

        num.Shift(shift, bitLength);
        Assert.Equal(expected, num.ToText(@in.Length));
        
        //Обратная операция
        num.Shift(-shift, bitLength);
        Assert.Equal(@in, num.ToText(@in.Length));
    }

    [Theory]
    [InlineData("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ", 145)]
    [InlineData("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ0011", 149)]
    [InlineData("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ1110011011011", 158)]
    public void MessageToBinary_ReturnsCorrectResult(string @in, int expectedLen)
    {
        var result = @in.ToBinary();

        Assert.Equal(expectedLen, result.Length);
    }

    [Theory]
    [InlineData("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ")]
    [InlineData("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ0011", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ0011")]
    [InlineData("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ1110011011011", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМЬЫ011")]
    public void BinaryToMessage_ReturnsCorrectResult(string @in, string expected)
    {
        var bins = @in.ToBinary();

        var result = bins.ToTextMessage();

        Assert.Equal(expected, result);
    }
}

