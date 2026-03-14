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

        C = MixInputs(C);

        var part1 = cipher.Encrypt(C[0], C[2]);
        var part2 = cipher.Encrypt(C[3], C[1]);
        var confused = Confuse(part1, part2);

        var result = cipher.Encrypt(confused, part1);
        var compressed = Compress(result, outSize);

        return compressed;
    }
    
    internal static string Confuse(string text1, string text2)
    {
        var arr1 = text1.ToNumArray();
        var arr2 = text2.ToNumArray();

        for (var i = 0; i < 16; i++)
        {
            arr1[i] = arr1[i] > arr2[i]
                ? (arr1[i] + i) % 32
                : (arr2[i] + i) % 32;
        }

        var tmp = arr1.ToText();
        var t1 = Converter.AddTexts(tmp, text1);
        var t2 = Converter.AddTexts(t1, text2);
        return t2;
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

    private static string[] MixInputs(string[] textArray)
    {
        var in1 = textArray[0];
        var in2 = textArray[1];
        var in3 = textArray[2];
        var in4 = textArray[3];

        var out1 = Converter.AddTexts(in1, in2);
        var out2 = Converter.SubtractTexts(in1, in2);
        var out3 = Converter.AddTexts(out2, Converter.AddTexts(in3, in4));
        var out4 = Converter.AddTexts(out1, Converter.SubtractTexts(in3, in4));


        return [ out1, out2, out3, out4 ];
    }
}