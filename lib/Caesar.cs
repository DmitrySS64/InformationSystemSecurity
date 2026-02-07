using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystemSecurity.lib
{
    internal class Caesar
    {
        public static string Encript(string text, string key)
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

        public static string PolyEncript(string text, string key)
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

        public static string EncriptSBlock(string text, string key) {

            throw new NotImplementedException();
        }
        public static string DecriptSBlock(string text, string key)
        {

            throw new NotImplementedException();
        }
    }
}
