using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.domain;

public class CBlockCipher(ICipher cipher)
{
    public string Encrypt(string[] textArray, CompressMode outSize)
    {
        if (textArray.Length is 0 or > 4)
            throw new ArgumentException("Input must be between 1 and 5 strings.");

        string[] C =
        [
            "________________", 
            "ПРОЖЕКТОР_ЧЕПУХИ", 
            "КОЛЫХАТЬ_ПАРОДИЮ",
            "КАРМАННЫЙ_АТАМАН"
        ];
        
        for (var i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].Length != 16)
                throw new ArgumentException("Each string must be 16 characters long.");

            C[i] = Converter.AddTexts(C[i], textArray[i]);
        }

        C[1] = Converter.AddTexts(C[1], textArray[0]);

        var part1 = cipher.Encrypt(C[0], C[2]);
        var part2 = cipher.Encrypt(C[3], C[1]);
        var confused = Confuse(part1, part2);

        var result = cipher.Encrypt(confused, part1);
        var compressed = Compress(result, outSize);

        return compressed;
    }

    //TODO: вернуть private
    public static string Confuse(string text1, string text2)
    {
        var arr1 = text1.ToNumArray();
        var arr2 = text2.ToNumArray();

        for (var i = 0; i < 16; i++)
        {
            arr1[i] = arr1[i] > arr2[i]
                ? (arr1[i] + i) % 32
                : (arr2[i] + i) % 32;
        }
        return arr1.ToText();
    }

    //TODO: вернуть private
    public static string Compress(string text, CompressMode mode)
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
            CompressMode.Out8 => Converter.AddTexts(
                string.Concat(a1, a3), 
                string.Concat(a2, a4)
            ),
            CompressMode.Out4 => Converter.AddTexts(
                Converter.SubtractTexts(a1, a3), 
                Converter.SubtractTexts(a2, a4)
            ),
            _ => throw new ArgumentException("Invalid compression mode.")
        };
    }
}