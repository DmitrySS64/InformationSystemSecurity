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
}