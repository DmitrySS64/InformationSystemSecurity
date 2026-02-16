using InformationSystemSecurity.domain.Enums;
using System.Text;

namespace InformationSystemSecurity.domain;

public class Sponge(ICipher cipher)
{
    private const int BlockSize = 4;

    private readonly CBlockCipher _blockCipher = new CBlockCipher(cipher);
    
    public string GetHash(string message)
    {
        var state = new string[5][];
        for (int i = 0; i < 5; i++)
            state[i] = Enumerable.Repeat("____", 5).ToArray();

        var @out = new StringBuilder();
        var K = 4 - message.Length % 4;

        if (K < 4)
        {
            for (var k = 0; k < K; k++)
            {
                message = string.Concat(message, '_');
            }
        }

        var M = message.Length / 4;

        for (var i = 0; i < M; i++)
        {
            var block = message.Substring(i * 4, 4);
            state = Absorb(state, block);
        }
        for (var i = 0; i < 16; i++)
        {
            (state, var block) = Squeeze(state);
            @out = @out.Append(block);
        }

        return @out.ToString();
    }
    
    public string[][] Absorb(string[][] state, string block)
    {
        if (block.Length != BlockSize)
            throw new ArgumentException($"Block must be {BlockSize} characters long.");

        var string2 = string.Concat(block, state[0][0], block, state[0][0]);

        var X = new string[5];

        for (int i = 0; i < 5; i++)
        {
            X[i] = "____";
            for (int j = 0; j < 5; j++)
            {
                X[i] = Alphabet.AddTexts(X[i], state[i][j]);
            }
        }
        var string1 = string.Concat(X[0], X[1], X[2], X[3]);
        
        state[0][0] = _blockCipher.Encrypt([string1, string2], CompressMode.Out4);

        return PermuteState(state);
    }
    
    public (string[][] ResultState, string ResultBlock) Squeeze(string[][] state)
    {
        state = PermuteState(state);

        var X = new string[5];
        for (var i = 0; i < 5; i++)
        {
            X[i] = "____";
            for (var j = 0; j < 5; j++)
            {
                X[i] = Alphabet.AddTexts(X[i], state[i][j]);
            }
        }

        var @string = string.Concat(X[0], X[1], X[2], X[3]);
        var block = _blockCipher.Encrypt([@string], CompressMode.Out4);
        return (state, block);
    }
    
    private static string[][] PermuteState(string[][] state)
    {
        state = MixColumns(state);
        state = ShatterBlocks(state);
        state = ShiftRows(state);
        return state;
    }
    
    public static string[][] MixColumns(string[][] state)
    {
        for (var i = 0; i < 5; i++)
        {
            var X = "____";

            for (var j = 0; j < 5; j++)
            {
                X = Alphabet.AddTexts(X, state[j][i]);
            }

            var q = (i + 1) % 5;
            for (var j = 0; j < 5; j++)
            {
                var tmp = Alphabet.AddTexts(X, state[j][q]);
                state[j][q] = Alphabet.SubtractTexts(tmp, state[j][i]);
            }
        }
        return state;
    }

    public static string[][] ShiftRows(string[][] state)
    {
        if (state == null || state.Length != 5 || state.Any(row => row?.Length != 5))
            throw new ArgumentException("State must be a 5x5 matrix");

        var result = new string[5][];
        for (var i = 0; i < 5; i++)
            result[i] = new string[5];


        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < 5; j++)
            {
                var shiftedRow = (j + i) % 5;
                result[j][i] = state[shiftedRow][i];
            }
        }

        return result;
    }
    
    public static string[][] ShatterBlocks(string[][] state)
    {
        for (var i = 0; i < 5; i++)
        {
            state[i][i] = ShiftBlock(state[i][i]);
        }
        return state;
    }
    
    private static string ShiftBlock(string block)
    {
        var numbers = Alphabet.ToNumArray(block);
        var shifted = new int[4];

        for (var i = 0; i < 4; i++)
        {
            var newPos = (i + 1) % 4;
            shifted[newPos] = numbers[i];
        }

        return Alphabet.ToText(shifted);
    }
}