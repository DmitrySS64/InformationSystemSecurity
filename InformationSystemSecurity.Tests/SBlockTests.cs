using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.tests;

public class SBlockTests
{
    [Fact]
    public void Merge_ReturnsExpectedPlainText()
    {
        // Arrange
        const string plainText = "БЛОК";
        const string key = "ХОРОШО_ВЫТЬ_ВАМИ";

        const string expected = "ЬЗЦЩ";

        // Act
        var result = SBlockCipher.MergeBlock(plainText, key);
        var reverseResult = SBlockCipher.MergeBlock(result, key, reverse: true);
        
        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(plainText, reverseResult);
    }
}