using System.Globalization;
using System.Text;
using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.Tools.StatsCollectors;

public static class SpongeHashStatsCollector
{
    public static void CollectAndPrintStats(string seed, int n = 10000, char target = '_', int position = 0)
    {
        if (n <= 0)
        {
            throw new ArgumentException("n must be positive", nameof(n));
        }

        var alphabet = TextConverter.AlphabetString;
        var alphaLen = TextConverter.AlphabetLength;

        const int expectedHashLen = 64;

        if (position < 0 || position >= expectedHashLen)
        {
            Console.WriteLine($"position must be in [0,{expectedHashLen - 1}]\n");
            return;
        }
        
        var countsByPos = new long[expectedHashLen, alphaLen];
        var targetCounts = new long[expectedHashLen];

        var processedCount = 0;

        var sponge = new Sponge(new Caesar(CaesarMode.Core));

        var h = seed;

        Console.WriteLine($"Collecting hash stats: seed='{seed}', n={n}, target='{target}', position={position}, time={DateTime.UtcNow:O}");

        for (var i = 0; i < n; i++)
        {
            h = sponge.GetHash(h);

            for (var pos = 0; pos < expectedHashLen; pos++)
            {
                var ch = h[pos];
                var idx = alphabet.IndexOf(ch);
                countsByPos[pos, idx]++;
                if (ch == target)
                    targetCounts[pos]++;
            }
            
            processedCount++;
        }

        var baseDir = Environment.CurrentDirectory;
        var freqPath = Path.Combine(baseDir, $"sponge_frequencies_{seed}.csv");
        var targetPath = Path.Combine(baseDir, $"sponge_target_{seed}_target_{target}.csv");
        var positionPath = Path.Combine(baseDir, $"sponge_position_{seed}_pos_{position}.csv");

        SaveFrequenciesCsv(freqPath, countsByPos, alphabet);
        SaveTargetCountsCsv(targetPath, targetCounts, processedCount);
        SavePositionCsv(positionPath, countsByPos, position, alphabet, processedCount);
    }

    private static void SaveFrequenciesCsv(string path, long[,] countsByPos, string alphabet)
    {
        try
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

            var alphaLen = alphabet.Length;
            sw.Write("pos");
            for (var a = 0; a < alphaLen; a++)
            {
                sw.Write(';');
                sw.Write(alphabet[a]);
            }
            sw.WriteLine();

            var rows = countsByPos.GetLength(0);
            for (var pos = 0; pos < rows; pos++)
            {
                sw.Write(pos.ToString(CultureInfo.InvariantCulture));
                for (var a = 0; a < alphaLen; a++)
                {
                    sw.Write(';');
                    sw.Write(countsByPos[pos, a].ToString(CultureInfo.InvariantCulture));
                }
                sw.WriteLine();
            }

            Console.WriteLine($"Saved frequencies CSV: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save frequencies CSV '{path}': {ex.Message}");
        }
    }

    private static void SaveTargetCountsCsv(string path, long[] targetCounts, int processedCount)
    {
        try
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

            sw.WriteLine("pos;count;frequency");
            for (var pos = 0; pos < targetCounts.Length; pos++)
            {
                var count = targetCounts[pos];
                var freq = processedCount > 0 ? (double)count / processedCount : 0.0;
                sw.Write(pos.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(count.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.WriteLine(freq.ToString("F6", CultureInfo.InvariantCulture));
            }

            Console.WriteLine($"Saved target counts CSV: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save target counts CSV '{path}': {ex.Message}");
        }
    }

    private static void SavePositionCsv(string path, long[,] countsByPos, int position, string alphabet, int processedCount)
    {
        try
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

            sw.WriteLine("symbol;count;frequency");
            var alphaLen = alphabet.Length;
            for (var a = 0; a < alphaLen; a++)
            {
                var symbol = alphabet[a];
                var count = countsByPos[position, a];
                var freq = processedCount > 0 ? (double)count / processedCount : 0.0;
                sw.Write(symbol);
                sw.Write(';');
                sw.Write(count.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.WriteLine(freq.ToString("F6", CultureInfo.InvariantCulture));
            }

            Console.WriteLine($"Saved position CSV: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save position CSV '{path}': {ex.Message}");
        }
    }
}