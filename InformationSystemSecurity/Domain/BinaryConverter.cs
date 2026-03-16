using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace InformationSystemSecurity.domain;

public static class BinaryConverter
{
    public static ulong Shift(this ref ulong num, int shift, int bitLength = 0)
    {
        if (bitLength == 0 || bitLength >= 64)
        {
            // Если bitLength не указан, используем эффективную длину
            bitLength = 64 - BitOperations.LeadingZeroCount(num);
            if (bitLength == 0) return num;
        }

        shift = -shift;
        shift %= bitLength;
        if (shift < 0)
        {
            shift += bitLength;
        }
        if (shift == 0) return num;

        var mask = (bitLength == 64) ? ulong.MaxValue : (1uL << bitLength) - 1;

        num &= mask;
        num = ((num << shift) | (num >> (bitLength - shift))) & mask;
        return num;
    }

    public static BigInteger Shift(this ref BigInteger num, int shift, int bitLength = 80)
    {
        // Нормализуем сдвиг
        shift = -shift;
        shift %= bitLength;
        if (shift < 0)
            shift += bitLength;

        if (shift == 0) return num;

        // Создаем маску для эффективной длины
        BigInteger mask = (BigInteger.One << bitLength) - 1;

        num &= mask;
        // Циклический сдвиг
        num = ((num << shift) | (num >> (bitLength - shift))) & mask;
        return num;
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
        var blockCount = textA.Length / TextConverter.BlockSize;

        var result = new StringBuilder();
        for (var i = 0; i < blockCount; i++)
        {
            var blockA = textA.Substring(i * TextConverter.BlockSize, TextConverter.BlockSize);
            var blockB = textB.Substring(i * TextConverter.BlockSize, TextConverter.BlockSize);

            result.Append(BlockXor(blockA, blockB));
        }

        return result.ToString();
    }

    // см. subblocks_xor
    private static string BlockXor(string blockA, string blockB)
    {
        if (blockA.Length != TextConverter.BlockSize || blockB.Length != TextConverter.BlockSize)
            throw new ArgumentException($"Blocks must be {TextConverter.BlockSize} characters long.");

        ulong binBlockA = blockA.ToNum();
        ulong binBlockB = blockB.ToNum();

        var XOR = binBlockA ^ binBlockB;
        
        return XOR.ToBlock();
    }
}