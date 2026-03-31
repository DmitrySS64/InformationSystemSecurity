using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Utils;

public static class PacketChannel
{
    // см. transmit (39 стр)
    public static byte[] GetStreamForTransmitting(Packet packet)
    {
        var outString = string.Join("", packet.Data);
        outString = string.Concat(
            outString, 
            packet.InitVector, 
            packet.Message, 
            packet.Mac);

        var outBins = outString.ToBinary();

        return outBins;
    }

    // см. recieve (39 стр)
    public static Packet ReceiveFromStream(byte[] stream)
    {
        var textStream = stream.ToTextMessage();

        var type =      textStream.Substring(Packet.TypeOffset, Packet.TypeLength);
        var sender =    textStream.Substring(Packet.SenderOffset,Packet.SenderLength);
        var receiver =  textStream.Substring(Packet.ReceiverOffset, Packet.ReceiverLength);
        var session =   textStream.Substring(Packet.SessionOffset, Packet.SessionLength);
        var length =    textStream.Substring(Packet.LengthOffset, Packet.LengthLength);
        var iv =        textStream.Substring(Packet.IvOffset, Packet.IvLength);

        var L = 0;

        for (var i = 0; i < 5; i++)
        {
            L = TextConverter.AlphabetLength * L + length[i].ToNum();
        }

        L /= 5;

        var message = textStream.Substring(Packet.MessageOffset, L);
        var mac = textStream.Substring(Packet.MessageOffset + L, textStream.Length - (Packet.MessageOffset + L));

        return new Packet
        {
            Data = [type, sender, receiver, session, length],
            InitVector = iv,
            Message = message,
            Mac = mac
        };
    }
}