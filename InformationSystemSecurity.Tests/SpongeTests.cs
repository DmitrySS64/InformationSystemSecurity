using InformationSystemSecurity.domain;

namespace InformationSystemSecurity.tests;

public class SpongeTests
{
    [Fact]
    public void Mix_Col()
    {
        var state = new string[5][];
        for (int i = 0; i < 5; i++)
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
        for (int i = 0; i < 5; i++)
            state[i] = Enumerable.Repeat("____", 5).ToArray();

        var @in = "_А__";

        var expected = new string[5][]
        {
            ["_ЫТК", "_ЖЯЦ", "_ФЭД", "_ЫТК", "_ДМФ"],
            ["ЫТК_", "ЖЯЦ_", "ФЭД_", "ЫТК_", "ЛЛЛ_"],
            ["ЫТК_", "ЖЯЦ_", "ФЭД_", "УУУ_", "ДМФ_"],
            ["ЫТК_", "ЖЯЦ_", "ЬЬЬ_", "ЫТК_", "ДМФ_"],
            ["ЫТК_", "____", "ФЭД_", "ЫТК_", "ДМФ_"]
        };

        var chiperCaesar = new Caesar(domain.Enums.CaesarMode.Core);
        var chiperSponge = new Sponge(chiperCaesar);

        var result = chiperSponge.Absorb(state, @in);

        Assert.Equal(result, expected);
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

        var expected = new string[5][]
{
            ["БЕТЧ", "ЙЩЫП", "ЬСЧЬ", "ГЦИН", "СИХЕ"],
            ["ЦКЕЧ", "ЩЫПЙ", "ШЧЮЕ", "ЦИНГ", "АИЩ_"],
            ["ЦКЕЧ", "ЩЫПЙ", "ШЧЮЕ", "ЧЫУЮ", "ИХЕС"],
            ["ЦКЕЧ", "ТЫН_", "ЯАЮГ", "ЮИПН", "ИХЕС"],
            ["ЦКЕЧ", "ХЖЗЙ", "СЧЬЬ", "ЦИНГ", "ИХЕС"]
        };
        var expectedBlock = "УУЦК";

        var chiperCaesar = new Caesar(domain.Enums.CaesarMode.Core);
        var chiperSponge = new Sponge(chiperCaesar);

        var (result, resultBlock) = chiperSponge.Squeeze(state);

        Assert.Equal(expected, result);
        Assert.Equal(expectedBlock, resultBlock);
    }

    [Theory]
    [InlineData("КАТЕГОРИЧЕСКИЙ_ИМПЕРАТИВ", "_ДВЖГЭРЬВВЬЛЕЩНЯУУ_РТБПЮЯЯЩКЭЮЦПЭКЩЫЛКЯДЛДИЫКШМ_НИЧБЩКЬУЛДСФЛЧТЕ")]
    public void GetHash(string plainText, string expected)
    {
        var chiperCaesar = new Caesar(domain.Enums.CaesarMode.Core);
        var chiperSponge = new Sponge(chiperCaesar);

        var result = chiperSponge.GetHash(plainText);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetHash_EmptyString()
    {
        var massege = new string('_', 4 * 16);
        var expected = "ПСФНЫЕ";

        var chiperCaesar = new Caesar(domain.Enums.CaesarMode.Core);
        var chiperSponge = new Sponge(chiperCaesar);

        var result = chiperSponge.GetHash(massege);

        Assert.Equal(expected, result[..6]);
    }
}

