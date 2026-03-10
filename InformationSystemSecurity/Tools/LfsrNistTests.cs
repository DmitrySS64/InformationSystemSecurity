using System.Globalization;
using System.Numerics;
using System.Text;
using InformationSystemSecurity.domain;
using InformationSystemSecurity.Domain.Lsfr;

namespace InformationSystemSecurity.Tools;

public static class LfsrNistTests
{
    private const int DefaultSeedCount = 200;
    private const int DefaultOutputsPerSeed = 200;
    private const string DefaultCsvFileName = "aslfsr_monobit_stats.csv";
    private const string DefaultMaxOnesCsvFileName = "aslfsr_max_ones_stats.csv";
    private const int StateCount = 3;
    private const int BitsPerOutput = 20;
    private const int MinAcceptedMaxOnes = 10;
    private const int MaxAcceptedMaxOnes = 15;
    private const ulong StateMask = 0xFFFFFUL;

    private static readonly ulong[] DefaultTSet =
    [
        0b_1001_0000_0000_0000_0000UL,
        0b_0111_0000_0000_0000_1000UL,
        0b_0010_0000_0100_0000_0000UL
    ];

    public readonly record struct AsLfsrMonobitRunResult(
        int RunIndex,
        string Seed,
        long N0,
        long N1,
        long N,
        double P0,
        double P1,
        double S,
        double X,
        bool Passed);

    public readonly record struct AsLfsrMonobitCollectionResult(
        string CsvPath,
        int PassedCount,
        IReadOnlyList<AsLfsrMonobitRunResult> Runs);

    public readonly record struct AsLfsrMaxOnesRunResult(
        int RunIndex,
        string Seed,
        int M,
        bool Passed);

    public readonly record struct AsLfsrMaxOnesCollectionResult(
        string CsvPath,
        int PassedCount,
        double PassedShare,
        IReadOnlyList<AsLfsrMaxOnesRunResult> Runs);

    public static AsLfsrMonobitCollectionResult CollectAndPrintAsLfsrMonobitStats(
        int seed,
        int seedCount = DefaultSeedCount,
        int outputsPerSeed = DefaultOutputsPerSeed,
        string csvFileName = DefaultCsvFileName)
    {
        if (seedCount <= 0)
            throw new ArgumentException("seedCount must be positive", nameof(seedCount));

        if (outputsPerSeed <= 0)
            throw new ArgumentException("outputsPerSeed must be positive", nameof(outputsPerSeed));

        if (string.IsNullOrWhiteSpace(csvFileName))
            throw new ArgumentException("csvFileName must not be empty", nameof(csvFileName));

        var random = new Random(seed);
        var runs = new List<AsLfsrMonobitRunResult>(seedCount);

        Console.WriteLine($"Collecting AsLfsr monobit stats: seed={seed}, seeds={seedCount}, outputsPerSeed={outputsPerSeed}, csv='{csvFileName}', time={DateTime.UtcNow:O}");

        for (var runIndex = 1; runIndex <= seedCount; runIndex++)
        {
            var seedStates = TestUtils.GenerateRandomAsLfsrStates(random);
            runs.Add(RunAsLfsrMonobitTest(seedStates, DefaultTSet, outputsPerSeed, runIndex));
        }

        var passedCount = runs.Count(r => r.Passed);
        var csvPath = Path.Combine(Environment.CurrentDirectory, csvFileName);
        SaveMonobitCsv(csvPath, runs);

        Console.WriteLine($"Seeds passed monobit test (x < 3): {passedCount}/{seedCount}");

        return new AsLfsrMonobitCollectionResult(csvPath, passedCount, runs);
    }

    public static AsLfsrMaxOnesCollectionResult CollectAndPrintAsLfsrMaxOnesStats(
        int seed,
        int seedCount = DefaultSeedCount,
        int outputsPerSeed = DefaultOutputsPerSeed,
        string csvFileName = DefaultMaxOnesCsvFileName)
    {
        if (seedCount <= 0)
            throw new ArgumentException("seedCount must be positive", nameof(seedCount));

        if (outputsPerSeed <= 0)
            throw new ArgumentException("outputsPerSeed must be positive", nameof(outputsPerSeed));

        if (string.IsNullOrWhiteSpace(csvFileName))
            throw new ArgumentException("csvFileName must not be empty", nameof(csvFileName));

        var random = new Random(seed);
        var runs = new List<AsLfsrMaxOnesRunResult>(seedCount);

        Console.WriteLine($"Collecting AsLfsr max ones stats: seed={seed}, seeds={seedCount}, outputsPerSeed={outputsPerSeed}, acceptedRange=[{MinAcceptedMaxOnes}, {MaxAcceptedMaxOnes}], csv='{csvFileName}', time={DateTime.UtcNow:O}");

        for (var runIndex = 1; runIndex <= seedCount; runIndex++)
        {
            var seedStates = TestUtils.GenerateRandomAsLfsrStates(random);
            runs.Add(RunAsLfsrMaxOnesTest(seedStates, DefaultTSet, outputsPerSeed, runIndex));
        }

        var passedCount = runs.Count(r => r.Passed);
        var passedShare = (double)passedCount / seedCount;
        var csvPath = Path.Combine(Environment.CurrentDirectory, csvFileName);
        SaveMaxOnesCsv(csvPath, runs);

        Console.WriteLine($"Seeds passed max ones test (m in [{MinAcceptedMaxOnes}, {MaxAcceptedMaxOnes}]): {passedShare:F6} ({passedCount}/{seedCount})");

        return new AsLfsrMaxOnesCollectionResult(csvPath, passedCount, passedShare, runs);
    }

    public static AsLfsrMonobitRunResult RunAsLfsrMonobitTest(
        ulong[] initialStates,
        ulong[] TSet,
        int outputsPerSeed = DefaultOutputsPerSeed,
        int runIndex = 1)
    {
        if (initialStates is null)
            throw new ArgumentNullException(nameof(initialStates));

        if (TSet is null)
            throw new ArgumentNullException(nameof(TSet));

        if (initialStates.Length != StateCount)
            throw new ArgumentException($"AsLfsr requires {StateCount} state values", nameof(initialStates));

        if (outputsPerSeed <= 0)
            throw new ArgumentException("outputsPerSeed must be positive", nameof(outputsPerSeed));

        if (TSet.Length != StateCount)
            throw new ArgumentException($"AsLfsr requires {StateCount} tap values", nameof(TSet));

        var state = initialStates.Select(s => s & StateMask).ToArray();
        long n1 = 0;

        for (var i = 0; i < outputsPerSeed; i++)
        {
            var result = AsLfsr.GetNext(state, TSet);
            state = result.States;
            n1 += BitOperations.PopCount(result.Stream & StateMask);
        }

        var n = (long)outputsPerSeed * BitsPerOutput;
        var n0 = n - n1;
        var p1 = (double)n1 / n;
        var p0 = (double)n0 / n;
        var s = Math.Sqrt(n * p1 * p0) / n;
        var x = s > 0 ? Math.Abs(p0 - p1) / s : double.PositiveInfinity;
        var passed = x < 3.0;

        return new AsLfsrMonobitRunResult(
            runIndex,
            FormatSeed(initialStates),
            n0,
            n1,
            n,
            p0,
            p1,
            s,
            x,
            passed);
    }

    public static AsLfsrMaxOnesRunResult RunAsLfsrMaxOnesTest(
        ulong[] initialStates,
        ulong[] TSet,
        int outputsPerSeed = DefaultOutputsPerSeed,
        int runIndex = 1)
    {
        if (initialStates is null)
            throw new ArgumentNullException(nameof(initialStates));

        if (TSet is null)
            throw new ArgumentNullException(nameof(TSet));

        if (initialStates.Length != StateCount)
            throw new ArgumentException($"AsLfsr requires {StateCount} state values", nameof(initialStates));

        if (outputsPerSeed <= 0)
            throw new ArgumentException("outputsPerSeed must be positive", nameof(outputsPerSeed));

        if (TSet.Length != StateCount)
            throw new ArgumentException($"AsLfsr requires {StateCount} tap values", nameof(TSet));

        var state = initialStates.Select(s => s & StateMask).ToArray();
        var currentRun = 0;
        var maxRun = 0;

        for (var i = 0; i < outputsPerSeed; i++)
        {
            var result = AsLfsr.GetNext(state, TSet);
            state = result.States;
            maxRun = Math.Max(maxRun, GetMaxOnesRunLength(result.Stream & StateMask, ref currentRun));
        }

        var passed = maxRun >= MinAcceptedMaxOnes && maxRun <= MaxAcceptedMaxOnes;

        return new AsLfsrMaxOnesRunResult(
            runIndex,
            FormatSeed(initialStates),
            maxRun,
            passed);
    }

    private static string FormatSeed(IEnumerable<ulong> states) =>
        string.Join("_", states.Select(state => (state & StateMask).ToBlock()));

    private static int GetMaxOnesRunLength(ulong stream, ref int currentRun)
    {
        var maxRun = currentRun;

        for (var bitIndex = BitsPerOutput - 1; bitIndex >= 0; bitIndex--)
        {
            if (((stream >> bitIndex) & 1UL) == 1UL)
            {
                currentRun++;
                if (currentRun > maxRun)
                    maxRun = currentRun;
            }
            else
            {
                currentRun = 0;
            }
        }

        return maxRun;
    }

    private static void SaveMonobitCsv(string path, IReadOnlyList<AsLfsrMonobitRunResult> runs)
    {
        try
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            sw.WriteLine("runIndex;seed;n0;n1;n;p0;p1;s;x;passed");

            foreach (var run in runs)
            {
                sw.Write(run.RunIndex.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.Seed);
                sw.Write(';');
                sw.Write(run.N0.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.N1.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.N.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.P0.ToString("F10", CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.P1.ToString("F10", CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.S.ToString("F10", CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.X.ToString("F10", CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.WriteLine(run.Passed ? "true" : "false");
            }

            Console.WriteLine($"Saved monobit CSV: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save monobit CSV '{path}': {ex.Message}");
        }
    }

    private static void SaveMaxOnesCsv(string path, IReadOnlyList<AsLfsrMaxOnesRunResult> runs)
    {
        try
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            sw.WriteLine("runIndex;seed;m;passed");

            foreach (var run in runs)
            {
                sw.Write(run.RunIndex.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.Write(run.Seed);
                sw.Write(';');
                sw.Write(run.M.ToString(CultureInfo.InvariantCulture));
                sw.Write(';');
                sw.WriteLine(run.Passed ? "true" : "false");
            }

            Console.WriteLine($"Saved max ones CSV: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save max ones CSV '{path}': {ex.Message}");
        }
    }
}