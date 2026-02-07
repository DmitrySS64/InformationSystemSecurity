using InformationSystemSecurity.domain;
using Xunit;

namespace InformationSystemSecurity.tests
{
    public class Lab1Tests
    {
        #region Alphabet Tests

        [Fact]
        public void TextToNumberArray_And_NumberArrayToText()
        {
            string alphabetString = Alphabet.AlphabetString;

            int[] a = Alphabet.Text2Array(alphabetString);
            string b = Alphabet.Array2Text(a);

            Assert.Equal(Alphabet.AlphabetString, b);
        }

        [Fact]
        public void AddCharacters()
        {
            var a = 'Я';
            var b = 'Ж';
            var c = 'Е';

            Assert.Equal(c, Alphabet.AddChars(a, b));
        }

        [Fact]
        public void SubtractCharacters()
        {
            var a = 'Я';
            var b = 'Ж';
            var c = 'Е';

            Assert.Equal(a, Alphabet.SubtractChars(c, b));
        }

        [Fact]
        public void AddTexts()
        {
            string text1 = "ЕЖИК";
            string text2 = "В_ТУМАНЕ";
            string expected = "ИЖЬЯМАНЕ";

            string result = Alphabet.AddTexts(text1, text2);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SubtractTexts_ShouldSubtractTextsCorrectly()
        {
            string cipherText = "ИЖЬЯМАНЕ";
            string text2 = "В_ТУМАНЕ";
            string expected = "ЕЖИК____";

            string result = Alphabet.SubtractTexts(cipherText, text2);

            Assert.Equal(expected, result);
        }

        #endregion

        #region Caesar Cipher Tests

        [Fact]
        public void Encrypt_WithZeroKey()
        {
            string plainText = "ОЛОЛО_КРИНЖ";
            string key = "_";

            string result = Caesar.Encrypt(plainText, key);

            Assert.Equal(plainText, result);
        }

        [Fact]
        public void Encrypt_WithKey()
        {
            string plainText = "ОЛОЛО_КРИНЖ";
            string key = "Х";
            string expected = "ДБДБДХАЖЯГЭ";

            string result = Caesar.Encrypt(plainText, key);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Decrypt()
        {
            string plainText = "ОЛОЛО_КРИНЖ";
            string key = "Х";

            string encrypted = Caesar.Encrypt(plainText, key);
            string decrypted = Caesar.Decrypt(encrypted, key);

            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void Decrypt_WithZeroKey()
        {
            string cipherText = "ОЛОЛО_КРИНЖ";
            string key = "_";

            string result = Caesar.Decrypt(cipherText, key);

            Assert.Equal(cipherText, result);
        }

        #endregion

        #region Polyalphabetic Cipher Tests

        [Theory]
        [InlineData("ОЛОЛО_КРИНЖ", "Х", "ДЧРГЭГДАОЙШ")]
        [InlineData("ОЛОЛО_КРИНЖ", "ПАНТЕОН", "ЯЭНЮЖЖ_ХОБН")]
        public void PolyEncrypt(string textIn, string key, string textOut)
        {
            string result = Caesar.PolyEncrypt(textIn, key);

            Assert.Equal(textOut, result);
        }

        [Theory]
        [InlineData("ОЛОЛО_КРИНЖ", "Х", "ДЧРГЭГДАОЙШ")]
        [InlineData("ОЛОЛО_КРИНЖ", "ПАНТЕОН", "ЯЭНЮЖЖ_ХОБН")]
        public void PolyDecrypt(string textOut, string key, string textIn)
        {
            string result = Caesar.PolyDecrypt(textIn, key);

            Assert.Equal(textOut, result);
        }

        #endregion

        #region S-Block Cipher Tests

        [Theory]
        [InlineData("БЛОК", "ХОРОШО_БЫТЬ_ВАМИ", "АЗЩЯ")]
        [InlineData("БЛОК", "ЧЕРНОВОЙ_АХИЛЛЕС", "СЮАЖ")]
        [InlineData("ВЛОГ", "ХОРОШО_БЫТЬ_ВАМИ", "БЗЩЧ")]
        [InlineData("ВЛОГ", "ЧЕРНОВОЙ_АХИЛЛЕС", "ТЮА_")]
        public void EncriptSBlock(string textIn, string key, string textOut)
        {
            string result = Caesar.EncryptSBlock(textIn, key);

            Assert.Equal(textOut, result);
        }

        #endregion
    }
}