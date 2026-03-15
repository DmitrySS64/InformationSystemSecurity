using System.Text.RegularExpressions;

namespace InformationSystemSecurity.domain;

public static class BinaryConverter
{
    public static ulong Shift(this ref ulong num, int shift)
    {
        // TODO: см. binary_shift в ТГ
    }
    
    public static ulong PushBitToLeft(this ref ulong num, byte bit)
    {
        num = (num << 1) | bit;
        num &= 0xFFFFFUL;
        return num;
    }
    
    public static ulong ToBinary(this int[] tapPositions)
    {
        ulong result = 0;
        foreach (var position in tapPositions) 
        {
            var pos = position - 1;
            if (pos is >= 0 and < 64) 
                result |= 1UL << pos;
        }

        return result;
    }

    public static string ToBinaryString(this ulong value)
    {
        var binary = Convert.ToString((long)value, 2);
        var padding = (4 - binary.Length % 4) % 4;
        binary = binary.PadLeft(binary.Length + padding, '0');

        return "0b" + Regex.Replace(binary, ".{4}", "$0_").TrimEnd('_');
    }

    // см. block_xor (на самом деле, приходят не блоками)
    public static string TextXor(string textA, string textB)
    {
        // TODO: ... use BlockXor
    }

    // см. subblocks_xor
    private static string BlockXor(string blockA, string blockB)
    {
        if (blockA.Length != TextConverter.BlockSize || blockB.Length != TextConverter.BlockSize)
            throw new ArgumentException($"Blocks must be {TextConverter.BlockSize} characters long.");

        // TODO: как и в предыдщей лр, не нужны никакие dec2bin, просто работаем с бинарными
    }
}