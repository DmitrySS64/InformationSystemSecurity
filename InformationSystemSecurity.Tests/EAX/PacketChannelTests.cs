using InformationSystemSecurity.domain.Models;
using InformationSystemSecurity.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystemSecurity.Tests.EAX;

public class PacketChannelTests
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
    public void ReceiveFromStream_ReturnOriginPacket()
    {
        var assData = new AssociatedData("ВБ", "АЛИСА_АЖ", "БОБ___ОЧ", "ЕГИПТЯНИН");
        var packet = Packet.Prepare(assData, "КОЛЕСО", messages[1]);
        var ytst = PacketChannel.ReceiveFromStream(PacketChannel.GetStreamForTransmitting(packet));

        var expected = new string[] { assData.SecurityLevel, assData.Sender, assData.Receiver, assData.SessionId, "__ОБП" };

        Assert.Equal(expected, ytst.Data);

        Assert.Equal(packet.Message, ytst.Message);
        Assert.Equal(packet.InitVector, ytst.InitVector); 
        Assert.Equal("", packet.Mac);
        Assert.Equal(packet.Mac, ytst.Mac);
    }
}
