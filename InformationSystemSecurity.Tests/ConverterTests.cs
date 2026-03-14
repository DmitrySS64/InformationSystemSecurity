using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.tests;

public class ConverterTests
{
    [Fact]
    public void Text2Array_Then_Array2Text_ReturnsOriginalAlphabet()
    {
        // Arrange
        var alphabetString = Converter.AlphabetString;

        // Act
        var a = alphabetString.ToNumArray();
        var b = a.ToText();

        // Assert
        Assert.Equal(Converter.AlphabetString, b);
    }

    [Fact]
    public void AddChars_WhenAddingTwoChars_ReturnsExpectedChar()
    {
        // Arrange
        const char a = 'Я';
        const char b = 'Ж';
        const char expected = 'Е';

        // Act
        var result = Converter.AddChars(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubtractChars_WhenSubtractingTwoChars_ReturnsExpectedChar()
    {
        // Arrange
        const char expected = 'Я';
        const char b = 'Ж';
        const char c = 'Е';

        // Act
        var result = Converter.SubtractChars(c, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AddTexts_WhenAddingTwoTexts_ReturnsExpectedText()
    {
        // Arrange
        const string text1 = "ЕЖИК";
        const string text2 = "В_ТУМАНЕ";
        const string expected = "ИЖЬЯМАНЕ";

        // Act
        var result = Converter.AddTexts(text1, text2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubtractTexts_WhenSubtractingTwoTexts_ReturnsExpectedText()
    {
        // Arrange
        const string cipherText = "ИЖЬЯМАНЕ";
        const string text2 = "В_ТУМАНЕ";
        const string expected = "ЕЖИК____";

        // Act
        var result = Converter.SubtractTexts(cipherText, text2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubtractTexts_WhenSubtractingTwoTexts1_ReturnsExpectedText()
    {
        //От длинного отнимается короткий
        // Arrange
        const string cipherText = "ИЖЬЯМАНЕ";
        const string text2 = "ЕЖИК";
        const string expected = "В_ТУМАНЕ";

        // Act
        var result = Converter.SubtractTexts(cipherText, text2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubtractTexts_WhenSubtractingTwoTexts2_ReturnsExpectedText()
    {
        //от короткого отнимается длинный
        // Arrange
        const string cipherText = "ЕЖИК";
        const string text2 = "ИЖЬЯМАНЕ";
        const string expected = "Э_МЛТЯСЩ";

        // Act
        var result = Converter.SubtractTexts(cipherText, text2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToNum_ValidBlock_ReturnsCorrectNumber()
    {
        // Arrange
        const string block = "АБВГ";
        const ulong expected = 34916;

        // Act
        var result = block.ToNum();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToBlock_ValidNumber_ReturnsCorrectBlock()
    {
        // Arrange
        const ulong number = 34916;
        const string expected = "АБВГ";

        // Act
        var result = number.ToBlock();

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("____", 0)]
    [InlineData("___А", 1)]
    [InlineData("__Б_", 0b_100_0000UL)]
    [InlineData("__БГ", 0b_100_0100UL)]
    public void ToNum_ReturnsCorrectBinaryBlock(string input, ulong expected)
    {
        // Act
        var result = input.ToNum();

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void ToBinary_ReturnsCorrectBinary() 
    {
        int[] input = [20, 17];
        const ulong expected = 0b_1001_0000_0000_0000_0000UL;

        var result = input.ToBinary();

        Assert.Equal(expected, result);
    }
}