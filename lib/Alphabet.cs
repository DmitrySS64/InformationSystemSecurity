using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystemSecurity.lib
{
    internal static class Alphabet
    {
        public const string ABC = "_АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЫЬЭЮЯ";
        public static readonly int Length = ABC.Length;

        public static int Char2Num(char c) => ABC.IndexOf(c);
        public static char Num2Char(int num) => ABC[(num + Length) % Length];
        public static int[] Text2Array(string text) => text.Select(Char2Num).ToArray();
        public static string Array2Text(int[] nums) => string.Concat(nums.Select(Num2Char));

        public static char AddChars(char c1, char c2) => Num2Char(Char2Num(c1) + Char2Num(c2) + Length);
        public static char SubtractChars(char c1, char c2) => Num2Char(Char2Num(c1) - Char2Num(c2));

        public static string AddTexts(string text1, string text2)
        {
            int maxLen = Math.Max(text1.Length, text2.Length);
            text1 = text1.PadRight(maxLen, '_');
            text2 = text2.PadRight(maxLen, '_');
            var result = text1.Zip(text2, (c1, c2) => AddChars(c1, c2)).ToArray();
            return new string(result);
        }
        public static string SubtractTexts(string text1, string text2)
        {
            int maxLen = Math.Max(text1.Length, text2.Length);
            text1 = text1.PadRight(maxLen, '_');
            text2 = text2.PadRight(maxLen, '_');
            var result = text1.Zip(text2, (c1, c2) => SubtractChars(c1, c2)).ToArray();
            return new string(result);
        }
    }
}
