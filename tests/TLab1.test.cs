using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InformationSystemSecurity.lib;
using NUnit.Framework;

namespace InformationSystemSecurity.tests
{
    [TestFixture]
    internal class TLab1
    {
        [Test]
        public void Alphabet_Text2Array_and_back() {
            var a = Alphabet.Text2Array(Alphabet.AlphabetString);
            var b = Alphabet.Array2Text(a);

            Assert.That(b, Is.EqualTo(Alphabet.AlphabetString), "Ошибка в методах Text2Array и Array2Text");
        }

        [Test]
        public void Alphabet_AddChars_and_SubtractChars()
        {
            var a = 'Я';
            var b = 'Ж';
            var c = 'Е';

            Assert.That(Alphabet.AddChars(a,b), Is.EqualTo(c), "Ошибка в методе AddChars");
            Assert.That(Alphabet.SubtractChars(c, b), Is.EqualTo(a), "Ошибка в методе SubtractChars");
        }

        [Test]
        public void Alphabet_AddTexts_and_SubtractTexts() {
            string a = "ЕЖИК";
            string a_1 = "ЕЖИК____";
            string b = "В_ТУМАНЕ";
            string c = "ИЖЬЯМАНЕ";

            Assert.That(Alphabet.AddTexts(a, b), Is.EqualTo(c), "Ошибка в методе AddTexts");
            Assert.That(Alphabet.SubtractTexts(c, b), Is.EqualTo(a_1), "Ошибка в методе SubtractTexts");
        }

        [Test]
        public void Caesar_Encript()
        {
            string _in = "ОЛОЛО_КРИНЖ";
            string k1 = "_";
            string k2 = "Х";

            string out2 = "ДБДБДХАЖЯГЭ";

            Assert.That(Caesar.Encript(_in, k1), Is.EqualTo(_in), "Ошибка в методе Caesar.Encript");
            Assert.That(Caesar.Encript(_in, k2), Is.EqualTo(out2), "Ошибка в методе Caesar.Encript");
        }

        [Test]
        public void Caesar_Decrypt()
        {
            string in1 = "ОЛОЛО_КРИНЖ";
            string in2 = "ДБДБДХАЖЯГЭ";
            string k1 = "_";
            string k2 = "Х";

            string _out = "ОЛОЛО_КРИНЖ";

            Assert.That(Caesar.Decrypt(in1, k1), Is.EqualTo(_out), "Ошибка в методе Caesar.Decrypt");
            Assert.That(Caesar.Decrypt(in2, k2), Is.EqualTo(_out), "Ошибка в методе Caesar.Decrypt");
        }

        [Test]
        public void Caesar_PolyEncript()
        {
            string 
                in1 = "ОЛОЛО_КРИНЖ",
                in2 = in1,
                k1 = "Х",
                k2 = "ПАНТЕОН",
                out1 = "ДЧРГЭГДАОЙШ",
                out2 = "ЯЭНЮЖЖ_ХОБН";

            Assert.That(Caesar.PolyEncript(in1, k1), Is.EqualTo(out1), "Ошибка в методе Caesar.PolyEncript");
            Assert.That(Caesar.PolyEncript(in2, k2), Is.EqualTo(out2), "Ошибка в методе Caesar.PolyEncript");
        }

        [Test]
        public void Caesar_PolyDecrypt()
        {
            string
                in1 = "ОЛОЛО_КРИНЖ",
                in2 = in1,
                k1 = "Х",
                k2 = "ПАНТЕОН",
                out1 = "ДЧРГЭГДАОЙШ",
                out2 = "ЯЭНЮЖЖ_ХОБН";

            Assert.That(Caesar.PolyDecrypt(out1, k1), Is.EqualTo(in1), "Ошибка в методе Caesar.PolyDecrypt");
            Assert.That(Caesar.PolyDecrypt(out2, k2), Is.EqualTo(in2), "Ошибка в методе Caesar.PolyDecrypt");
        }


        [Test]
        public void Caesar_EncriptSBlock()
        {
            string
                in1 = "БЛОК",
                in2 = "ВЛОГ",
                k1 = "ХОРОШО_БЫТЬ_ВАМИ",
                k2 = "ЧЕРНОВОЙ_АХИЛЛЕС",
                out11 = "АЗЩЯ",
                out12 = "СЮАЖ",
                out21 = "БЗЩЧ",
                out22 = "ТЮА_";

            Assert.That(Caesar.EncriptSBlock(in1, k1), Is.EqualTo(out11), "Ошибка в методе Caesar.EncriptSBlock");
            Assert.That(Caesar.EncriptSBlock(in1, k2), Is.EqualTo(out12), "Ошибка в методе Caesar.EncriptSBlock");
            Assert.That(Caesar.EncriptSBlock(in2, k1), Is.EqualTo(out21), "Ошибка в методе Caesar.EncriptSBlock");
            Assert.That(Caesar.EncriptSBlock(in2, k2), Is.EqualTo(out22), "Ошибка в методе Caesar.EncriptSBlock");
        }

    }
}
