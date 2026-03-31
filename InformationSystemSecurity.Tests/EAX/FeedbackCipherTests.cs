using InformationSystemSecurity.domain;
using InformationSystemSecurity.Domain.Lsfr;

namespace InformationSystemSecurity.Tests.EAX;

public class FeedbackCipherTests
{
    private readonly string[] messages = GetMessages();

    private static string[] GetMessages()
    {
        var massages = new List<string>();
        foreach (string line in File.ReadLines("inp.txt"))
        {
            massages.Add(line);
        }
        return massages.ToArray();
    }

    [Fact]
    public void EncryptWithMac_ReturnCorrectString()
    {
        var feedbackCipher = new FeedbackCipher();
        var iv = "АЛИСА_УМЕЕТ_ПЕТЬ";
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var lfsr = new AsLfsrWithCBlock(key);

        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);

        var resultString = feedbackCipher.Encrypt(messages[0], iv, keySet, domain.Enums.MacResultMode.WithMac);

        var expectedMessage = "ШНШФФЕНУЮЬЫМООИСТЕОО";
        var expectedLen = 384;
        var expectedSubStrPos = 368;
        var expectedSubStrLen = 16;
        var expectedSubStr = "ИОЬАБСЮЗЕЭЧРОМИР";

        Assert.Equal(expectedMessage, resultString.Substring(0, expectedMessage.Length));
        Assert.Equal(expectedLen, resultString.Length);
        Assert.Equal(expectedSubStr, resultString.Substring(expectedSubStrPos, expectedSubStrLen));
    }

    [Fact]
    public void EncryptWithoutMac_ReturnCorrectString()
    {
        var feedbackCipher = new FeedbackCipher();
        var iv = "АЛИСА_УМЕЕТ_ПЕТЬ";
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var lfsr = new AsLfsrWithCBlock(key);

        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);

        var resultString = feedbackCipher.Encrypt(messages[0], iv, keySet, domain.Enums.MacResultMode.NoMac);

        var expectedMessage = "ШНШФФЕНУЮЬЫМООИСТЕОО";
        var expectedLen = 368;

        Assert.Equal(expectedMessage, resultString.Substring(0, expectedMessage.Length));
        Assert.Equal(expectedLen, resultString.Length);
    }

    [Fact]
    public void EncryptOnlyMac_ReturnCorrectString()
    {
        var feedbackCipher = new FeedbackCipher();
        var iv = "АЛИСА_УМЕЕТ_ПЕТЬ";
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var lfsr = new AsLfsrWithCBlock(key);

        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);

        var resultString = feedbackCipher.Encrypt(messages[0], iv, keySet, domain.Enums.MacResultMode.OnlyMac);

        var expectedMac = "ИОЬАБСЮЗЕЭЧРОМИР";

        Assert.Equal(expectedMac, resultString);
    }

    [Fact]
    public void DecryptWithMac_ReturnCorrectString()
    {
        var feedbackCipher = new FeedbackCipher();
        var iv = "АЛИСА_УМЕЕТ_ПЕТЬ";
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var lfsr = new AsLfsrWithCBlock(key);

        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);

        var encryptedString = feedbackCipher.Encrypt(messages[0], iv, keySet, domain.Enums.MacResultMode.WithMac);

        var decryptedString = feedbackCipher.Decrypt(encryptedString, iv, keySet, domain.Enums.MacResultMode.WithMac);

        var expectedLen = 384;
        var expectedSubStrPos = 368;
        var expectedSubStrLen = 16;
        var expectedSubStr = new string('_', 16);

        Assert.Equal(messages[0], decryptedString.Substring(0, messages[0].Length));
        Assert.Equal(expectedLen, decryptedString.Length);
        Assert.Equal(expectedSubStr, decryptedString.Substring(expectedSubStrPos, expectedSubStrLen));
    }

}
