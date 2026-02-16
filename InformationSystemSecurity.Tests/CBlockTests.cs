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
        var chiper_Caesar = new Caesar(CaesarMode.Core);
        var chiper_CBlock = new CBlockCipher(chiper_Caesar);
        
        var result = chiper_CBlock.Encrypt(array, outSize);

        Assert.Equal(result, expected);
    }
}

