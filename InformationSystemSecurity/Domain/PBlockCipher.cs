using InformationSystemSecurity.Domain.Utils;

namespace InformationSystemSecurity.domain;

public class PBlockCipher 
{
    public static string Encrypt(string text, int roundNumber)
    {
        if (text.Length != 16)
            throw new ArgumentException("Input text must be 16 characters long.");

        var square = MagicSquare.GetDefaultSet()[roundNumber % 3];
        var j = 4 * (roundNumber % 4) + 2;

        var tmp = MagicSquare.Encrypt(text, square).ToBigInteger();

        tmp.Shift(j, 16 * 5);
        return tmp.ToText();
    }
    
    public static string Decrypt(string text, int roundNumber)
    {
        if (text.Length != 16)
            throw new ArgumentException("Input text must be 16 characters long.");

        var square = MagicSquare.GetDefaultSet()[roundNumber % 3];
        var j = -(4 * (roundNumber % 4) + 2);

        var tmp = text.ToBigInteger();
        tmp.Shift(j, 16 * 5);
        var result = MagicSquare.Decrypt(tmp.ToText(), square);

        return result;
    }
}