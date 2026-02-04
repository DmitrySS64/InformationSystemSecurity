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
            int keyNum = Alphabet.Char2Num(key[0]);
            return Alphabet.Array2Text(Alphabet.Text2Array(text).Select(n=>(n + keyNum)).ToArray());
        }
        public static string Decrypt(string text, string key)
        {
            int keyNum = Alphabet.Char2Num(key[0]);
            return Alphabet.Array2Text(Alphabet.Text2Array(text).Select(n => (n - keyNum + Alphabet.Length)).ToArray());
        }

        public static string PolyEncript(string text, string key)
        {
            string _out = "";
            char t_k = Alphabet.Num2Char(0);
            int KeyLen = key.Length;
            for (int i = 0; i < text.Length; i++)
            {
                char t_i = text[i];
                int q = i - KeyLen * (int)(i/KeyLen);
                t_k = Alphabet.AddChars(t_k, key[q]);
                _out += Alphabet.AddChars(t_i, t_k);
            }

            return _out;
        }
        public static string PolyDecrypt(string text, string key)
        {
            string _out = "";
            char t_k = Alphabet.Num2Char(0);
            int KeyLen = key.Length;
            for (int i = 0; i < text.Length; i++)
            {
                char t_i = text[i];
                int q = i - KeyLen * (int)(i / KeyLen);
                t_k = Alphabet.AddChars(t_k, key[q]);
                _out += Alphabet.SubtractChars(t_i, t_k);
            }

            return _out;
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
