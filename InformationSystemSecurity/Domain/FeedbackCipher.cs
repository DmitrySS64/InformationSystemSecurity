using InformationSystemSecurity.domain.Enums;
using InformationSystemSecurity.Domain.Utils;

namespace InformationSystemSecurity.domain;

public class FeedbackCipher
{
    public const int SBlockRoundCount = 8;
    public const int blockLength = 16;
    private readonly SpNet _spNet = new(CaesarMode.Poly);
    
    // см. frw_cfb (стр 42)
    public string Encrypt(string message, string initVector, string[] keySet, MacResultMode macResultMode)
    {
        if (message.Length % blockLength != 0)
        {
            throw new ArgumentException("The string must be a multiple of 16.");
        }

        var m = message.Length / blockLength;
        var cont = new string('_', 16);
        var feedback = initVector;
        var outString = "";
        var keyStream = "";
        for (var i = 0; i < m; i++)
        {
            var inp = message.Substring(i * blockLength, blockLength);
            cont = BinaryConverter.TextXor(inp, cont);
            keyStream = _spNet.Encrypt(feedback, keySet, SBlockRoundCount);
            feedback = BinaryConverter.TextXor(inp, keyStream);
            outString += feedback;
        }
        keyStream = _spNet.Encrypt(feedback, keySet, SBlockRoundCount);
        var mac = BinaryConverter.TextXor(cont, keyStream);

        return macResultMode switch
        {
            MacResultMode.NoMac => outString, // изменённый
            MacResultMode.WithMac => string.Concat(outString, mac),
            MacResultMode.OnlyMac => mac,
            _ => throw new ArgumentOutOfRangeException(nameof(macResultMode), macResultMode, null)
        };
    }
    
    // см. inv_cfb (стр 43)
    public string Decrypt(string message, string initVector, string[] keySet, MacResultMode macResultMode)
    {
        var m = message.Length / blockLength;
        var cont = new string('_', blockLength);
        var feedback = initVector;
        var outString = "";
        var keyStream = "";
        var text = "";

        var macIn = (int)macResultMode == 2 ? -1 : (int)macResultMode;
        for (var i = 0; i < m - macIn; i++)
        {
            var inp = message.Substring(i * blockLength, blockLength);
            keyStream = _spNet.Encrypt(feedback, keySet, SBlockRoundCount);
            feedback = inp;
            text = BinaryConverter.TextXor(inp, keyStream);
            cont = BinaryConverter.TextXor(cont, text);
            outString += text;
        }
        if (macIn == 0)
            return outString;

        var mac = message.Substring((m - 1) * blockLength, blockLength);
        keyStream = _spNet.Encrypt(feedback, keySet, SBlockRoundCount);
        text = BinaryConverter.TextXor(mac, keyStream);
        cont = BinaryConverter.TextXor(cont, text);

        return macResultMode switch
        {
            MacResultMode.WithMac => string.Concat(outString, cont),
            MacResultMode.OnlyMac => cont,
            _ => throw new ArgumentOutOfRangeException(nameof(macResultMode), macResultMode, null)
        };
    }
}