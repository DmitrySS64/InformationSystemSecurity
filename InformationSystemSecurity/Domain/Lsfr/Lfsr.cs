using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Lsfr;

public class Lfsr
{
    public static LfsrResult GetNext(ulong state, ulong taps)
    {
        // todo ...
        return new LfsrResult { State = ..., Stream = ... };
    }
    
    public static ulong Push(ulong state, ulong taps)
    {
        // Тут просто используем бинарную логику (ulong уже в бинарном виде)
        var temp = // todo 
        return state.PushBit(temp % 2);
    }
}