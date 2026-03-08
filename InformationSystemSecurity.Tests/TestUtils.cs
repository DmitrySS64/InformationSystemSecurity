using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.tests;

public static class TestUtils
{
    // Глобальные настройки для добавления случайных тест-кейсов
    private const int RandomTextCasesPerGenerator = 5;
    private const int RandomKeyCasesPerGenerator = 50;
    // private static Random _random = Random.Shared;
    private static Random _random = new(7);

    
    public static void SetRandomSeed(int seed)
    {
        _random = new Random(seed);
    }

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
        var closePairs = new List<(string First, string Second)>
        {
            ("ОРЕХ", "ОПЕХ"),
            ("ОПЕХ", "ОПЕФ")
        };
        var keys = new List<string> { "ХОРОШО_БЫТЬ_ВАМИ", "МОЛЧАНИЕ_ЗОЛОТО_" };

        if (RandomKeyCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomKeyCasesPerGenerator; i++)
            {
                keys.Add(GenerateRandomText(16));
            }
        }

        if (RandomTextCasesPerGenerator > 0)
        {
            closePairs.AddRange(GenerateRandomClosePairs(RandomTextCasesPerGenerator / 2, 4));
            // closePairs.AddRange(GenerateRandomEqualSumPairs(RandomTextCasesPerGenerator/2, 4));
        }

        return MixPairsWithKeys(closePairs, keys);
    }

    public static IEnumerable<object[]> GetRotationTestData()
    {
        var plainTexts = new List<string> { "БЛОК", "АБВГ", "__АА" };
        var keys = new List<string> { "ХОРОШО_БЫТЬ_ВАМИ", "МОЛЧАНИЕ_ЗОЛОТО_" };

        if (RandomTextCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomTextCasesPerGenerator; i++)
            {
                plainTexts.Add(GenerateRandomText(4));
            }
        }

        if (RandomKeyCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomKeyCasesPerGenerator; i++)
            {
                keys.Add(GenerateRandomText(16));
            }
        }

        var rotations = new List<(string First, string Second)>();
        foreach (var plainText in plainTexts)
        {
            rotations.AddRange(GetAllRotations(plainText));
        }

        return MixPairsWithKeys(rotations, keys);
    }

    public static IEnumerable<object[]> GetAdditiveHomomorphismData()
    {
        var textPairs = new List<(string First, string Second)>
        {
            ("БЛОК", "КОД_"),
            ("_АБВ", "ГДЕЖ"),
            ("АБВГ", "ДЕЖЗ"),
            ("ОРЕХ", "ОПЕХ")
        };

        if (RandomTextCasesPerGenerator > 0)
        {
            textPairs.AddRange(GenerateRandomPairs(RandomTextCasesPerGenerator, 4));
        }

        var keys = new List<string>
        {
            "ХОРОШО_БЫТЬ_ВАМИ",
            "МОЛЧАНИЕ_ЗОЛОТО_"
        };

        if (RandomKeyCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomKeyCasesPerGenerator; i++)
            {
                keys.Add(GenerateRandomText(16));
            }
        }

        return MixPairsWithKeys(textPairs, keys);
    }

    public static IEnumerable<object[]> GetKeyChangeTestData()
    {
        var texts = new List<string> { "БЛОК", "_АБВ" };
        var keyPairs = new List<(string First, string Second)>
        {
            ("ЖАРОВЫНОСЛИВОСТЬ", "ЖБРОВЫНОСЛИВОСТЬ"),
            ("ЖБРОВЫНОСЛИВОСТЬ", "ЖБРОВЫНОСКИВОСТЬ"),
            ("ЖАРОВЫНОСЛИВОСТЬ", "ЖБРОВЫНОСКИВОСТЬ")
        };

        if (RandomTextCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomTextCasesPerGenerator; i++)
            {
                texts.Add(GenerateRandomText(4));
            }
        }

        if (RandomKeyCasesPerGenerator > 0)
        {
            keyPairs.AddRange(GenerateRandomClosePairs(RandomKeyCasesPerGenerator, 16));
        }

        return MixTextsWithKeyPairs(texts, keyPairs);
    }

    public static IEnumerable<object[]> GetKeyRotationTestData()
    {
        var texts = new List<string> { "БЛОК", "_АБВ" };
        var keys = new List<string>
        {
            "ЖАРОВЫНОСЛИВОСТЬ",
            "САЛАМАЛЕЙКУМ_САС",
            "СВИНКА_ПЕППА_ТОП"
        };

        if (RandomTextCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomTextCasesPerGenerator; i++)
            {
                texts.Add(GenerateRandomText(4));
            }
        }

        if (RandomKeyCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomKeyCasesPerGenerator; i++)
            {
                keys.Add(GenerateRandomText(16));
            }
        }

        var keyPairs = new List<(string First, string Second)>();
        foreach (var key in keys)
        {
            keyPairs.AddRange(GetAllRotations(key));
        }

        return MixTextsWithKeyPairs(texts, keyPairs);
    }
    

    public static IEnumerable<object[]> GetKeyAdditionTestData()
    {
        var texts = new List<string> { "БЛОК", "_АБВ" };
        var keyPairs = new List<(string First, string Second)>
        {
            ("ЖАРОВЫНОСЛИВОСТЬ", "САЛАМАЛЕЙКУМ_САС"),
            ("СВИНКА_ПЕППА_ТОП", "САЛАМАЛЕЙКУМ_САС"),
            ("ЖБРОВЫНОСЛИВОСТЬ", "СВИНКА_ПЕППА_ТОП")
        };

        if (RandomTextCasesPerGenerator > 0)
        {
            for (var i = 0; i < RandomTextCasesPerGenerator; i++)
            {
                texts.Add(GenerateRandomText(4));
            }
        }

        if (RandomKeyCasesPerGenerator > 0)
        {
            keyPairs.AddRange(GenerateRandomPairs(RandomKeyCasesPerGenerator, 16));
        }

        return MixTextsWithKeyPairs(texts, keyPairs);
    }
    

    private static string GenerateRandomText(int length)
    {
        var alphabet = Converter.AlphabetString;
        var result = new char[length];
        for (var i = 0; i < length; i++)
        {
            var index = _random.Next(alphabet.Length);
            result[i] = alphabet[index];
        }

        return new string(result);
    }

    private static IEnumerable<(string Original, string Rotated)> GetAllRotations(string text, int maxShift = 1)
    {
        var pairs = new List<(string Original, string Rotated)>();
        maxShift = maxShift < 0 ? text.Length-1 : maxShift;
        for (var shift = 1; shift <= maxShift; shift++)
        {
            var rotated = RotateText(text, shift);
            if (rotated != text)
            {
                pairs.Add((text, rotated));
            }
        }
        
        return pairs;
    }

    private static IEnumerable<(string First, string Second)> GenerateRandomClosePairs(int count, int length)
    {
        for (var i = 0; i < count; i++)
        {
            var baseText = GenerateRandomText(length);
            var indexToChange = _random.Next(length);
            var originalChar = baseText[indexToChange];
            var newChar = originalChar;

            while (newChar == originalChar)
            {
                newChar = Converter.AlphabetString[_random.Next(Converter.AlphabetLength)];
            }

            var modified = baseText.ToCharArray();
            modified[indexToChange] = newChar;

            yield return (baseText, new string(modified));
        }
    }

    private static IEnumerable<(string First, string Second)> GenerateRandomEqualSumPairs(int count, int length)
    {
        if (length < 2)
        {
            length = 2;
        }

        for (var i = 0; i < count; i++)
        {
            var baseText = GenerateRandomText(length);
            var chars = baseText.ToCharArray();

            var indexA = _random.Next(length);
            var indexB = _random.Next(length - 1);
            if (indexB >= indexA) indexB++;

            var numA = chars[indexA].ToNum();
            var numB = chars[indexB].ToNum();

            numA = (numA - 1 + Converter.AlphabetLength) % Converter.AlphabetLength;
            numB = (numB + 1) % Converter.AlphabetLength;

            chars[indexA] = numA.ToChar();
            chars[indexB] = numB.ToChar();

            yield return (baseText, new string(chars));
        }
    }

    private static IEnumerable<(string First, string Second)> GenerateRandomPairs(int count, int length)
    {
        for (var i = 0; i < count; i++)
        {
            var key1 = GenerateRandomText(length);
            var key2 = GenerateRandomText(length);
            while (key2 == key1)
            {
                key2 = GenerateRandomText(length);
            }
            yield return (key1, key2);
        }
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

    public static IEnumerable<object[]> GetCBlockTestData()
    {
        var entityStr = new string('_', 16);
        yield return
        [
            new[] { "ХОРОШО_БЫТЬ_ВАМИ" },
            CompressMode.Out16,
            "ЯМЫШСХБ_ГФАМЭ_ЛЗ"
        ];
        yield return
        [
            new[] { "ХОРОШО_БЫТЬ_ВАМИ" },
            CompressMode.Out8,
            "РВЭШАФМФ"
        ];
        yield return
        [
            new[] { "ХОРОШО_БЫТЬ_ВАМИ", "КЬЕРКЕГОР_ПРОПАЛ" },
            CompressMode.Out4,
            "ЕЗЦР"
        ];
        yield return
        [
            new[] { entityStr, entityStr, entityStr, entityStr},
            CompressMode.Out4,
            "ЭЧЧЯ"
        ];
        yield return
        [
            new[] { entityStr, entityStr, entityStr, entityStr},
            CompressMode.Out8,
            "ЦНУЬЩХЬЭ"
        ];
        yield return
        [
            new[] { entityStr, entityStr, entityStr, entityStr},
            CompressMode.Out16,
            "ЫЕРФЬЗВЖРЖЙЯИОСЮ"
        ];
    }

    public static IEnumerable<object[]> GetCBlockSensitivityTestData()
    {
        var textsLists = new List<List<string>>
        {
            new()
            {
                "ХОРОШО_БЫТЬ_ВАМИ",
            },
            new()
            {
                "ХОРОШО_БЫТЬ_ВАМИ"
            },
            new()
            {
                "_______________А",
                "А_______________",
                "_______А________",
                "________________",
            },
            new()
            {
                "ХОРОШО_БЫТЬ_ВАМИ",
                "КЬЕРКЕГОР_ПРОПАЛ"
            }
            
        };
        var modifiedTextsLists = new List<List<string>>
        {
            new()
            {
                "ХОРОШО_БЫТЬ_ВАМИ",
                "________________"
            },
            new()
            {
                "ХОРОШО_БЫТЬ_ВАМИ",
                "________А_______"
            },
            new()
            {
                "_______________А",
                "________________",
                "________________",
                "________________",
            },
            new()
            {
                "ЧЕРНЫЙ_АББАТ_ПОЛ",
                "ХОРОШО_БЫТЬ_ВАМИ",
                "КЬЕРКЕГОР_ПРОПАЛ"
            }
            
        };

        for (var i = 0; i < textsLists.Count; i++)
        {
            var keys = textsLists[i].ToArray();
            var modifiedKeys = modifiedTextsLists[i].ToArray();
            yield return [keys, modifiedKeys, CompressMode.Out16, 10];
            yield return [keys, modifiedKeys, CompressMode.Out8, 5];
            yield return [keys, modifiedKeys, CompressMode.Out4, 3];
        }

    }
}