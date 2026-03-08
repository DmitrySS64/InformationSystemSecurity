using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;
using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.Domain.Lsfr;

public class AsLfsrWithCBlock
{
    private ulong[][] _state; //4x3
    private string[] _stream = null!;
    private readonly ulong[][] _taps; //4x3 См. "SET"
    private readonly CBlockCipher _cBlock;
    
    // Инициализация вынесена в консутруктор
    public AsLfsrWithCBlock(string seed, ulong[][] taps)
    {
        if (seed.Length != 16)
            throw new ArgumentException("Seed must be 16 characters long");
        
        _taps = taps;
        _cBlock = new CBlockCipher(new Caesar());
        _state = InitState(seed);
    }
    
    public AsLfsrWithCBlockResult GetNext()
    {
        // todo ...
        ... var t = AsLfsr.GetNext(_state[i], _taps[i]);
        ...
            
        _stream = ...
        _state = ...
        return new AsLfsrWithCBlockResult(_state, _stream);
    }

    // см. initialize_PRNG в "общее"
    private ulong[][] InitState(string seed)
    {
        // todo ...
        var secret = _cBlock.Encrypt(value, CompressMode.Out16);
        ...
        // дальше часть идёт из C_AS_LSFR_next (стр. 54), так как нет смысла разбивать это на 2 функции,
        // тут одна строка добавляется (которая state <- seed2bins)
        
        var state = // преобразовать части полученных строк в бинарную форму,
                    // можно через select + block2bins (Converter.ToNum(this string block))
        return state;
    }
}