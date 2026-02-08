using System.Text;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.domain;

public class Caesar(CaesarMode mode = CaesarMode.Poly) : ICipher
{
    public string Encrypt(string text, string key)
    {
        return mode switch
        {
            CaesarMode.Simple => SimpleEncrypt(text, key),
            CaesarMode.Poly => PolyEncrypt(text, key),
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
            chars[i] = Alphabet.AddChars(text[i], keyChar);
        return new string(chars);
    }

    private static string SimpleDecrypt(string text, string key)
    {
        var keyChar = key[0];
        var chars = new char[text.Length];
        for (var i = 0; i < text.Length; i++)
            chars[i] = Alphabet.SubtractChars(text[i], keyChar);
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
            keyState = Alphabet.AddChars(keyState, key[keyPosition]);
            result.Append(Alphabet.AddChars(currentTextChar, keyState));
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
            keyState = Alphabet.AddChars(keyState, key[keyPosition]);
            result.Append(Alphabet.SubtractChars(currentTextChar, keyState));
        }

        return result.ToString();
    }
}