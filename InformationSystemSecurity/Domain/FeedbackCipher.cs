using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.domain;

public class FeedbackCipher
{
    public const int SBlockRoundCount = 8;
    private readonly SpNet _spNet = new(CaesarMode.Core);
    
    // см. frw_cfb
    public string Encrypt(string message, string initVector, string[] keySet, MacResultMode macResultMode)
    {
        // todo
        // ciph_frw 
        _spNet.Encrypt(..., SBlockRoundCount);
        
        // todo ... (textXor уже есть в BinaryConverter, переиспользовать его)

        return macResultMode switch
        {
            MacResultMode.NoMac => message, // изменённый
            MacResultMode.WithMac => // concat
            MacResultMode.OnlyMac => mac,
            _ => throw new ArgumentOutOfRangeException(nameof(macResultMode), macResultMode, null)
        };
    }
    
    // см. inv_cfb
    public string Decrypt(string message, string initVector, string[] keySet, MacResultMode macResultMode)
    {
        // todo
    }
}