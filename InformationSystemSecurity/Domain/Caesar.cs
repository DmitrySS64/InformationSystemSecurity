using System.Text;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.domain;

public class Caesar(CaesarMode mode = CaesarMode.Core) : ICipher
{
    public string Encrypt(string text, string key)
    {
        return mode switch
        {
            CaesarMode.Simple => SimpleEncrypt(text, key),
            CaesarMode.Poly => PolyEncrypt(text, key),
            CaesarMode.Core => CoreEncrypt(primeText: text, auxText: key),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string Decrypt(string text, string key)
    {
        return mode switch
        {
            CaesarMode.Simple => SimpleDecrypt(text, key),
            CaesarMode.Poly => PolyDecrypt(text, key),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string SimpleEncrypt(string text, string key)
    {
        var keyChar = key[0];
        var chars = new char[text.Length];
        for (var i = 0; i < text.Length; i++)
            chars[i] = TextConverter.AddChars(text[i], keyChar);
        return new string(chars);
    }

    private static string SimpleDecrypt(string text, string key)
    {
        var keyChar = key[0];
        var chars = new char[text.Length];
        for (var i = 0; i < text.Length; i++)
            chars[i] = TextConverter.SubtractChars(text[i], keyChar);
        return new string(chars);
    }

    private static string PolyEncrypt(string text, string key)
    {
        var result = new StringBuilder();
        var keyState = '_';

        for (var i = 0; i < text.Length; i++)
        {
            var currentTextChar = text[i];
            var keyPosition = i % key.Length;
            keyState = TextConverter.AddChars(keyState, key[keyPosition]);
            result.Append(TextConverter.AddChars(currentTextChar, keyState));
        }

        return result.ToString();
    }

    private static string PolyDecrypt(string text, string key)
    {
        var result = new StringBuilder();
        var keyState = '_';

        for (var i = 0; i < text.Length; i++)
        {
            var currentTextChar = text[i];
            var keyPosition = i % key.Length;
            keyState = TextConverter.AddChars(keyState, key[keyPosition]);
            result.Append(TextConverter.SubtractChars(currentTextChar, keyState));
        }

        return result.ToString();
    }

    private static string CoreEncrypt(string primeText, string auxText)
    {
        if (primeText.Length != 16 || auxText.Length != 16)
            throw new ArgumentException("Each text must be 16 characters long.");

        int[] C1 = [1, -1, 1, -1, 1, -1, 1];
        int[] C2 = [1, -1, 1, -1, 1];

        var aux = auxText.ToNumArray();
        var prime = primeText.ToNumArray();
        var tmp = 0;
        var t1 = 0;
        foreach (var i in aux) { 
            t1 += i;
        }
        var c1 = t1 % 7;
        var c2 = prime[2 * c1 + 1] % 5;
        var c3 = (prime[2 * c2] + prime[2 * c1]) % 16;
        var arr = new int[16];

        for (var i = 0; i < 16; i++)
        {
            var q = (c1 + i) % 7;
            var j = (c2 + i) % 5;
            var p = (c3 + i) % 16;
            var l = i % 16;
            tmp = (tmp + 64 + prime[p] + C1[q] * C2[j] * aux[l]) % 32;
            arr[l] = tmp;
        }

        return arr.ToText();
    }
}