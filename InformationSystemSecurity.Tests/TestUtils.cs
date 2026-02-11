namespace InformationSystemSecurity.tests;

public static class TestUtils
{
    public static int CountDifferences(string str1, string str2)
    {
        var diffCount = 0;
        for (var i = 0; i < str1.Length; i++)
        {
            if (str1[i] != str2[i])
            {
                diffCount++;
            }
        }

        return diffCount;
    }

    public static IEnumerable<object[]> GetCloseInputsTestData()
    {
        var texts = new[] { "ОРЕХ", "ОПЕХ", "ОПЕФ" };
        var keys = new[] { "ХОРОШО_БЫТЬ_ВАМИ", "МОЛЧАНИЕ_ЗОЛОТО_" };

        var closePairs = new List<(string First, string Second)>();
        for (int i = 0; i < texts.Length; i++)
        {
            for (int j = i + 1; j < texts.Length; j++)
            {
                closePairs.Add((texts[i], texts[j]));
            }
        }

        return MixPairsWithKeys(closePairs, keys);
    }

    public static IEnumerable<object[]> GetRotationTestData()
    {
        var plainTexts = new[] { "БЛОК", "АБВГ", "__АА" };
        var keys = new[] { "ХОРОШО_БЫТЬ_ВАМИ", "МОЛЧАНИЕ_ЗОЛОТО_" };
        var shifts = new[] { 1, 2, 3 };

        var rotations = new List<(string First, string Second)>();
        foreach (var plainText in plainTexts)
        {
            foreach (var shift in shifts)
            {
                // Пропускаем сдвиг, равный или превышающий длину строки
                if (shift >= plainText.Length) continue;

                var rotated = RotateText(plainText, shift);
                rotations.Add((plainText, rotated));
            }
        }

        return MixPairsWithKeys(rotations, keys);
    }

    public static IEnumerable<object[]> GetAdditiveHomomorphismData()
    {
        var textPairs = new (string First, string Second)[]
        {
            ("БЛОК", "КОД_"),
            ("_АБВ", "ГДЕЖ"),
            ("АБВГ", "ДЕЖЗ"),
            ("ОРЕХ", "ОПЕХ")
        };

        var keys = new[]
        {
            "ХОРОШО_БЫТЬ_ВАМИ",
            "МОЛЧАНИЕ_ЗОЛОТО_"
        };

        return MixPairsWithKeys(textPairs, keys);
    }

    public static IEnumerable<object[]> GetKeyChangeTestData()
    {
        var texts = new[] { "БЛОК", "_АБВ" };
        var keyPairs = new (string First, string Second)[]
        {
            ("ЖАРОВЫНОСЛИВОСТЬ", "ЖБРОВЫНОСЛИВОСТЬ"),
            ("ЖБРОВЫНОСЛИВОСТЬ", "ЖБРОВЫНОСКИВОСТЬ"),
            ("ЖАРОВЫНОСЛИВОСТЬ", "ЖБРОВЫНОСКИВОСТЬ")
        };

        return MixTextsWithKeyPairs(texts, keyPairs);
    }

    public static IEnumerable<object[]> GetKeyRotationTestData()
    {
        var texts = new[] { "БЛОК", "_АБВ" };
        var keys = new[]
        {
            "ЖАРОВЫНОСЛИВОСТЬ",
            "САЛАМАЛЕЙКУМ_САС",
            "СВИНКА_ПЕППА_ТОП"
        };
        var shifts = new[] { 1, 2, 3 };

        var keyPairs = new List<(string First, string Second)>();
        foreach (var key in keys)
        {
            foreach (var shift in shifts)
            {
                if (shift >= key.Length) continue;
                keyPairs.Add((key, RotateText(key, shift)));
            }
        }

        return MixTextsWithKeyPairs(texts, keyPairs);
    }

    public static IEnumerable<object[]> GetKeyAdditionTestData()
    {
        var texts = new[] { "БЛОК", "_АБВ" };
        var keyPairs = new (string First, string Second)[]
        {
            ("ЖАРОВЫНОСЛИВОСТЬ", "САЛАМАЛЕЙКУМ_САС"),
            ("СВИНКА_ПЕППА_ТОП", "САЛАМАЛЕЙКУМ_САС"),
            ("ЖБРОВЫНОСЛИВОСТЬ", "СВИНКА_ПЕППА_ТОП")
        };

        return MixTextsWithKeyPairs(texts, keyPairs);
    }

    private static IEnumerable<object[]> MixTextsWithKeyPairs(
        IEnumerable<string> texts,
        IEnumerable<(string First, string Second)> keyPairs)
    {
        foreach (var text in texts)
        {
            foreach (var (first, second) in keyPairs)
            {
                yield return [text, first, second];
            }
        }
    }

    private static IEnumerable<object[]> MixPairsWithKeys(
        IEnumerable<(string First, string Second)> pairs,
        IEnumerable<string> keys)
    {
        foreach (var (first, second) in pairs)
        {
            foreach (var key in keys)
            {
                yield return [first, second, key];
            }
        }
    }

    private static string RotateText(string text, int shift)
    {
        return text[shift..] + text[..shift];
    }
}