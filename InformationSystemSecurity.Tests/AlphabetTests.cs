using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.tests;

public class AlphabetTests
{
    [Fact]
    public void Text2Array_Then_Array2Text_ReturnsOriginalAlphabet()
    {
        // Arrange
        string alphabetString = Alphabet.AlphabetString;

        // Act
        int[] a = Alphabet.Text2Array(alphabetString);
        string b = Alphabet.Array2Text(a);

        // Assert
        Assert.Equal(Alphabet.AlphabetString, b);
    }

    [Fact]
    public void AddChars_WhenAddingTwoChars_ReturnsExpectedChar()
    {
        // Arrange
        const char a = 'Я';
        const char b = 'Ж';
        const char expected = 'Е';

        // Act
        char result = Alphabet.AddChars(a, b);

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
        char result = Alphabet.SubtractChars(c, b);

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
        string result = Alphabet.AddTexts(text1, text2);

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
        string result = Alphabet.SubtractTexts(cipherText, text2);

        // Assert
        Assert.Equal(expected, result);
    }
}