using System;
using System.Linq;

namespace InformationSystemSecurity.lib
{
    internal static class Alphabet
    {
        public const string AlphabetString = "_АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЫЬЭЮЯ";
        public static readonly int Length = AlphabetString.Length;

        public static int Char2Num(char c)
        {
            int charCode = (int)c;

            if (charCode == 95) return 0;

            int baseNumber = charCode - 1039;

            if (charCode > 1066)
            {
                return baseNumber - 1;
            }

            return baseNumber;
        }

        public static char Num2Char(int num)
        {
            if (num == 0) return '_';

            int charCode = num + 1039;

            if (num > 26)
            {
                charCode++;
            }

            return (char)charCode;
        }
        
        public static int[] Text2Array(string text)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<int>();

            int[] result = new int[text.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Char2Num(text[i]);
            }

            return result;
        }

        public static string Array2Text(int[] nums)
        {
            if (nums == null || nums.Length == 0) return string.Empty;

            var chars = nums.Select(Num2Char);

            string result = string.Concat(chars);

            return result;
        }

        public static char AddChars(char c1, char c2)
        {
            int firstNum = Char2Num(c1);
            int secondNum = Char2Num(c2);
            int resultNum = (firstNum + secondNum) % Length;

            return Num2Char(resultNum);
        }

        public static char SubtractChars(char c1, char c2)
        {
            int firstNum = Char2Num(c1);
            int secondNum = Char2Num(c2);
            int resultNum = (firstNum - secondNum + Length) % Length;

            return Num2Char(resultNum);
        }

        public static string AddTexts(string text1, string text2)
        {
            int minLen = Math.Min(text1.Length, text2.Length);

            string text = text1;
            if (text1.Length < text2.Length)
            {
                text = text2;
            }
            int maxLen = text.Length;

            char[] result = new char[maxLen];
            for (int i = 0; i < minLen; i++)
            {
                result[i] = AddChars(text1[i], text2[i]);
            }
            if (maxLen > minLen)
            {
                for (int i = minLen; i < maxLen; i++)
                {
                    result[i] = text[i];
                }
            }

            return new string(result);
        }

        public static string SubtractTexts(string text1, string text2)
        {
            int firstLength = text1.Length;
            int secondLength = text2.Length;
            int maxLength = Math.Max(firstLength, secondLength);

            char[] resultChars = new char[maxLength];

            int commonLength = Math.Min(firstLength, secondLength);

            for (int i = 0; i < commonLength; i++)
            {
                resultChars[i] = SubtractChars(text1[i], text2[i]);
            }

            if (firstLength > secondLength)
            {
                for (int i = commonLength; i < maxLength; i++)
                {
                    resultChars[i] = text1[i];
                }
            }
            else if (secondLength > firstLength)
            {
                for (int i = commonLength; i < maxLength; i++)
                {
                    resultChars[i] = SubtractChars('_', text2[i]);
                }
            }

            return new string(resultChars);
        }
    }
}
