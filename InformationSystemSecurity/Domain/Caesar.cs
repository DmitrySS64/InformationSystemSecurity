namespace InformationSystemSecurity.domain;

public static class Caesar
{
    public static string Encrypt(string text, string key)
    {
        var keyChar = key[0];

        var chars = new char[text.Length];
        for (var i = 0; i < text.Length; i++)
            chars[i] = Alphabet.AddChars(text[i], keyChar);

        return new string(chars);
    }
    public static string Decrypt(string text, string key)
    {
        var keyChar = key[0];

        var chars = new char[text.Length];
        for (var i = 0; i < text.Length; i++)
            chars[i] = Alphabet.SubtractChars(text[i], keyChar);

        return new string(chars);
    }

    public static string PolyEncrypt(string text, string key)
    {
        var result = "";
        var keyState = '_';

        for (var i = 0; i < text.Length; i++)
        {
            var currentTextChar = text[i];
            var keyPosition = i % key.Length;
            keyState = Alphabet.AddChars(keyState, key[keyPosition]);
            result += Alphabet.AddChars(currentTextChar, keyState);
        }

        return result;
    }
    public static string PolyDecrypt(string text, string key)
    {
        var result = "";
        var keyState = '_';

        for (var i = 0; i < text.Length; i++)
        {
            var currentTextChar = text[i];
            var keyPosition = i % key.Length;
            keyState = Alphabet.AddChars(keyState, key[keyPosition]);
            result += Alphabet.SubtractChars(currentTextChar, keyState);
        }

        return result;
    }

    public static string EncryptSBlock(string text, string key)
    {
        throw new NotImplementedException();
    }
    public static string DecryptSBlock(string text, string key)
    {
        throw new NotImplementedException();
    }
}