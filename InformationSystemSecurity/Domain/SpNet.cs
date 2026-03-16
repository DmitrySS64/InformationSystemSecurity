using System.Runtime.ExceptionServices;
using System.Text;

namespace InformationSystemSecurity.domain;

public class SpNet
{
    private SBlockCipher _sBlock = null!;

    public string Encrypt(string text, string[] keySet, int roundCount)
    {
        for (var i = 0; i < roundCount; i++)
            text = RoundForward(text, keySet[i], i);

        return text;
    }
    
    public string Decrypt(string text, string[] keySet, int roundCount)
    {
        for (var i = roundCount - 1; i >= 0; i--)
            text = RoundInverse(text, keySet[i], i);
        
        return text;
    } 
    
    public string RoundForward(string text, string key, int roundNumber)
    {
        var sBlockResult = new string[4];
        _sBlock = new SBlockCipher(new Caesar(Enums.CaesarMode.Poly), key, true, true);
        for (var i = 0; i < 4; i++)
        {
            var block = text.Substring(i * 4, 4);
            sBlockResult[i] = _sBlock.Encrypt(block);
        }
        var pBlockResult = PBlockCipher.Encrypt(string.Concat(sBlockResult), roundNumber);
        return BinaryConverter.TextXor(pBlockResult, key);
    }
    
    public string RoundInverse(string text, string key, int roundNumber)
    {
        var sBlockResult = new string[4];
        _sBlock = new SBlockCipher(new Caesar(Enums.CaesarMode.Poly), key, true, true);
        var xorResult = BinaryConverter.TextXor(text, key);
        var pBlockResult = PBlockCipher.Decrypt(xorResult, roundNumber);
        for (var i = 0; i < 4; i++)
        {
            var block = pBlockResult.Substring(i * 4, 4);
            sBlockResult[i] = _sBlock.Decrypt(block);
        }

        return string.Concat(sBlockResult);
    }
}