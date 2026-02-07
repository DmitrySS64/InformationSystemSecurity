using InformationSystemSecurity.lib;
using NUnit.Framework;
using System.Security.Permissions;

namespace InformationSystemSecurity.tests
{
    [TestFixture]
    public class Lab1Tests
    {
        #region Alphabet Tests
        [Test]
        public void TextToNumberArray_And_NumberArrayToText() {
            string alphabetString = Alphabet.AlphabetString;
            
            int[] a = Alphabet.Text2Array(alphabetString);
            string b = Alphabet.Array2Text(a);

            Assert.That(b, Is.EqualTo(Alphabet.AlphabetString));
        }

        [Test]
        public void AddCharacters()
        {
            var a = 'Я';
            var b = 'Ж';
            var c = 'Е';

            Assert.That(Alphabet.AddChars(a,b), Is.EqualTo(c));
        }

        [Test]
        public void SubtractCharacters()
        {
            var a = 'Я';
            var b = 'Ж';
            var c = 'Е';

            Assert.That(Alphabet.SubtractChars(c, b), Is.EqualTo(a));
        }

        [Test]
        public void AddTexts()
        {
            string text1 = "ЕЖИК";
            string text2 = "В_ТУМАНЕ";
            string expected = "ИЖЬЯМАНЕ";

            string result = Alphabet.AddTexts(text1, text2);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SubtractTexts_ShouldSubtractTextsCorrectly()
        {
            string cipherText = "ИЖЬЯМАНЕ";
            string text2 = "В_ТУМАНЕ";
            string expected = "ЕЖИК____";

            string result = Alphabet.SubtractTexts(cipherText, text2);

            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region Caesar Cipher Tests

        [Test]
        public void Encrypt_WithZeroKey()
        {
            string plainText = "ОЛОЛО_КРИНЖ";
            string key = "_";

            string result = Caesar.Encrypt(plainText, key);

            Assert.That(result, Is.EqualTo(plainText));
        }

        [Test]
        public void Encrypt_WithKey()
        {
            // Arrange
            string plainText = "ОЛОЛО_КРИНЖ";
            string key = "Х";
            string expected = "ДБДБДХАЖЯГЭ";

            // Act
            string result = Caesar.Encrypt(plainText, key);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Decrypt()
        {
            // Arrange
            string plainText = "ОЛОЛО_КРИНЖ";
            string key = "Х";

            // Act
            string encrypted = Caesar.Encrypt(plainText, key);
            string decrypted = Caesar.Decrypt(encrypted, key);

            // Assert
            Assert.That(decrypted, Is.EqualTo(plainText));
        }

        [Test]
        public void Decrypt_WithZeroKey()
        {
            // Arrange
            string cipherText = "ОЛОЛО_КРИНЖ";
            string key = "_";

            // Act
            string result = Caesar.Decrypt(cipherText, key);

            // Assert
            Assert.That(result, Is.EqualTo(cipherText));
        }

        #endregion

        #region Polyalphabetic Cipher Tests

        [TestCase("ОЛОЛО_КРИНЖ", "Х", "ДЧРГЭГДАОЙШ")]
        [TestCase("ОЛОЛО_КРИНЖ", "ПАНТЕОН", "ЯЭНЮЖЖ_ХОБН")]
        public void PolyEncrypt(string textIn, string key, string textOut)
        {
            string result = Caesar.PolyEncrypt(textIn, key);

            Assert.That(result, Is.EqualTo(textOut));
        }

        [TestCase("ОЛОЛО_КРИНЖ", "Х", "ДЧРГЭГДАОЙШ")]
        [TestCase("ОЛОЛО_КРИНЖ", "ПАНТЕОН", "ЯЭНЮЖЖ_ХОБН")]
        public void PolyDecrypt(string textOut, string key, string textIn)
        {
            string result = Caesar.PolyDecrypt(textIn, key);
            Assert.That(result, Is.EqualTo(textOut));
        }

        #endregion

        #region S-Block Cipher Tests

        [TestCase("БЛОК", "ХОРОШО_БЫТЬ_ВАМИ", "АЗЩЯ")]
        [TestCase("БЛОК", "ЧЕРНОВОЙ_АХИЛЛЕС", "СЮАЖ")]
        [TestCase("ВЛОГ", "ХОРОШО_БЫТЬ_ВАМИ", "БЗЩЧ")]
        [TestCase("ВЛОГ", "ЧЕРНОВОЙ_АХИЛЛЕС", "ТЮА_")]
        public void EncriptSBlock(string textIn, string key, string textOut)
        {
            string result = Caesar.EncryptSBlock(textIn, key);

            Assert.That(result, Is.EqualTo(textOut));
        }

        #endregion
    }
}
