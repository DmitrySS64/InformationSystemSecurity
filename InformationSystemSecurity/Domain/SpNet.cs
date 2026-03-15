using System.Text;

namespace InformationSystemSecurity.domain;

public class SpNet
{
    private readonly SBlockCipher _sBlock = null!;
        
    public string Encrypt(string text, string[] keySet, int roundCount)
    {
        //TODO: см. frw_SPNet
    } 
    
    public string Decrypt(string text, string[] keySet, int roundCount)
    {
        //TODO: см. inv_SPNet
    } 
    
    internal string RoundForward(string text, string key, int roundNumber)
    {
        var sBlockResult = new string[4];
        // TODO
        ...
        var pBlockResult = PBlockCipher.Encrypt(string.Concat(sBlockResult), roundNumber);
        return BinaryConverter.TextXor(pBlockResult, key);
    }
    
    internal string RoundInverse(string text, string key, int roundNumber)
    {
        // TODO
    }
}