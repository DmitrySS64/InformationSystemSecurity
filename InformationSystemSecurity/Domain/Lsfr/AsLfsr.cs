using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Lsfr;

public class AsLfsr
{
    public static AsLfsrResult GetNext(ulong[] states, ulong[] taps)
    {
        var outputBit = 0uL;

        for (var i = 0; i < 20; i++)
        {
            var tmp = Push(states, taps);

            states = tmp.States;
            outputBit.PushBit((byte)tmp.Stream);
        }

        return new AsLfsrResult
        {
            States = states,
            Stream = outputBit,
        };
    }
    
    public static AsLfsrResult Push(ulong[] states, ulong[] taps)
    {
        if (states.Length != 3 || taps.Length != 3)
            throw new ArgumentException("Invalid number of state or taps array");
        
        var lsfrSet = new ulong[states.Length];
        for (var i = 0; i < states.Length; i++)
            lsfrSet[i] = Lfsr.Push(states[i], taps[i]);

        var lsfr0Bit = (lsfrSet[0] & 1) == 1;

        ulong stream;
        if (!lsfr0Bit)
            stream = lsfrSet[1] & 1;
        else
            stream = lsfrSet[2] & 1;

        return new AsLfsrResult
        {
            States = lsfrSet,
            Stream = stream, //<-бит
        };
    }
}