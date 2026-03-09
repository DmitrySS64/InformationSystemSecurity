using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Lsfr;

public class AsLfsr
{
    //Некорректный stream
    public static AsLfsrResult GetNext(ulong[] states, ulong[] taps)
    {
        ulong outputBit = 0;

        for (int i = 0; i < 20; i++)
        {
            var tmp = Push(states, taps);

            states = tmp.States;
            outputBit = (outputBit << 1) | tmp.Stream;
        }

        return new AsLfsrResult
        {
            States = states,
            Stream = outputBit,
        };
    }
    
    //Некорректный stream
    public static AsLfsrResult Push(ulong[] states, ulong[] taps)
    {
        if (states.Length != 3 || taps.Length != 3)
            throw new ArgumentException("Invalid number of state or taps array");
        
        var lsfrSet = new ulong[states.Length];
        for (var i = 0; i < states.Length; i++)
            lsfrSet[i] = Lfsr.Push(states[i], taps[i]);
   
        bool lsfr0Bit19 = ((lsfrSet[0] >> 19 ) & 1) == 1;

        ulong stream;
        if (lsfr0Bit19)
            stream = (lsfrSet[1] >> 19) & 1;
        else
            stream = (lsfrSet[2] >> 19) & 1;

        return new AsLfsrResult
        {
            States = lsfrSet,
            Stream = stream, //<-бит
        };
    }
}