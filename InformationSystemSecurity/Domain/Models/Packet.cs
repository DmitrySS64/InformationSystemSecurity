using InformationSystemSecurity.Domain.Utils;
using System.Text;

namespace InformationSystemSecurity.domain.Models;

public class Packet
{
    #region Смещения в символах от начала пакета
    
    public const int TypeOffset = 0;
    public const int TypeLength = 2;

    public const int SenderOffset = 2;
    public const int SenderLength = 8;

    public const int ReceiverOffset = 10;
    public const int ReceiverLength = 8;

    public const int SessionOffset = 18;
    public const int SessionLength = 9;

    public const int LengthOffset = 27;
    public const int LengthLength = 5;

    public const int IvOffset = 32;
    public const int IvLength = 16;

    public const int MessageOffset = 48;
    // MacOffset = MessageOffset + L // todo убрать комменты 
    // MacLength = TotalLength - MacOffset
    
    #endregion 
    
    public required string[] Data { get; set; }
    
    public required string InitVector { get; set; }
    
    public required string Message { get; set; }
    
    public required string Mac { get; set; }
    
    // см. prepare_packet (стр 38)
    public static Packet Prepare(AssociatedData associatedData, string initVector, string message)
    {
        initVector = TextConverter.AddTexts(initVector, new string('_', 16));
        var msg = PaddingManager.PadMessage(message);
        var L = msg.ToBinary().Length;
        var a = "";
        for (var i = 0; i < 5; i++)
        {
            a = (L % TextConverter.AlphabetLength).ToChar() + a;
            L /= TextConverter.AlphabetLength;
        }

        var data = new string[5]
        {
            associatedData.SecurityLevel,
            associatedData.Sender,
            associatedData.Receiver,
            associatedData.SessionId,
            a
        };

        return new Packet
        {
            Data = data,
            InitVector = initVector,
            Message = msg,
            Mac = ""
        };
    }
}