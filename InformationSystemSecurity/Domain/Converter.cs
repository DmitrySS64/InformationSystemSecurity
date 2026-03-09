using System.Text;

namespace InformationSystemSecurity.domain;

public static class Converter
{
    public const string AlphabetString = "_АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЫЬЭЮЯ";
    public static int AlphabetLength => AlphabetString.Length;
    public const int BlockSize = 4;

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
    
    public static ulong ToNum(this string block)
    {
        if (block.Length != BlockSize)
            throw new ArgumentException($"Block must be {BlockSize} characters long.");

        ulong pos = 1;
        ulong output = 0;

        var tmp = block.ToNumArray();
        for (var i = BlockSize - 1; i >= 0; i--)
        {
            output += pos * (ulong)tmp[i];
            pos *= 32;
        }
        return output;
    }
    
    public static string ToBlock(this ulong num)
    {
        // TODO
        // функцию div отдельно не нужно создавать - используем C#
        var result = new StringBuilder(BlockSize);

        for (int i = 0; i < BlockSize; i++)
        {
            var digit = (int)(num % 32);
            result.Insert(0, digit.ToChar());
            num /= 32;
        }

        return result.ToString();
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
    
    // (см. push_reg) Вроде должно работать на уровне бинарных операций, но надо потестить
    // Если работает, то block2bin и bin2block вроде не нужны
    public static ulong PushBit(this ulong num, byte bit)
    {
        num = (num << 1) | bit;
        num &= 0xFFFFFUL;
        return num;
    }

    // (см. taps2bin)
    public static ulong ToBinary(this int[] tapPositions)
    {
        // todo: тут просто поставить единицы в заданных позициях и получить бинарное число
        // 100% можно сделать в пару строк, а не как в маткаде - можно у ИИ спросить, если что

        ulong result = 0;
        foreach (var position in tapPositions) { 
            if (position >= 0 && position < 64) 
                result |= 1UL << position;
        }

        return result;
    }


    /// <summary>
    /// Преобразует 20-битное число в массив из 20 бит (ulong)
    /// </summary>
    public static ulong[] ToBits(this ulong value)
    {
        var bits = new ulong[20];
        for (int i = 0; i < 20; i++)
        {
            bits[i] = (value >> i) & 1;
        }
        return bits;
    }

    /// <summary>
    /// Преобразует массив из 20 бит в 4-символьный блок
    /// </summary>
    public static string ToBlock(this ulong[] bits)
    {
        if (bits.Length != 20)
            throw new ArgumentException("Array must have 20 bits");

        // Собираем 20 бит в одно число
        ulong value = 0;
        for (int i = 0; i < 20; i++)
        {
            if (bits[i] == 1)
                value |= 1UL << i;
        }

        // Конвертируем число в блок (предполагаем, что есть метод ToBlock)
        return value.ToBlock(); // Используем существующий extension метод
    }
}