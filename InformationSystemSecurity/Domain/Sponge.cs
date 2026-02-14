using InformationSystemSecurity.domain.Enums;

namespace InformationSystemSecurity.domain;

public class Sponge(ICipher cipher)
{
    private const int BlockSize = 4;

    private readonly CBlockCipher _blockCipher = new CBlockCipher(cipher);
    
    public string GetHash(string message)
    {
        var state = Enumerable.Repeat(
            Enumerable.Repeat("____", 5).ToArray(), 5)
            .ToArray();
        
        // todo ...
        for ...
            state = Absorb(state, block);
        for ...
            (state, block) = Squeeze(state);
    }
    
    private string[][] Absorb(string[][] state, string block)
    {
        if (block.Length != BlockSize)
            throw new ArgumentException($"Block must be {BlockSize} characters long.");
        
        // todo ...

        state[0][0] = _blockCipher.Encrypt([string1, string2], CompressMode.Out4);
        return PermuteState(state);
    }
    
    private (string[][] ResultState, string ResultBlock) Squeeze(string[][] state)
    {
        state = PermuteState(state);
        // todo ...
        
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
    
    private static string[][] MixColumns(string[][] state)
    {
        throw new NotImplementedException();
    }

    private static string[][] ShiftRows(string[][] state)
    {
        throw new NotImplementedException();
    }
    
    private static string[][] ShatterBlocks(string[][] state)
    {
        throw new NotImplementedException();
    }
    
    private static string[][] ShiftBlocks(string[][] state)
    {
        throw new NotImplementedException();
    }
    
}