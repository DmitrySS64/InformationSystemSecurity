using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.domain;

public class CBlockCipher(ICipher cipher)
{
    public string Encrypt(string[] textArray, CompressMode outSize)
    {
        if (textArray.Length is 0 or > 4)
            throw new ArgumentException("Input must be between 1 and 5 strings.");
        
        // todo ...

        var compressed = string.Empty;
        
        foreach (var text in textArray)
        {
            if (text.Length != 16)
                throw new ArgumentException("Each string must be 16 characters long.");
            
            // todo ...

            var part1 = cipher.Encrypt(textArray[0], textArray[2]);
            var part2 = cipher.Encrypt(textArray[3], textArray[1]);
            var confused = Confuse(part1, part2);
            
            var result = cipher.Encrypt(confused, part1);
            compressed = Compress(result, outSize);
        }

        return compressed;
    }

    private static string Confuse(string text1, string text2)
    {
        throw new NotImplementedException();
    }
    
    private static string Compress(string text, CompressMode mode)
    {
        if (mode == CompressMode.Out16)
            return text;

        if (string.IsNullOrEmpty(text) || text.Length < 16)
            return "input_error";

        var a1 = text.Substring(0, 4);
        var a2 = text.Substring(4, 4);
        var a3 = text.Substring(8, 4);
        var a4 = text.Substring(12, 4);

        return mode switch
        {
            CompressMode.Out8 => // todo ...
            CompressMode.Out4 => // todo ...
            _ => throw new ArgumentException("Invalid compression mode.")
        };
    }
}