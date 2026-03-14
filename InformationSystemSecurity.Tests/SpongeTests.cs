using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.tests;

public class SpongeTests
{
    [Fact]
    public void Mix_Col()
    {
        var state = new string[5][];
        for (var i = 0; i < 5; i++)
            state[i] = Enumerable.Repeat("____", 5).ToArray();

        state[1][0] = "__А_";

        var expected = new string[5][]
        {
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "____", "__Г_", "__Л_", "__У_"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"]
        };

        var result = Sponge.MixColumns(state);

        Assert.Equal(result, expected);
    }

    [Fact]
    public void ShatterBlocks() 
    { 
        var state = new string[5][]
        {
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "____", "__Г_", "__Л_", "__У_"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"]
        };

        var expected = new string[5][]
        {
            ["___М", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "____", "__Г_", "__Л_", "__У_"],
            ["__М_", "__А_", "___В", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "___М", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "___Т"]
        };

        var result = Sponge.ShatterBlocks(state);

        Assert.Equal(result, expected);
    }

    [Fact]
    public void ShiftRows() 
    {
        var state = new string[5][]
        {
            ["___М", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "____", "__Г_", "__Л_", "__У_"],
            ["__М_", "__А_", "___В", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "___М", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "___Т"]
        };

        var expected = new string[5][]
        {
            ["___М", "____", "___В", "___М", "___Т"],
            ["__М_", "__А_", "__В_", "__М_", "__Т_"],
            ["__М_", "__А_", "__В_", "__М_", "__У_"],
            ["__М_", "__А_", "__В_", "__Л_", "__Т_"],
            ["__М_", "__А_", "__Г_", "__М_", "__Т_"]
        };

        var result = Sponge.ShiftRows(state);

        Assert.Equal(result, expected);
    }

    [Fact]
    public void Absorb() 
    {
        var state = new string[5][];
        for (var i = 0; i < 5; i++)
            state[i] = Enumerable.Repeat("____", 5).ToArray();

        var in1 = "_А__";
        var in2 = "ВИЛЯ";

        var expected1 = new string[5][]
        {
            ["РЭФД", "ФРИШ", "ЯТЫК", "РЭФД", "ОВКЫ"], 
            ["ЭФДР", "РИШФ", "ТЫКЯ", "ЭФДР", "УУУГ"], 
            ["ЭФДР", "РИШФ", "ТЫКЯ", "ЛЛЛЬ", "ВКЫО"], 
            ["ЭФДР", "РИШФ", "ГГГУ", "ЭФДР", "ВКЫО"], 
            ["ЭФДР", "____", "ТЫКЯ", "ЭФДР", "ВКЫО"]
        };
        var expected2 = new string[5][]
        {
            ["СЗК_", "ЩЗЖП", "ФВЛЯ", "ВДЗЭ", "ПБКН"],
            ["ЦБЖЭ", "ЗЖПЩ", "УФЧЙ", "ДЗЭВ", "УЛЧЬ"],
            ["ЦБЖЭ", "ЗЖПЩ", "УФЧЙ", "ЯЯВВ", "БКНП"],
            ["ЦБЖЭ", "ЦЮЦД", "НЕБЮ", "ХРХЧ", "БКНП"],
            ["ЦБЖЭ", "ИДЭЭ", "ВЛЯФ", "ДЗЭВ", "БКНП"]
        };


        var cipherCaesar = new Caesar(CaesarMode.Core);
        var cipherSponge = new Sponge(cipherCaesar);

        var result1 = cipherSponge.Absorb(state, in1);
        var result2 = cipherSponge.Absorb(result1, in2);

        Assert.Equal(expected1, result1);
        Assert.Equal(expected2, result2);
    }


    [Fact]
    public void Squeeze()
    {
        var state = new string[5][]
        {
            ["БЫ_Щ", "ЙЖ_Б", "ЮФ_Е", "БЫ_Щ", "ЮД_Е"],
            ["Ы_ЩБ", "Ж_БЙ", "Ф_ЕЮ", "Ы_ЩБ", "Л_ЗЗ"],
            ["Ы_ЩБ", "Ж_БЙ", "Ф_ЕЮ", "У_ЧЧ", "Д_ЕЮ"],
            ["Ы_ЩБ", "Ж_БЙ", "Ь_ЗЗ", "Ы_ЩБ", "Д_ЕЮ"],
            ["Ы_ЩБ", "____", "Ф_ЕЮ", "Ы_ЩБ", "Д_ЕЮ"]
        };

        var expectedState1x = new string[5][]
        {
            ["БЕТЧ", "ЙЩЫП", "ЬСЧЬ", "ГЦИН", "СИХЕ"],
            ["ЦКЕЧ", "ЩЫПЙ", "ШЧЮЕ", "ЦИНГ", "АИЩ_"],
            ["ЦКЕЧ", "ЩЫПЙ", "ШЧЮЕ", "ЧЫУЮ", "ИХЕС"],
            ["ЦКЕЧ", "ТЫН_", "ЯАЮГ", "ЮИПН", "ИХЕС"],
            ["ЦКЕЧ", "ХЖЗЙ", "СЧЬЬ", "ЦИНГ", "ИХЕС"]
        };
        var expectedState1xBlock = "ПБЧБ";
        var expectedState2x = new string[5][]
        {
            ["ЗГПЫ", "ЙАБФ", "ЙЦПЕ", "ЬФТЙ", "ПЯЖТ"],
            ["ЫЭКУ", "АБФЙ", "ГШЗС", "ЮЗГГ", "ЬНЮУ"],
            ["УБДЬ", "ЩБТ_", "УГЛ_", "ЙУИУ", "ЩЕАЗ"],
            ["МЖЯГ", "ЭНМЙ", "ФЕБЩ", "ЫЬЙЩ", "ААЖ_"],
            ["ХЬШЛ", "ЕЕТП", "ЦПЕЙ", "ЬНПУ", "ЗЬМЧ"]
        };
        var expectedState2xBlock = "ЛСМО";

        var cipherCaesar = new Caesar(CaesarMode.Core);
        var cipherSponge = new Sponge(cipherCaesar);

        var (state1x, state1xBlock) = cipherSponge.Squeeze(state);
        var (state2x, state2xBlock) = cipherSponge.Squeeze(state1x);

        Assert.Equal(expectedState1x, state1x);
        Assert.Equal(expectedState1xBlock, state1xBlock);

        Assert.Equal(expectedState2x, state2x);
        Assert.Equal(expectedState2xBlock, state2xBlock);
    }

    [Theory]
    [InlineData("КАТЕГОРИЧЕСКИЙ_ИМПЕРАТИВ", "ВРЫТЩС")]
    public void GetHash(string plainText, string expected)
    {
        var cipherCaesar = new Caesar(CaesarMode.Core);
        var cipherSponge = new Sponge(cipherCaesar);

        var result = cipherSponge.GetHash(plainText);

        Assert.Equal(expected, result[..6]);
    }

    [Fact]
    public void GetHash_EmptyString()
    {
        var massege = new string('_', 4 * 16);
        var expected = "ЕНЧНПС";

        var cipherCaesar = new Caesar(CaesarMode.Core);
        var cipherSponge = new Sponge(cipherCaesar);

        var result = cipherSponge.GetHash(massege);

        Assert.Equal(expected, result[..6]);
    }
}

