using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;
using InformationSystemSecurity.Domain.Lsfr;
using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Tests.EAX;

public class EaxSessionTests
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

    private byte[] Invert(byte[] stream, int pos)
    {
        stream[pos] = (byte)((stream[pos] + 1) % 2);
        return stream;
    }

    [Fact]
    public void ReceiveMessages_ReturnsCorrectPackets()
    {
        var associatedData = new AssociatedData("ВБ", "БОБ___ЬЬ", "АЛИСА_ЯЗ", "ЭКЛАМПСИЯ");
        var sessionKey = "СЕАНСОВЫЙ_КЛЮЧИК";
        var nonse = "СЕМИХАТОВ_КВАНТЫ";
        var eaxSession = new EaxSession(associatedData, sessionKey, nonse);

        var channel = eaxSession.SendMessages(messages);

        channel[0] = Invert(channel[0], 317);
        channel[3] = Invert(channel[3], 12);

        var transmission = eaxSession.ReceiveMessages(channel);

        var expectedMac1 = "__АШП";
        var expectedMac2 = "__ОБП";
        var expectedMac3 = "__БСП";
        var expectedMac4 = "___ЖП";

        //data
        Assert.Equal(associatedData.SecurityLevel, transmission[0].Data[0]);
        Assert.Equal(associatedData.Sender, transmission[0].Data[1]);
        Assert.Equal(associatedData.Receiver, transmission[0].Data[2]);
        Assert.Equal(associatedData.SessionId, transmission[0].Data[3]);
        Assert.NotEqual(associatedData.Sender, transmission[3].Data[1]);
        Assert.Equal(expectedMac1, transmission[0].Data[4]);
        Assert.Equal(expectedMac2, transmission[1].Data[4]);
        Assert.Equal(expectedMac3, transmission[2].Data[4]);
        Assert.Equal(expectedMac4, transmission[3].Data[4]);
        //InitVector
        Assert.Equal("ХУТЕВБЯЖЦЧЛВ____", transmission[0].InitVector);
        Assert.Equal("ХУТЕВБЯЖЦЧЛВ___А", transmission[1].InitVector);
        Assert.Equal("ХУТЕВБЯЖЦЧЛВ___Б", transmission[2].InitVector);
        Assert.Equal("ХУТЕВБЯЖЦЧЛВ___В", transmission[3].InitVector);
        //message
        Assert.Equal(messages[1], transmission[1].Message);
        //mac
        Assert.Equal("БПИКЮЮАПЮБЯЮРУИА", transmission[0].Mac);
        Assert.Equal("OK", transmission[1].Mac);
        Assert.Equal("OK", transmission[2].Mac);
        Assert.Equal("НЭЛПИУЛМОЧОЙДЙЫА", transmission[3].Mac);
    }

    [Fact]
    public void DecryptWithoutMac_ReturnCorrectPacket()
    {
        var associatedData = new AssociatedData("ВБ", "АЛИСА_АЖ", "БОБ___ОЧ", "ЕГИПТЯНИН");
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var secIn = "ТОЖЕ_ЕЩЕ_НЕВАЖНО"; //<- в презентации ЕЩЁ
        var packet = Packet.Prepare(associatedData, "БОБ_НЕМНОГО_ПЬЯН", messages[0]);
        packet.Data[4] = "_____";
        var cad = string.Join("", packet.Data); //32

        //var expectedMac = "ФЙСВСЩНТЙЭЬМЧБЖЛ";
        //var expectedMessage = "ЦЯЬШЭДВ_ЯЖВЙРЫЩФКДТДУ";

        var eaxSession = new EaxSession(associatedData, key, "");

        var feedbackCipher = new FeedbackCipher();
        var lfsr = new AsLfsrWithCBlock(key);

        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);
        var cadmac = feedbackCipher.Encrypt(cad, secIn, keySet, MacResultMode.OnlyMac);

        var sentPacket = eaxSession.Encrypt(packet, cadmac, keySet, secIn);
        var result = eaxSession.Decrypt(sentPacket, keySet, secIn);

        //Проверка невалидна (ошибка в презентации)
        //Assert.Equal(expectedMac, sentPacket.Mac);
        //Assert.Equal(expectedMessage, sentPacket.Message.Substring(0, expectedMessage.Length));

        Assert.Equal(messages[0], result.Message);
        Assert.Equal(new string('_', 16), result.Mac);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EncryptDecrypt_RoundTrip_ForPresentationScenario(bool onlyMac)
    {
        var associatedData = new AssociatedData("ВБ", "АЛИСА_АЖ", "БОБ___ОЧ", "ЕГИПТЯНИН");
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var secIn = "ТОЖЕ_ЕЩЕ_НЕВАЖНО";
        var packet = Packet.Prepare(associatedData, "БОБ_НЕМНОГО_ПЬЯН", messages[0]);
        packet.Data[4] = "_____";

        var eaxSession = new EaxSession(associatedData, key, "");
        var feedbackCipher = new FeedbackCipher();
        var lfsr = new AsLfsrWithCBlock(key);
        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);

        var cad = string.Join("", packet.Data);
        var cadmac = feedbackCipher.Encrypt(cad, secIn, keySet, MacResultMode.NoMac);

        var sentPacket = eaxSession.Encrypt(packet, cadmac, keySet, secIn, onlyMac);
        var result = eaxSession.Decrypt(sentPacket, keySet, secIn, onlyMac);

        Assert.Equal(packet.InitVector, sentPacket.InitVector);
        Assert.Equal(packet.Data, sentPacket.Data);
        Assert.Equal(16, sentPacket.Mac.Length);

        if (onlyMac)
            Assert.Equal(packet.Message, sentPacket.Message);
        else
            Assert.NotEqual(packet.Message, sentPacket.Message);

        Assert.Equal(packet.Message, result.Message);
        Assert.Equal(messages[0], PaddingManager.UnpadMessage(result.Message));
    }

    [Fact]
    public void Last20BitsOfMac_DoNotAffectRecoveredMessage()
    {
        var associatedData = new AssociatedData("ВБ", "АЛИСА_АЖ", "БОБ___ОЧ", "ЕГИПТЯНИН");
        var key = "СЕАНСОВЫЙ_КЛЮЧИК";
        var secIn = "ТОЖЕ_ЕЩЕ_НЕВАЖНО";

        var packet = Packet.Prepare(associatedData, "БОБ_НЕМНОГО_ПЬЯН", messages[0]);
        packet.Data[4] = "_____";

        var eaxSession = new EaxSession(associatedData, key, "");
        var feedbackCipher = new FeedbackCipher();
        var lfsr = new AsLfsrWithCBlock(key);
        var keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);

        var cad = string.Join("", packet.Data);
        var cadmac = feedbackCipher.Encrypt(cad, secIn, keySet, MacResultMode.NoMac);

        var sentPacket = eaxSession.Encrypt(packet, cadmac, keySet, secIn);
        var tamperedPacket = new Packet
        {
            Data = sentPacket.Data,
            InitVector = sentPacket.InitVector,
            Message = sentPacket.Message,
            // 4 символа = 20 бит в текущем алфавите (по 5 бит на символ).
            Mac = sentPacket.Mac[..12] + "АБВГ"
        };

        var validResult = eaxSession.Decrypt(sentPacket, keySet, secIn);
        var tamperedResult = eaxSession.Decrypt(tamperedPacket, keySet, secIn);

        Assert.Equal(validResult.Message, tamperedResult.Message);
        Assert.Equal(messages[0], PaddingManager.UnpadMessage(tamperedResult.Message));
        Assert.NotEqual(validResult.Mac, tamperedResult.Mac);
    }
}

