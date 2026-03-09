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
    
    [Theory]
    [MemberData(nameof(TestUtils.GetCloseInputsTestData), MemberType = typeof(TestUtils))]
    public void CaesarSimpleCloseInputs_ResultsDiffer(string plainText, string modifiedPlainText, string key)
    {
        var caesar = new Caesar(mode: CaesarMode.Simple);
        
        // Act
        var result1 = caesar.Encrypt(plainText, key);
        var result2 = caesar.Encrypt(modifiedPlainText, key);

        // Assert
        var textDiff = TestUtils.CountDifferences(plainText, modifiedPlainText);
        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > textDiff, $"Expected more than {textDiff} differing character, but got {diffCount}. {result1}:{result2}");
    }
    
    [Theory]
    [MemberData(nameof(TestUtils.GetRotationTestData), MemberType = typeof(TestUtils))]
    public void CaesarSimpleRotation_NotPermutation(string plainText, string rotatedPlainText, string key)
    {
        var caesar = new Caesar(mode: CaesarMode.Simple);
        
        // Act
        var result1 = caesar.Encrypt(plainText, key);
        var result2 = caesar.Encrypt(rotatedPlainText, key);

        // Assert: результат не является перестановкой (символы не те же)
        var sorted1 = string.Concat(result1.OrderBy(c => c));
        var sorted2 = string.Concat(result2.OrderBy(c => c));
        Assert.NotEqual(sorted1, sorted2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetAdditiveHomomorphismData), MemberType = typeof(TestUtils))]
    public void CaesarSimpleAdditiveHomomorphism_NotHomomorphic(string textA, string textB, string key)
    {
        var caesar = new Caesar(mode: CaesarMode.Simple);

        var sumCipher = Converter.AddTexts(caesar.Encrypt(textA, key), caesar.Encrypt(textB, key));
        var sumPlain = Converter.AddTexts(textA, textB);
        var cipherSumPlain = caesar.Encrypt(sumPlain, key);

        Assert.NotEqual(sumCipher, cipherSumPlain);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyChangeTestData), MemberType = typeof(TestUtils))]
    public void CaesarSimpleKeyChange_ResultsDiffer(string plainText, string key1, string key2)
    {
        var caesar = new Caesar(mode: CaesarMode.Simple);

        var result1 = caesar.Encrypt(plainText, key1);
        var result2 = caesar.Encrypt(plainText, key2);
        
        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > 1, $"Expected more than 1 differing character, but got {diffCount}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyRotationTestData), MemberType = typeof(TestUtils))]
    public void CaesarSimpleKeyRotation_ResultsDiffer(string plainText, string key, string rotatedKey)
    {
        var caesar = new Caesar(mode: CaesarMode.Simple);

        var result1 = caesar.Encrypt(plainText, key);
        var result2 = caesar.Encrypt(plainText, rotatedKey);

        Assert.NotEqual(result1, result2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyAdditionTestData), MemberType = typeof(TestUtils))]
    public void CaesarSimpleKeyAddition_ResultsDiffer(string plainText, string key1, string key2)
    {
        var caesar = new Caesar(mode: CaesarMode.Simple);

        var sumCipher = Converter.AddTexts(caesar.Encrypt(plainText, key1), caesar.Encrypt(plainText, key2));
        var keySum = Converter.AddTexts(key1, key2);
        var cipherKeySum = caesar.Encrypt(plainText, keySum);

        Assert.NotEqual(sumCipher, cipherKeySum);
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
    
    [Theory]
    [MemberData(nameof(TestUtils.GetCloseInputsTestData), MemberType = typeof(TestUtils))]
    public void CaesarPolyCloseInputs_ResultsDiffer(string plainText, string modifiedPlainText, string key)
    {
        var caesar = new Caesar(mode: CaesarMode.Poly);
        
        // Act
        var result1 = caesar.Encrypt(plainText, key);
        var result2 = caesar.Encrypt(modifiedPlainText, key);

        // Assert
        var textDiff = TestUtils.CountDifferences(plainText, modifiedPlainText);
        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > textDiff, $"Expected more than {textDiff} differing character, but got {diffCount}. {result1}:{result2}");
    }
    
    [Theory]
    [MemberData(nameof(TestUtils.GetRotationTestData), MemberType = typeof(TestUtils))]
    public void CaesarPolyRotation_NotPermutation(string plainText, string rotatedPlainText, string key)
    {
        var caesar = new Caesar(mode: CaesarMode.Poly);
        
        // Act
        var result1 = caesar.Encrypt(plainText, key);
        var result2 = caesar.Encrypt(rotatedPlainText, key);

        // Assert: результат не является перестановкой (символы не те же)
        var sorted1 = string.Concat(result1.OrderBy(c => c));
        var sorted2 = string.Concat(result2.OrderBy(c => c));
        Assert.NotEqual(sorted1, sorted2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetAdditiveHomomorphismData), MemberType = typeof(TestUtils))]
    public void CaesarPolyAdditiveHomomorphism_NotHomomorphic(string textA, string textB, string key)
    {
        var caesar = new Caesar(mode: CaesarMode.Poly);

        var sumCipher = Converter.AddTexts(caesar.Encrypt(textA, key), caesar.Encrypt(textB, key));
        var sumPlain = Converter.AddTexts(textA, textB);
        var cipherSumPlain = caesar.Encrypt(sumPlain, key);

        Assert.NotEqual(sumCipher, cipherSumPlain);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyChangeTestData), MemberType = typeof(TestUtils))]
    public void CaesarPolyKeyChange_ResultsDiffer(string plainText, string key1, string key2)
    {
        var caesar = new Caesar(mode: CaesarMode.Poly);

        var result1 = caesar.Encrypt(plainText, key1);
        var result2 = caesar.Encrypt(plainText, key2);
        
        var diff = TestUtils.CountDifferences(result1, result2);

        Assert.True(diff > 1, $"Expected more than 1 differing character, but got {diff}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyRotationTestData), MemberType = typeof(TestUtils))]
    public void CaesarPolyKeyRotation_ResultsDiffer(string plainText, string key, string rotatedKey)
    {
        var caesar = new Caesar(mode: CaesarMode.Poly);

        var result1 = caesar.Encrypt(plainText, key);
        var result2 = caesar.Encrypt(plainText, rotatedKey);

        Assert.NotEqual(result1, result2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyAdditionTestData), MemberType = typeof(TestUtils))]
    public void CaesarPolyKeyAddition_ResultsDiffer(string plainText, string key1, string key2)
    {
        var caesar = new Caesar(mode: CaesarMode.Poly);

        var sumCipher = Converter.AddTexts(caesar.Encrypt(plainText, key1), caesar.Encrypt(plainText, key2));
        var keySum = Converter.AddTexts(key1, key2);
        var cipherKeySum = caesar.Encrypt(plainText, keySum);

        Assert.NotEqual(sumCipher, cipherKeySum);
    }
    #endregion

    #region Caesar core

    [Theory]
    [InlineData("ХОРОШО_БЫТЬ_ВАМИ", "КЬЕРКЕГОР_ПРОПАЛ", "ЗЗБХЛТЧЯЯПОЦЦЖЙР")]
    [InlineData("КЬЕРКЕГОР_ПРОПАЛ", "ХОРОШО_БЫТЬ_ВАМИ", "ЕЭЭЦХИЧЖБДСАУХОВ")]
    public void CoreEncrypt_ReturnsExpectedCipherText(string primeText, string auxText, string expectedCipherText)
    {
        // Arrange
        var caesar = new Caesar(mode: CaesarMode.Core);

        // Act
        var result = caesar.Encrypt(primeText, auxText);

        // Assert
        Assert.Equal(expectedCipherText, result);
    }
    
    #endregion

    #region SBlock Caesar-Cipher Tests

    [Fact]
    public void SBlockSimpleEncrypt_ReturnsExpectedCipherText()
    {
        // Arrange
        const string plainText = "БЛОК";
        const string key = "ХОРОШО_БЫТЬ_ВАМИ";

        var caesar = new Caesar(mode: CaesarMode.Poly);
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

        var caesar = new Caesar(mode: CaesarMode.Poly);
        var sBlockCipher = new SBlockCipher(caesar, key);

        const string expected = "БЛОК";

        // Act
        var result = sBlockCipher.Decrypt(cipherText);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SBlockEncryptWithRoundKey_ReturnsExpectedCipherText()
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
    public void SBlockEncryptWithRoundKeyAndMerge_ReturnsExpectedCipherText()
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

    [Theory]
    [MemberData(nameof(TestUtils.GetCloseInputsTestData), MemberType = typeof(TestUtils))]
    public void SBlockCloseInputs_ResultsDiffer(string plainText, string modifiedPlainText, string key)
    {
        var sBlockCipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: true);
        // Act
        var result1 = sBlockCipher.Encrypt(plainText);
        var result2 = sBlockCipher.Encrypt(modifiedPlainText);

        // Assert
        var textDiff = TestUtils.CountDifferences(plainText, modifiedPlainText);
        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > textDiff, $"Expected more than {textDiff} differing character, but got {diffCount}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetRotationTestData), MemberType = typeof(TestUtils))]
    public void SBlockRotation_NotPermutation(string plainText, string rotatedPlainText, string key)
    {
        var sBlockCipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: true);
        
        // Act
        var result1 = sBlockCipher.Encrypt(plainText);
        var result2 = sBlockCipher.Encrypt(rotatedPlainText);

        // Assert: результат не является перестановкой (символы не те же)
        var sorted1 = string.Concat(result1.OrderBy(c => c));
        var sorted2 = string.Concat(result2.OrderBy(c => c));
        Assert.NotEqual(sorted1, sorted2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetAdditiveHomomorphismData), MemberType = typeof(TestUtils))]
    public void SBlockAdditiveHomomorphism_NotHomomorphic(string textA, string textB, string key)
    {
        var sBlockCipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: true);

        var sumCipher = Converter.AddTexts(sBlockCipher.Encrypt(textA), sBlockCipher.Encrypt(textB));
        var sumPlain = Converter.AddTexts(textA, textB);
        var cipherSumPlain = sBlockCipher.Encrypt(sumPlain);

        Assert.NotEqual(sumCipher, cipherSumPlain);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyChangeTestData), MemberType = typeof(TestUtils))]
    public void SBlockKeyChange_ResultsDiffer(string plainText, string key1, string key2)
    {
        var cipher1 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key1, roundKey: true, merge: true);
        var cipher2 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key2, roundKey: true, merge: true);

        var result1 = cipher1.Encrypt(plainText);
        var result2 = cipher2.Encrypt(plainText);

        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > 1, $"Expected more than 1 differing character, but got {diffCount}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyRotationTestData), MemberType = typeof(TestUtils))]
    public void SBlockKeyRotation_ResultsDiffer(string plainText, string key, string rotatedKey)
    {
        var cipher1 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: true);
        var cipher2 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), rotatedKey, roundKey: true, merge: true);

        var result1 = cipher1.Encrypt(plainText);
        var result2 = cipher2.Encrypt(plainText);

        Assert.NotEqual(result1, result2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyAdditionTestData), MemberType = typeof(TestUtils))]
    public void SBlockKeyAddition_ResultsDiffer(string plainText, string key1, string key2)
    {
        var cipher1 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key1, roundKey: true, merge: true);
        var cipher2 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key2, roundKey: true, merge: true);

        var sumCipher = Converter.AddTexts(cipher1.Encrypt(plainText), cipher2.Encrypt(plainText));
        var keySum = Converter.AddTexts(key1, key2);
        var cipherKeySum = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), keySum, roundKey: true, merge: true).Encrypt(plainText);

        Assert.NotEqual(sumCipher, cipherKeySum);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetCloseInputsTestData), MemberType = typeof(TestUtils))]
    public void SBlockCloseInputsNoMerge_ResultsDiffer(string plainText, string modifiedPlainText, string key)
    {
        var sBlockCipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: false);

        var result1 = sBlockCipher.Encrypt(plainText);
        var result2 = sBlockCipher.Encrypt(modifiedPlainText);

        var textDiff = TestUtils.CountDifferences(plainText, modifiedPlainText);
        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > textDiff, $"Expected more than {textDiff} differing character, but got {diffCount}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetRotationTestData), MemberType = typeof(TestUtils))]
    public void SBlockRotationNoMerge_NotPermutation(string plainText, string rotatedPlainText, string key)
    {
        var sBlockCipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: false);
        
        // Act
        var result1 = sBlockCipher.Encrypt(plainText);
        var result2 = sBlockCipher.Encrypt(rotatedPlainText);

        // Assert: результат не является перестановкой (символы не те же)
        var sorted1 = string.Concat(result1.OrderBy(c => c));
        var sorted2 = string.Concat(result2.OrderBy(c => c));
        Assert.NotEqual(sorted1, sorted2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetAdditiveHomomorphismData), MemberType = typeof(TestUtils))]
    public void SBlockAdditiveHomomorphismNoMerge_NotHomomorphic(string textA, string textB, string key)
    {
        var sBlockCipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: false);

        var sumCipher = Converter.AddTexts(sBlockCipher.Encrypt(textA), sBlockCipher.Encrypt(textB));
        var sumPlain = Converter.AddTexts(textA, textB);
        var cipherSumPlain = sBlockCipher.Encrypt(sumPlain);

        Assert.NotEqual(sumCipher, cipherSumPlain);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyChangeTestData), MemberType = typeof(TestUtils))]
    public void SBlockKeyChangeNoMerge_ResultsDiffer(string plainText, string key1, string key2)
    {
        var cipher1 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key1, roundKey: true, merge: false);
        var cipher2 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key2, roundKey: true, merge: false);

        var result1 = cipher1.Encrypt(plainText);
        var result2 = cipher2.Encrypt(plainText);

        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > 1, $"Expected more than 1 differing character, but got {diffCount}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyRotationTestData), MemberType = typeof(TestUtils))]
    public void SBlockKeyRotationNoMerge_ResultsDiffer(string plainText, string key, string rotatedKey)
    {
        var cipher1 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key, roundKey: true, merge: false);
        var cipher2 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), rotatedKey, roundKey: true, merge: false);

        var result1 = cipher1.Encrypt(plainText);
        var result2 = cipher2.Encrypt(plainText);

        Assert.NotEqual(result1, result2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyAdditionTestData), MemberType = typeof(TestUtils))]
    public void SBlockKeyAdditionNoMerge_ResultsDiffer(string plainText, string key1, string key2)
    {
        var cipher1 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key1, roundKey: true, merge: false);
        var cipher2 = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key2, roundKey: true, merge: false);

        var sumCipher = Converter.AddTexts(cipher1.Encrypt(plainText), cipher2.Encrypt(plainText));
        var keySum = Converter.AddTexts(key1, key2);
        var cipherKeySum = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), keySum, roundKey: true, merge: false).Encrypt(plainText);

        Assert.NotEqual(sumCipher, cipherKeySum);
    }

    #endregion

    [Fact]
    public void TestTest()
    {
        // make test with plainText: "БЛОК", key1: "ИЖФВЧЫШЕУДШПЛННЯ"
        const string plainText = "БЛОК";
        const string key1 = "ИЖФВЧЫШЕУДШПЛННЯ";
        var cipher = new SBlockCipher(new Caesar(mode: CaesarMode.Poly), key1, true, true);
        var result = cipher.Encrypt(plainText);
    }
}

