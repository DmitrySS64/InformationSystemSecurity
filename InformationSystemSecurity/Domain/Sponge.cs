using InformationSystemSecurity.domain.Enums;
using System.Text;

namespace InformationSystemSecurity.domain;

public class Sponge(ICipher cipher)
{
    private readonly CBlockCipher _blockCipher = new CBlockCipher(cipher);
    
    //TODO: починить 
    public string GetHash(string message)
    {
        var state = new string[5][];
        for (var i = 0; i < 5; i++)
        {
            state[i] = new string[5];
            Array.Fill(state[i], "____");
        }

        var output = new StringBuilder();

        // Padding
        var K = 4 - (message.Length % 4);
        if (K < 4)
        {
            message += new string('_', K);
        }

        // Absorbing phase
        var M = message.Length / 4;
        for (var i = 0; i < M; i++)
        {
            var block = message.Substring(i * 4, 4);
            state = Absorb(state, block);
        }

        // Squeezing phase
        for (var i = 0; i < 16; i++)
        {
            (state, var block) = Squeeze(state);
            output.Append(block);
        }

        return output.ToString();
    }
    
    public string[][] Absorb(string[][] inState, string block)
    {
        if (block.Length != Converter.BlockSize)
            throw new ArgumentException($"Block must be {Converter.BlockSize} characters long.");

        var state = CopyState(inState);
        var string1 = string.Concat(block, state[0][0], block, state[0][0]);

        var X = new string[5];

        for (var i = 0; i < 5; i++)
        {
            X[i] = "____";
            for (var j = 0; j < 5; j++)
            {
                X[i] = Converter.AddTexts(X[i], state[i][j]);
            }
        }
        var string2 = string.Concat(X[0], X[1], X[2], X[3]);
        
        state[0][0] = _blockCipher.Encrypt([string2, string1], CompressMode.Out4);

        return PermuteState(state);
    }
    
    public (string[][] ResultState, string ResultBlock) Squeeze(string[][] inState)
    {
        var state = CopyState(inState);
        state = PermuteState(state);

        var X = new string[5];
        for (var i = 0; i < 5; i++)
        {
            X[i] = "____";
            for (var j = 0; j < 5; j++)
            {
                X[i] = Converter.AddTexts(X[i], state[i][j]);
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
                X = Converter.AddTexts(X, state[j][i]);
            
            var q = (i + 1) % 5;
            for (var j = 0; j < 5; j++)
            {
                var tmp = Converter.AddTexts(X, state[j][q]);
                state[j][q] = Converter.SubtractTexts(tmp, state[j][i]);
            }
        }
        return state;
    }

    public static string[][] ShiftRows(string[][] state)
    {
        if (state is not { Length: 5 } || state.Any(row => row.Length != 5))
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
            state[i][i] = ShiftBlock(state[i][i]);
        
        return state;
    }
    
    private static string ShiftBlock(string block)
    {
        var numbers = block.ToNumArray();
        var shifted = new int[4];

        for (var i = 0; i < 4; i++)
        {
            var newPos = (i + 1) % 4;
            shifted[newPos] = numbers[i];
        }

        return shifted.ToText();
    }

    private static string[][] CopyState(string[][] state)
    {
        var copy = new string[5][];
        for (var i = 0; i < 5; i++)
        {
            copy[i] = new string[5];
            for (var j = 0; j < 5; j++)
            {
                copy[i][j] = state[i][j];
            }
        }
        return copy;
    }
}