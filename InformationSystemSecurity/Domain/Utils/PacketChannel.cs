using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Utils;

public static class PacketChannel
{
    // см. transmit
    public static byte[] GetStreamForTransmitting(Packet packet)
    {
        // todo
    }

    // см. recieve
    public static Packet ReceiveFromStream(byte[] stream)
    {
        var textStream = stream.ToTextMessage();
        
        // todo получать части, как ниже: 
        var type = textStream.Substring(Packet.TypeOffset, Packet.TypeLength);
        // ...
    }
}