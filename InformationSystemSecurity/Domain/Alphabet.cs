namespace InformationSystemSecurity.domain;

public static class Alphabet
{
    public const string AlphabetString = "_АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЫЬЭЮЯ";
    public static int AlphabetLength => AlphabetString.Length;

    public static int ToNum(this char c)
    {
        if (c == 95) 
            return 0;

        var baseNumber = c - 1039;
        return (c > 1066) // Исключаем "Ъ"
            ? baseNumber - 1
            : baseNumber;
    }

    public static char ToChar(this int num)
    {
        if (num == 0) 
            return '_';

        var charCode = num + 1039;
        if (num > 26) // Учитываем "Ъ"
            charCode++; 

        return (char)charCode;
    }
        
    public static int[] ToNumArray(this string text)
    {
        if (string.IsNullOrEmpty(text)) 
            return [];
        
        var result = text.Select(ToNum).ToArray();

        return result;
    }

    public static string ToText(this int[] nums)
    {
        if (nums.Length == 0) 
            return "";

        var chars = nums.Select(ToChar).ToArray();
        return new string(chars);
    }

    public static char AddChars(char c1, char c2)
    {
        var resultNum = (c1.ToNum() + c2.ToNum()) % AlphabetLength;
        return resultNum.ToChar();
    }

    public static char SubtractChars(char c1, char c2)
    {
        var resultNum = (c1.ToNum() - c2.ToNum() + AlphabetLength) % AlphabetLength;
        return resultNum.ToChar();
    }

    public static string AddTexts(string text1, string text2)
    {
        var commonLength = Math.Min(text1.Length, text2.Length);
        var maxLength = Math.Max(text1.Length, text2.Length);
        var longerText = text1.Length > text2.Length ? text1 : text2;
        
        var result = new char[maxLength];
        for (var i = 0; i < commonLength; i++)
            result[i] = AddChars(text1[i], text2[i]);

        if (maxLength == commonLength) 
            return new string(result);
    
        // Если тексты разные по длине, добавляем оставшиеся символы из более длинного текста
        for (var i = commonLength; i < maxLength; i++) 
            result[i] = longerText[i];
        
        return new string(result);
    }

    public static string SubtractTexts(string text1, string text2)
    {
        var commonLength = Math.Min(text1.Length, text2.Length);
        var maxLength = Math.Max(text1.Length, text2.Length);
        var longerText = text1.Length >= text2.Length ? text1 : text2;
        
        var result = new char[maxLength];
        for (var i = 0; i < commonLength; i++)
            result[i] = SubtractChars(text1[i], text2[i]);
        
        if (maxLength == commonLength) 
            return new string(result);
        
        //Если первый текст длинее второго, переписываем оставшиеся символы (a - 0)
        if (text1.Length > text2.Length)
        {
            for (var i = commonLength; i < maxLength; i++)
                result[i] = longerText[i];
        }
        //Иначе отнимаем от '_' символы второго текста (0 - c)
        else
        {
            for (var i = commonLength; i < maxLength; i++)
                result[i] = SubtractChars('_', longerText[i]);
        }

        return new string(result);
    }
}