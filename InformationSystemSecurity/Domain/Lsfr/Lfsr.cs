using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Models;
using System.Numerics;

namespace InformationSystemSecurity.Domain.Lsfr;

public class Lfsr
{
    //Сломан Stream
    public static LfsrResult GetNext(ulong state, ulong taps)
    {
        ulong stream = 0;
        for (int i = 0; i < 20; i++)
        {
            state = Push(state, taps);
            var bit = (state >> 19) & 1UL;
            stream |= bit << i;
            //stream = (stream << 1) | bit;
        }
        return new LfsrResult { State = state, Stream = stream };
    }
    
    public static ulong Push(ulong state, ulong taps)
    {
        var tmp = state & taps;

        var feedback = (byte)(BitOperations.PopCount(tmp) % 2);
        
        state = state.PushBit(feedback);

        return state;
    }
}