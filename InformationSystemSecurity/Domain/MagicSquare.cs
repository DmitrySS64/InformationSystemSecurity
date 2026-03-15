namespace InformationSystemSecurity.domain;

public static class MagicSquare
{
    public static readonly int[][] Default1 =
    [
        [16, 3, 2, 13],
        [5, 10, 11, 8],
        [9, 6, 7, 12],
        [4, 15, 14, 1]
    ];

    public static readonly int[][] Default2 =
    [
        [7, 14, 4, 9],
        [12, 1, 15, 6],
        [13, 8, 10, 3],
        [2, 11, 5, 16]
    ];

    public static readonly int[][] Default3 =
    [
        [4, 14, 15, 1],
        [9, 7, 6, 12],
        [5, 11, 10, 8],
        [16, 2, 3, 13]
    ];

    public static int[][][] GetDefaultSet() => [Default1, Default2, Default3];

    public static string Encrypt(string block, int[][] square)
    {
        // TODO
    }

    public static string Decrypt(string block, int[][] square)
    {
        // TODO
    }
}