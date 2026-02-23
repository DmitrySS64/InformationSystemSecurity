using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.tests;

public class CBlockTests
{
    [Theory]
    [InlineData("ХОРОШО_БЫТЬ_ВАМИ", "КЬЕРКЕГОР_ПРОПАЛ", "ХЭТУЭУЙХВЬЕЬЫЭЫЫ")]
    [InlineData("ХОРОШО_БЫТЬ_ВАМИ", "ХОРОШО_ПРОБРОСИЛ", "ХПТСЭУЕЦВЬЕЬЫЯЫЫ")]
    [InlineData("ХОРОШО_БЫТЬ_ВАМИ", "ХОРОШО_БЫТЬ_ВАМИ", "ХПТСЭУЕИВЬЕКОНЫЧ")]
    public void Confuse_ReturnsExpectedText(string inputText1, string inputText2, string expected)
    {
        var result = CBlockCipher.Confuse(inputText1, inputText2);

        // Assert
        Assert.Equal(result, expected);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetCBlockTestData), MemberType = typeof(TestUtils))]
    public void CBlock_ReturnsExpectedCipherText(string[] array, CompressMode outSize, string expected)
    {
        var cipherCBlock = new CBlockCipher(new Caesar(CaesarMode.Core));
        
        var result = cipherCBlock.Encrypt(array, outSize);

        Assert.Equal(result, expected);
    }

    [Theory]
    [MemberData(nameof(TestUtils.GetCBlockSensitivityTestData), MemberType = typeof(TestUtils))]
    public void CBlock_CloseInputs(string[] baseArray, string[] variantArray,
        CompressMode outSize, int minDifference)
    {
        var cipherCBlock = new CBlockCipher(new Caesar(CaesarMode.Core));
        var baseline = cipherCBlock.Encrypt(baseArray, outSize);
        var changed = cipherCBlock.Encrypt(variantArray, outSize);

        var diff = TestUtils.CountDifferences(baseline, changed);
        Assert.True(diff >= minDifference,
            $"Expected >={minDifference} differences (mode={outSize}), got {diff}. Base='{string.Join(',', baseArray)}', Variant='{string.Join(',', variantArray)}'");
    }
}