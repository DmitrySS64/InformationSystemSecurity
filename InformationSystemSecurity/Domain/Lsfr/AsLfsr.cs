using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Lsfr;

public class AsLfsr
{
    public static AsLfsrResult GetNext(ulong[] states, ulong[] taps)
    {
        // todo ...
        var outputBit = ...
        return new AsLfsrResult
        {
            States = ...,
            Stream = outputBit,
        };
    }
    
    private static LfsrResult Push(ulong[] states, ulong[] taps)
    {
        if (states.Length != 3 || taps.Length != 3)
            throw new ArgumentException("Invalid number of state or taps array");
        
        var lsfrSet = new ulong[states.Length];
        for (var i = 0; i < states.Length; i++)
            lsfrSet[i] = Lfsr.Push(states[i], taps[i]);
   
        // todo ....
        return new AsLfsrResult
        {
            States = ...,
            Stream = ...,
        };
    }
}