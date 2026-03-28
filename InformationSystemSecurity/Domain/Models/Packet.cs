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
    
    // см. prepare_packet
    public static Packet Prepare(AssociatedData associatedData, string initVector, string message)
    {
        // todo ...
        return new Packet
        {
            Data = ...,
            InitVector = ...,
            Message = ...,
            Mac = ...
        };
    }
}