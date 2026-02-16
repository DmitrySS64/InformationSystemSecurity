using InformationSystemSecurity.domain.Enums;
using System.Security.Cryptography;

namespace InformationSystemSecurity.domain;

public class CBlockCipher(ICipher cipher)
{
    public string Encrypt(string[] textArray, CompressMode outSize)
    {
        if (textArray.Length is 0 or > 4)
            throw new ArgumentException("Input must be between 1 and 5 strings.");

        var C = new string[4];
        C[0] = "________________";
        C[1] = "ПРОЖЕКТОР_ЧЕПУХИ";
        C[2] = "КОЛЫХАТЬ_ПАРОДИЮ";
        C[3] = "КАРМАННЫЙ_АТАМАН";

        var compressed = string.Empty;
        
        for (var i = 0; i < textArray.Length; i++)
        {
            if (textArray[i].Length != 16)
                throw new ArgumentException("Each string must be 16 characters long.");

            C[i] = Alphabet.AddTexts(C[i], textArray[i]);
        }

        C[1] = Alphabet.AddTexts(C[1], textArray[0]);

        var part1 = cipher.Encrypt(C[0], C[2]);
        var part2 = cipher.Encrypt(C[3], C[1]);
        var confused = Confuse(part1, part2);

        var result = cipher.Encrypt(confused, part1);
        compressed = Compress(result, outSize);

        return compressed;
    }

    //TODO: вернуть private
    public static string Confuse(string text1, string text2)
    {
        var arr1 = Alphabet.ToNumArray(text1);
        var arr2 = Alphabet.ToNumArray(text2);

        for (var i = 0; i < 16; i++)
        {
            if (arr1[i] > arr2[i])
            {
                arr1[i] = (arr1[i] + i) % 32;
            }
            else 
            {
                arr1[i] = (arr2[i] + i) % 32;
            }
        }
        return Alphabet.ToText(arr1);
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
            CompressMode.Out8 => Alphabet.AddTexts(
                string.Concat(a1, a3), 
                string.Concat(a2, a4)
            ),
            CompressMode.Out4 => Alphabet.AddTexts(
                Alphabet.SubtractTexts(a1, a3), 
                Alphabet.SubtractTexts(a2, a4)
            ),
            _ => throw new ArgumentException("Invalid compression mode.")
        };
    }
}