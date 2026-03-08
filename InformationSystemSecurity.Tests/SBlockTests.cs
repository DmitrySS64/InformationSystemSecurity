using InformationSystemSecurity.domain;
using System.Collections.Generic;

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

    [Theory]
    [MemberData(nameof(TestUtils.GetCloseInputsTestData), MemberType = typeof(TestUtils))]
    public void CloseInputs_ResultsDiffer(string plainText, string modifiedPlainText, string key)
    {
        var result1 = SBlockCipher.MergeBlock(plainText, key);
        var result2 = SBlockCipher.MergeBlock(modifiedPlainText, key);

        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > 1, $"Expected more than 1 differing character, but got {diffCount}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetRotationTestData), MemberType = typeof(TestUtils))]
    public void Rotation_NotPermutation(string plainText, string rotatedPlainText, string key)
    {
        var result1 = SBlockCipher.MergeBlock(plainText, key);
        var result2 = SBlockCipher.MergeBlock(rotatedPlainText, key);

        var sorted1 = string.Concat(result1.OrderBy(c => c));
        var sorted2 = string.Concat(result2.OrderBy(c => c));
        Assert.NotEqual(sorted1, sorted2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetAdditiveHomomorphismData), MemberType = typeof(TestUtils))]
    public void AdditiveHomomorphism_NotHomomorphic(string textA, string textB, string key)
    {
        var sumCipher = Converter.AddTexts(SBlockCipher.MergeBlock(textA, key), SBlockCipher.MergeBlock(textB, key));
        var sumPlain = Converter.AddTexts(textA, textB);
        var cipherSumPlain = SBlockCipher.MergeBlock(sumPlain, key);

        Assert.NotEqual(sumCipher, cipherSumPlain);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyChangeTestData), MemberType = typeof(TestUtils))]
    public void KeyChange_ResultsDiffer(string plainText, string key1, string key2)
    {
        var result1 = SBlockCipher.MergeBlock(plainText, key1);
        var result2 = SBlockCipher.MergeBlock(plainText, key2);

        var diffCount = TestUtils.CountDifferences(result1, result2);
        Assert.True(diffCount > 1, $"Expected more than 1 differing character, but got {diffCount}. {result1}:{result2}");
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyRotationTestData), MemberType = typeof(TestUtils))]
    public void KeyRotation_ResultsDiffer(string plainText, string key, string rotatedKey)
    {
        var result1 = SBlockCipher.MergeBlock(plainText, key);
        var result2 = SBlockCipher.MergeBlock(plainText, rotatedKey);

        Assert.NotEqual(result1, result2);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetKeyAdditionTestData), MemberType = typeof(TestUtils))]
    public void KeyAddition_ResultsDiffer(string plainText, string key1, string key2)
    {
        var sumCipher = Converter.AddTexts(SBlockCipher.MergeBlock(plainText, key1), SBlockCipher.MergeBlock(plainText, key2));
        var keySum = Converter.AddTexts(key1, key2);
        var cipherKeySum = SBlockCipher.MergeBlock(plainText, keySum);

        Assert.NotEqual(sumCipher, cipherKeySum);
    }
}