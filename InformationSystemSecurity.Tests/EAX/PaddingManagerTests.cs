using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.Tests.EAX;

public class PaddingManagerTests
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
    public void PadMessage_ReturnOriginalString()
    {
        var msg = messages[0];
        var result = PaddingManager.PadMessage(msg);

        Assert.Equal(msg, result);
    }

    [Fact]
    public void PadMessage_ReturnCorrectStringLength()
    {
        var msg = messages[1];
        var expectedLen = 3088;
        var result = PaddingManager.PadMessage(msg);

        Assert.Equal(expectedLen, result.Length);
    }

    [Fact]
    public void UnpadMessage_ReturnCorrectStringLenght()
    {
        var str = messages[1];
        var paddingMessege = PaddingManager.PadMessage(str);

        var result = PaddingManager.UnpadMessage(paddingMessege);

        Assert.Equal(str.Length, result.Length);
    }
}
