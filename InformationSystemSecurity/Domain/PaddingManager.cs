using InformationSystemSecurity.domain.Models;
using InformationSystemSecurity.Domain.Utils;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace InformationSystemSecurity.domain;

public static class PaddingManager
{
    private const int BlockLength = 80;
    private const int PaddingLengthInfoLength = 7;
    private const int BlocksCountInfoLength = 10;
    
    private const int MinPaddingLength = 23;
    private const int MaxPaddingLength = 103;



    // стр 36
    public static string PadMessage(string message)
    {
        //todo (используем константы
        // например, 57 = BlockLength - MinPaddingLength и т.д.)

        var bins = message.ToBinary();
        var blocksCount = bins.Length / BlockLength;
        
        var flag = true;
        if (bins.Length % BlockLength == 0)
            flag = GetPaddingInfo(bins) != null;

        if (flag)
        {
            var pad = ProducePadding(bins.Length % BlockLength, blocksCount);
            bins = [.. bins, .. pad];
        }

        return bins.ToTextMessage();
    }

    private static byte[] ProducePadding(int remainder, int numberBlocks)
    {
        int totalBlocks;
        int paddingBits;
        if (remainder == 0)
        {
            totalBlocks = numberBlocks + 1;
            paddingBits = BlockLength;
        }
        else if (remainder <= (BlockLength - MinPaddingLength))
        {
            paddingBits = BlockLength - remainder;
            totalBlocks = numberBlocks + 1;
        }
        else
        {
            totalBlocks = numberBlocks + 2;
            paddingBits = BlockLength * 2 - remainder;
        }

        var padding = new byte[paddingBits];

        padding[0] = 1;

        for (int i = 1; i <= paddingBits - 21; i++)
        {
            padding[i] = 0;
        }

        int tempR = paddingBits;
        for (int i = 6; i >= 0; i--)
        {
            padding[paddingBits - 20 + i] = (byte)(tempR & 1);
            tempR >>= 1;
        }

        int tempB = totalBlocks;
        for (int i = 9; i >= 0; i--)
        {
            padding[paddingBits - 20 + PaddingLengthInfoLength + i] = (byte)(tempB & 1);
            tempB >>= 1;
        }

        padding[paddingBits - 3] = 0;
        padding[paddingBits - 2] = 0;
        padding[paddingBits - 1] = 1;

        return padding;
    }

    public static string UnpadMessage(string message)
    {
        var bins = message.ToBinary();
        var t = GetPaddingInfo(bins);

        if (t == null)
            return message;

        var tmp = bins[0..^t.PaddingLength];

        return tmp.ToTextMessage();
    }
    
    // см. check_padding (стр 35)
    private static PaddingInfo? GetPaddingInfo(byte[] message)
    {
        if (message.Length % BlockLength != 0 || message.Length == 0)
            return null;

        var blocksCount = message.Length / BlockLength;
        var tb = message[^20..];

        if (tb[^3] != 0 || tb[^2] != 0 || tb[^1] != 1)
            return null;

        var paddingLength = 0;
        for (var i = 0; i < PaddingLengthInfoLength; i++)
        {
            paddingLength = (paddingLength << 1) | tb[i];
        }

        var numberBlocks = 0;
        for (var i = PaddingLengthInfoLength; i < PaddingLengthInfoLength + BlocksCountInfoLength; i++) 
        {
            numberBlocks = (numberBlocks << 1) | tb[i];
        }

        if (numberBlocks != blocksCount || paddingLength < MinPaddingLength || paddingLength >= MaxPaddingLength)
            return null;

        tb = message[^paddingLength..^21];

        if (tb[0] == 0) return null;

        for (var i = 1; i <= paddingLength - 22; i++)
            if (tb[i] == 1) return null;

        return new PaddingInfo(numberBlocks, paddingLength);
    }
}