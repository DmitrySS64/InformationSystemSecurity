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
    private readonly string _secret;
    private int _messageCount;
    private int _lastVector;

    private readonly FeedbackCipher _feedbackCipher = new();

    public EaxSession(AssociatedData assData, string key, string nonce)
    {
        // Инициализаций (до if type == "send" на стр. 70)
        _associatedData = assData;
        var t1 = String.Concat(_associatedData.Receiver, _associatedData.Sender); //16
        var t2 = String.Concat(_associatedData.SecurityLevel, _associatedData.SessionId, //2 + 9 + 5 = 16
            new string('_', 16 - Packet.TypeLength - Packet.SessionLength));
        var cad = TextConverter.AddTexts(t1, t2);
        _baseInitVector = TextConverter.AddTexts(cad, nonce)[..12];
        var t3 = (_associatedData.Receiver.ToBigInteger() < _associatedData.Sender.ToBigInteger()) ? 
            t1 : String.Concat(_associatedData.Sender, _associatedData.Receiver); // 16
        _messageCount = 0;
        _lastVector = -1;
        var lfsr = new AsLfsrWithCBlock(key);
        _keySet = lfsr.ProduceRoundKeys(FeedbackCipher.SBlockRoundCount);
        _secret = _feedbackCipher.Encrypt(t3+t2, key, _keySet, Enums.MacResultMode.OnlyMac);
        var data = string.Concat(
            _associatedData.SecurityLevel,
            _associatedData.Sender,
            _associatedData.Receiver,
            _associatedData.SessionId,
            new string('_', 5));
        _mac = _feedbackCipher.Encrypt(data, _secret, _keySet, Enums.MacResultMode.OnlyMac);
    }

    public byte[] SendMessage(string message)
    {
        var initVector = _baseInitVector + TextConverter.ToBlock((ulong)_messageCount);
        var tmpPacket = Packet.Prepare(_associatedData, initVector, message);

        _messageCount++;

        switch (_associatedData.SecurityLevel)
        {
            case MessageSecurityLevels.Open:
                return PacketChannel.GetStreamForTransmitting(tmpPacket);
            case MessageSecurityLevels.MacOnly:
                var secPacketM = this.Encrypt(tmpPacket, _mac, _keySet, _secret, true);
                return PacketChannel.GetStreamForTransmitting(secPacketM);
            case MessageSecurityLevels.Encrypted:
                var secPacket = this.Encrypt(tmpPacket, _mac, _keySet, _secret);
                return PacketChannel.GetStreamForTransmitting(secPacket);
            default:
                throw new ArgumentException("messageSecurity");
        }
    }

    // см. EAX_CFB в ветке if type = "send" (стр 70)
    public byte[][] SendMessages(string[] messages)
    {
        var outArray = new byte[messages.Length][];
        for (var i = 0; i < messages.Length; i++)
        {
            outArray[i] = SendMessage(messages[i]);
        }

        return outArray;
    }

    public Packet ReceiveMessage(byte[] stream)
    {
        var tmpPacket = PacketChannel.ReceiveFromStream(stream);

        var rdata = tmpPacket.Data;

        var currentVector = (int) tmpPacket.InitVector.Substring(12, 4).ToNum();

        if (currentVector <= _lastVector)
            throw new AccessViolationException("Incorrect Vector");

        if (rdata[0] == MessageSecurityLevels.Encrypted)
        {
            var recPacket = this.Decrypt(tmpPacket, _keySet, _secret);
            recPacket.Message = PaddingManager.UnpadMessage(recPacket.Message);
            if (recPacket.Mac == new string('_', 16))
            {
                _lastVector = currentVector;
                recPacket.Mac = "OK";
            }
            return recPacket;
        }
        else if (rdata[0] == MessageSecurityLevels.MacOnly && _associatedData.SecurityLevel != MessageSecurityLevels.Encrypted)
        {
            var recPacket = this.Decrypt(tmpPacket, _keySet, _secret, true);
            recPacket.Message = PaddingManager.UnpadMessage(recPacket.Message);
            if (recPacket.Mac == new string('_', 16))
            {
                _lastVector = currentVector;
                recPacket.Mac = "OK";
            }
            return recPacket;
        }
        else if (rdata[0] == MessageSecurityLevels.Open && _associatedData.SecurityLevel == MessageSecurityLevels.Open)
        {
            var recPacket = tmpPacket;
            recPacket.Message = PaddingManager.UnpadMessage(recPacket.Message);
            if (recPacket.Mac == "")
            {
                _lastVector = currentVector;
                recPacket.Mac = "N/A";
            }
            return recPacket;
        }

        return tmpPacket;
    }

    public Packet[] ReceiveMessages(byte[][] streams)
    {
        var outArray = new Packet[streams.Length];
        _lastVector = -1;

        for (var i = 0; i < streams.Length; i++)
        {
            outArray[i] = ReceiveMessage(streams[i]);
        }

        return outArray;
    }

    // см. EAX_CFB_frw (стр 60)
    public Packet Encrypt(Packet packet, string sessionMac, string[] keySet, string nonce, bool onlyMac = false)
    {
        var tmp = packet.Data[0] + packet.Data[3] + packet.Data[4]; //2 + 9 + 5
        var civ = _feedbackCipher.Encrypt(nonce + tmp, packet.InitVector, keySet, Enums.MacResultMode.OnlyMac); //16
        string mac;
        string msg;
        if (onlyMac)
        {
            tmp = _feedbackCipher.Encrypt(packet.Message, civ, keySet, Enums.MacResultMode.OnlyMac); //16
            var a = BinaryConverter.TextXor(tmp, civ);
            mac = BinaryConverter.TextXor(a, sessionMac);
            msg = packet.Message;
        }
        else
        {
            tmp = _feedbackCipher.Encrypt(packet.Message, civ, keySet, Enums.MacResultMode.WithMac);
            var m = tmp.Substring(packet.Message.Length, 16);
            var a = BinaryConverter.TextXor(m, civ); //16
            mac = BinaryConverter.TextXor(a, sessionMac);
            msg = tmp.Substring(0, packet.Message.Length);
        }

        return new Packet
        {
            Data = packet.Data,
            InitVector = packet.InitVector,
            Message = msg,
            Mac = mac
        };
    }
    
    // см. EAX_CFB_inv (стр 61)
    public Packet Decrypt(Packet packet, string[] keySet, string nonce, bool onlyMac = false)
    {
        var tmp = packet.Data[0] + packet.Data[3] + packet.Data[4]; //2 + 9 + 5 = 16
        var data = string.Join("", packet.Data[0..4]) + new string('_', 5); //2 + 8 + 8 + 9 + 5 = 32

        var cmac = _feedbackCipher.Encrypt(data, nonce, keySet, Enums.MacResultMode.OnlyMac); //16
        var civ = _feedbackCipher.Encrypt(nonce + tmp, packet.InitVector, keySet, Enums.MacResultMode.OnlyMac); //16
        string mac;
        string msg;
        if (onlyMac)
        {
            tmp = _feedbackCipher.Encrypt(packet.Message, civ, keySet, Enums.MacResultMode.OnlyMac); //16
            var a = BinaryConverter.TextXor(tmp, civ);
            var b = BinaryConverter.TextXor(a, cmac);
            mac = BinaryConverter.TextXor(packet.Mac, b);
            msg = packet.Message;
        }
        else
        {
            var a = BinaryConverter.TextXor(packet.Mac, civ);
            var cont = BinaryConverter.TextXor(a, cmac);
            tmp = _feedbackCipher.Decrypt(packet.Message + cont, civ, keySet, Enums.MacResultMode.WithMac);
            mac = tmp.Substring(packet.Message.Length, 16);
            msg = tmp.Substring(0, packet.Message.Length);
        }

        return new Packet {
            Data = packet.Data,
            InitVector = packet.InitVector,
            Message = msg,
            Mac = mac
        };
    }
}