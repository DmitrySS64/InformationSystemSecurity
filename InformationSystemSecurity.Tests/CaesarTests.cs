using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.tests;

public class CaesarTests
{
    #region Simple Caesar Tests
    
    [Fact]
    public void SimpleEncrypt_WhenKeyIsZero_ReturnsPlainText()
    {
        // Arrange
        const string plainText = "ОЛОЛО_КРИНЖ";
        const string key = "_";
        
        var caesar = new Caesar(mode: CaesarMode.Simple);

        // Act
        var result = caesar.Encrypt(plainText, key);

        // Assert
        Assert.Equal(plainText, result);
    }

    [Fact]
    public void SimpleEncrypt_ReturnsExpectedCipherText()
    {
        // Arrange
        const string plainText = "ОЛОЛО_КРИНЖ";
        const string key = "Х";
        const string expected = "ДБДБДХАЖЯГЭ";
        
        var caesar = new Caesar(mode: CaesarMode.Simple);

        // Act
        var result = caesar.Encrypt(plainText, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SimpleDecrypt_ReturnsPlainText()
    {
        // Arrange
        const string plainText = "ОЛОЛО_КРИНЖ";
        const string key = "Х";
        var caesar = new Caesar(mode: CaesarMode.Simple);
        
        var encrypted = caesar.Encrypt(plainText, key);

        // Act
        var decrypted = caesar.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public void SimpleDecrypt_WhenKeyIsZero_ReturnsCipherTextUnchanged()
    {
        // Arrange
        const string cipherText = "ОЛОЛО_КРИНЖ";
        const string key = "_";
        var caesar = new Caesar(mode: CaesarMode.Simple);

        // Act
        var result = caesar.Decrypt(cipherText, key);

        // Assert
        Assert.Equal(cipherText, result);
    }
    
    #endregion

    #region Poly Caesar Tests
    
    [Theory]
    [InlineData("ОЛОЛО_КРИНЖ", "Х", "ДЧРГЭГДАОЙШ")]
    [InlineData("ОЛОЛО_КРИНЖ", "ПАНТЕОН", "ЯЭНЮЖЖ_ХОБН")]
    public void PolyEncrypt_ReturnsExpectedCipherText(string plainText, string key, string expectedCipherText)
    {
        // Arrange
        var caesar = new Caesar(mode: CaesarMode.Poly);
        
        // Act
        var result = caesar.Encrypt(plainText, key);

        // Assert
        Assert.Equal(expectedCipherText, result);
    }

    [Theory]
    [InlineData("ДЧРГЭГДАОЙШ", "Х", "ОЛОЛО_КРИНЖ")]
    [InlineData("ЯЭНЮЖЖ_ХОБН", "ПАНТЕОН", "ОЛОЛО_КРИНЖ")]
    public void PolyDecrypt_ReturnsExpectedPlainText(string cipherText, string key, string expectedPlainText)
    {
        // Arrange
        var caesar = new Caesar(mode: CaesarMode.Poly);
        
        // Act
        var result = caesar.Decrypt(cipherText, key);

        // Assert
        Assert.Equal(expectedPlainText, result);
    }
    
    #endregion

    #region SBlock Caesar-Cipher Tests

    [Fact]
    public void SBlockSimpleEncrypt_ReturnsExpectedCipherText()
    {
        // Arrange
        const string plainText = "БЛОК";
        const string key = "ХОРОШО_БЫТЬ_ВАМИ";
        
        var caesar = new Caesar();
        var sBlockCipher = new SBlockCipher(caesar, key);
        
        const string expected = "ЧРДП";
        
        // Act
        var result = sBlockCipher.Encrypt(plainText);
        
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SBlockSimpleDecrypt_ReturnsExpectedPlainText()
    {
        // Arrange
        const string cipherText = "ЧРДП";
        const string key = "ХОРОШО_БЫТЬ_ВАМИ";

        var caesar = new Caesar();
        var sBlockCipher = new SBlockCipher(caesar, key);

        const string expected = "БЛОК";

        // Act
        var result = sBlockCipher.Decrypt(cipherText);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void SBlockEncryptWithShuffle_ReturnsExpectedCipherText()
    {
        // Arrange
        const string plainText = "БЛОК";
        const string key = "ХОРОШО_БЫТЬ_ВАМИ";
        
        var caesar = new Caesar(mode: CaesarMode.Poly);
        var sBlockCipher = new SBlockCipher(caesar, key, roundKey: true);
        
        const string expected = "АЗЩЯ";
        
        // Act
        var result = sBlockCipher.Encrypt(plainText);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void SBlockEncryptWithShuffleAndMerge_ReturnsExpectedCipherText()
    {
        // Arrange
        const string plainText = "БЛОК";
        const string key = "ХОРОШО_ВЫТЬ_ВАМИ";
        
        var caesar = new Caesar(mode: CaesarMode.Poly);
        var sBlockCipher = new SBlockCipher(caesar, key, roundKey: true, merge: true);
        
        const string expected = "УЫ_Ш";
        
        // Act
        var result = sBlockCipher.Encrypt(plainText);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    #endregion
}