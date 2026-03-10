using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;
using InformationSystemSecurity.domain.Models;
using System.Text;

namespace InformationSystemSecurity.Domain.Lsfr;

public class AsLfsrWithCBlock
{
    private ulong[][] _state; //4x3
    private string _stream = null!;
    private readonly ulong[][] _taps; //4x3 См. "SET"
    private readonly CBlockCipher _cBlock;
    
    // Инициализация вынесена в консутруктор
    public AsLfsrWithCBlock(string seed, ulong[][] taps)
    {
        if (seed.Length != 16)
            throw new ArgumentException("Seed must be 16 characters long");
        
        _taps = taps;
        _cBlock = new CBlockCipher(new Caesar(CaesarMode.Core));
        _state = InitState(seed);
    }


    public AsLfsrWithCBlockResult GetNext()
    {
        for (var j = 0; j < 4; j++)
        {
            var tmp = 0UL;

            for (var k = 0; k < 4; k++)
            {
                var t = AsLfsr.GetNext(_state[k], _taps[j]);

                _state[k] = t.States;
                if (k == 0)
                {
                    tmp = t.Stream;
                }
                else
                {
                    tmp ^= t.Stream;
                }
            }
            _stream += tmp.ToBlock();
        }

        return new AsLfsrWithCBlockResult(_state, _stream);
    }

    // см. initialize_PRNG в "общее"
    private ulong[][] InitState(string seed)
    {
        var array = new string[4] {
            "ПЕРВОЕ_АКТЕРСТВО",
            "ВТОРОЙ_ДАЛЬТОНИК",
            "ТРЕТЬЯ_САДОВНИЦА",
            "ЧЕТВЕРТЫЙ_ГОБЛИН"
        };

        var value = new string[4];
        for (int i = 0; i < 4; i++)
        {
            value[i] = _cBlock.Encrypt([array[i], seed], CompressMode.Out16);
        }

        var secret = _cBlock.Encrypt(value, CompressMode.Out16);
        var init = new string[4];

        for (int i = 0; i < 4; i++)
        {
            var tmp = value[i];
            var TMP = new StringBuilder();

            for (int j = 0; j < 4; j++)
            {
                tmp = Converter.AddTexts(tmp, array[i]);
                TMP.Append(_cBlock.Encrypt([tmp, secret], CompressMode.Out4));
                tmp = Converter.AddTexts(tmp, TMP.ToString());
            }

            init[i] = TMP.ToString().Substring(4, 12);
        }

        // дальше часть идёт из C_AS_LSFR_next (стр. 54), так как нет смысла разбивать это на 2 функции,
        // тут одна строка добавляется (которая state <- seed2bins)

        var state = new ulong[4][];
                    // преобразовать части полученных строк в бинарную форму,
                    // можно через select + block2bins (Converter.ToNum(this string block))
        for (int i = 0; i < 4; i++)
        {
            var block1 = init[i].Substring(0, 4);
            var block2 = init[i].Substring(4, 4);
            var block3 = init[i].Substring(8, 4);

            state[i] = [
                block1.ToNum(),
                block2.ToNum(),
                block3.ToNum()
            ];
        }

        return state;
    }


    public static string[] InitPRNG(string seed)
    {
        var cBlock = new CBlockCipher(new Caesar(CaesarMode.Core));
        var array = new string[4] {
            "ПЕРВОЕ_АКТЕРСТВО",
            "ВТОРОЙ_ДАЛЬТОНИК",
            "ТРЕТЬЯ_САДОВНИЦА",
            "ЧЕТВЕРТЫЙ_ГОБЛИН"
        };

        var value = new string[4];
        for (int i = 0; i < 4; i++)
        {
            value[i] = cBlock.Encrypt([array[i], seed], CompressMode.Out16);
        }

        var secret = cBlock.Encrypt(value, CompressMode.Out16);
        var init = new string[4];

        for (int i = 0; i < 4; i++)
        {
            var tmp = value[i];
            var TMP = new StringBuilder();

            for (int j = 0; j < 4; j++)
            {
                tmp = Converter.AddTexts(tmp, array[i]);
                TMP.Append(cBlock.Encrypt([tmp, secret], CompressMode.Out4));
                tmp = Converter.AddTexts(tmp, TMP.ToString());
            }

            init[i] = TMP.ToString().Substring(4, 12);
        }

        return init;
    }
}