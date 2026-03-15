namespace InformationSystemSecurity.Tools;

public static class TestUtils
{
    private const int AsLfsrStateCount = 3;
    private const int AsLfsrBitsPerState = 20;

    public static ulong[] GenerateRandomAsLfsrStates(Random random)
    {
        ArgumentNullException.ThrowIfNull(random);

        var states = new ulong[AsLfsrStateCount];
        for (var i = 0; i < AsLfsrStateCount; i++)
        {
            states[i] = (ulong)random.NextInt64(1, 1L << AsLfsrBitsPerState);
        }

        return states;
    }
}

