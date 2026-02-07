using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.tests;

public class CaesarTests
{
    [Fact]
    public void CaesarEncrypt_WhenKeyIsZero_ReturnsPlainText()
    {
        // Arrange
        const string plainText = "ОЛОЛО_КРИНЖ";
        const string key = "_";

        // Act
        var result = Caesar.Encrypt(plainText, key);

        // Assert
        Assert.Equal(plainText, result);
    }

    [Fact]
    public void CaesarEncrypt_WhenKeyIsNonZero_ReturnsExpectedCipherText()
    {
        // Arrange
        const string plainText = "ОЛОЛО_КРИНЖ";
        const string key = "Х";
        const string expected = "ДБДБДХАЖЯГЭ";

        // Act
        var result = Caesar.Encrypt(plainText, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CaesarDecrypt_WhenEncryptedWithSameKey_ReturnsPlainText()
    {
        // Arrange
        const string plainText = "ОЛОЛО_КРИНЖ";
        const string key = "Х";
        var encrypted = Caesar.Encrypt(plainText, key);

        // Act
        var decrypted = Caesar.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public void CaesarDecrypt_WhenKeyIsZero_ReturnsCipherTextUnchanged()
    {
        // Arrange
        const string cipherText = "ОЛОЛО_КРИНЖ";
        const string key = "_";

        // Act
        var result = Caesar.Decrypt(cipherText, key);

        // Assert
        Assert.Equal(cipherText, result);
    }

    [Theory]
    [InlineData("ОЛОЛО_КРИНЖ", "Х", "ДЧРГЭГДАОЙШ")]
    [InlineData("ОЛОЛО_КРИНЖ", "ПАНТЕОН", "ЯЭНЮЖЖ_ХОБН")]
    public void PolyEncrypt_WhenEncryptingWithKey_ReturnsExpectedCipherText(string plainText, string key, string expectedCipherText)
    {
        // Arrange

        // Act
        var result = Caesar.PolyEncrypt(plainText, key);

        // Assert
        Assert.Equal(expectedCipherText, result);
    }

    [Theory]
    [InlineData("ДЧРГЭГДАОЙШ", "Х", "ОЛОЛО_КРИНЖ")]
    [InlineData("ЯЭНЮЖЖ_ХОБН", "ПАНТЕОН", "ОЛОЛО_КРИНЖ")]
    public void PolyDecrypt_WhenDecryptingWithKey_ReturnsExpectedPlainText(string cipherText, string key, string expectedPlainText)
    {
        // Arrange

        // Act
        var result = Caesar.PolyDecrypt(cipherText, key);

        // Assert
        Assert.Equal(expectedPlainText, result);
    }
}