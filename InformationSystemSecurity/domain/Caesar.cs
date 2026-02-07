namespace InformationSystemSecurity.domain;

public class Caesar
{
    public static string Encrypt(string text, string key)
    {
        char keyChar = key[0];

        char[] chars = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            chars[i] = Alphabet.AddChars(text[i], keyChar);
        }

        return new string(chars);
    }
    public static string Decrypt(string text, string key)
    {
        char keyChar = key[0];

        char[] chars = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            chars[i] = Alphabet.SubtractChars(text[i], keyChar);
        }

        return new string(chars);
    }

    public static string PolyEncrypt(string text, string key)
    {
        string result = "";
        char keyState = '_';
        int keyLength = key.Length;

        for (int i = 0; i < text.Length; i++)
        {
            char currentTextChar = text[i];
            int keyPosition = i % keyLength;
            keyState = Alphabet.AddChars(keyState, key[keyPosition]);
            result += Alphabet.AddChars(currentTextChar, keyState);
        }

        return result;
    }
    public static string PolyDecrypt(string text, string key)
    {
        string result = "";
        char keyState = '_';
        int keyLength = key.Length;

        for (int i = 0; i < text.Length; i++)
        {
            char currentTextChar = text[i];
            int keyPosition = i % keyLength;
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