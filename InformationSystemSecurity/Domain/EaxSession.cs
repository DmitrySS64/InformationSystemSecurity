using InformationSystemSecurity.Domain.Lsfr;
using InformationSystemSecurity.domain.Models;
using InformationSystemSecurity.Domain.Utils;

namespace InformationSystemSecurity.domain;

public class EaxSession
{
    private readonly string[] _keySet;
    private readonly AssociatedData _associatedData;
    private readonly string _baseInitVector; //см. IV0
    private readonly string _mac; // см. data_mac
    private int _messageCount;
    
    private readonly FeedbackCipher _feedbackCipher = new();

    public EaxSession(AssociatedData assData, string key, string nonce)
    {
        // todo: Инициализаций (до if type == "send" на стр. 70)
        // ...
        _associatedData = assData;
        _baseInitVector = ...
        _messageCount = 0;
        // todo: проверить
        var lfsr = new AsLfsrWithCBlock(key);
        _keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);
        _mac = _feedbackCipher.Encrypt(...)
    }

    // см. EAX_CFB в ветке if type = "send"
    public byte[] SendMessages(string[] messages)
    {
        // TODO
        // PS. messageCount изначально = 0, поэтому инкрементируем после использоавния, а не до
        // для tmp_packet:
        Packet.Prepare();

        switch (_associatedData.SecurityLevel)
        {
            case MessageSecurityLevels.Open:
                //TODO
            case MessageSecurityLevels.MacOnly:
                //TODO
            case MessageSecurityLevels.Encrypted:
                //TODO
                // Для sec_packet
                Encrypt();
                // Для transmit
                PacketChannel.GetStreamForTransmitting();
            default:
                throw new ArgumentException("messageSecurity");
        }
    }

    public Packet[] ReceiveMessages(byte[][] streams)
    {
        //TODO
        // для recieve:
        PacketChannel.ReceiveFromStream();
        if (messageCountReceived < _messageCount)
            throw new AccessViolationException("Not relevant messageCount received");
        
        // Вместо last обноялем _messageCount, изначально -1 не нужно
        
        // Использовать MessageSecurityLevels в ифах как выше
        // EAX_CFB_inv:
        Decrypt();
        // unpad_message 
        PaddingManager.UnpadMessage();
    }
    
    // ! Функция invert используеся только в тестах,
    // причём не думаю, что её нужно выносить, там одна строка кода выйдет
    
    // см. EAX_CFB_frw
    internal Packet Encrypt(Packet packet, string sessionMac, string[] keySet, string nonce, bool onlyMac)
    {
        // todo
        // frw_CFB =
        _feedbackCipher.Encrypt();

    }
    
    // см. EAX_CFB_inv
    internal Packet Decrypt(Packet packet, string[] keySet, string nonce, bool onlyMac)
    {
        // todo
        // frw_CFB =
        _feedbackCipher.Encrypt();
        // inv_CFB =
        _feedbackCipher.Decrypt();

    }
    
}