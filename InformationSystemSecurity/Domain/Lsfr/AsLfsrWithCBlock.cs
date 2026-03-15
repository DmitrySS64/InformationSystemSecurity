using InformationSystemSecurity.domain;
using InformationSystemSecurity.domain.Enums;
using InformationSystemSecurity.domain.Models;
using System.Text;

namespace InformationSystemSecurity.Domain.Lsfr;

public class AsLfsrWithCBlock
{
    private readonly ulong[][] _state; //4x3
    private string _stream = null!;
    private readonly ulong[][] _taps; //4x3
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
                    tmp = t.Stream;
                else
                    tmp ^= t.Stream;
            }

            _stream += tmp.ToBlock();
        }

        return new AsLfsrWithCBlockResult(_state, _stream);
    }

    // Todo: проверить (параметры задаются при создании через конструктор, taps = AsLfsrWithCBlock.GetDefaultTaps() )
    public string[] ProduceRoundKeys(int resultKeyCount)
    {
        var rawResults = new AsLfsrWithCBlockResult[resultKeyCount];
        for (var i = 0; i < resultKeyCount - 1; i++)
            rawResults[i] = GetNext();

        return rawResults.Select(x => x.Stream).ToArray();
    }
    
    private ulong[][] InitState(string seed)
    {
        var array = new[] 
        {
            "ПЕРВОЕ_АКТЕРСТВО",
            "ВТОРОЙ_ДАЛЬТОНИК",
            "ТРЕТЬЯ_САДОВНИЦА",
            "ЧЕТВЕРТЫЙ_ГОБЛИН"
        };

        var value = new string[4];
        for (var i = 0; i < 4; i++)
            value[i] = _cBlock.Encrypt([array[i], seed], CompressMode.Out16);

        var secret = _cBlock.Encrypt(value, CompressMode.Out16);
        var init = new string[4];

        for (var i = 0; i < 4; i++)
        {
            var tempBuilder = new StringBuilder();
            var currentValue = value[i];

            for (var j = 0; j < 4; j++)
            {
                currentValue = TextConverter.AddTexts(currentValue, array[i]);
                tempBuilder.Append(_cBlock.Encrypt([currentValue, secret], CompressMode.Out4));
                currentValue = TextConverter.AddTexts(currentValue, tempBuilder.ToString());
            }

            init[i] = tempBuilder.ToString().Substring(4, 12);
        }

        var state = new ulong[4][];
        for (var i = 0; i < 4; i++)
        {
            state[i] =
            [
                init[i].Substring(0, 4).ToNum(),
                init[i].Substring(4, 4).ToNum(),
                init[i].Substring(8, 4).ToNum()
            ];
        }

        return state;
    }

    //TODO?: см. produce_round_keys.Написано использовать апарметры из лр3, здесь их получаем,
    // хотя он как будто использует другие (make_lfsr_set), поэтому надо по тестам смотреть и,
    // если что, просто тут поменять константы
    internal static ulong[][] GetDefaultTaps()
    {
        var first = new[] { 19, 18 }.ToBinary();
        var second = new[] { 18, 7 }.ToBinary();

        return 
        [
            [first, second, new[] { 17, 3 }.ToBinary()],
            [first, second, new[] { 16, 14, 13, 11 }.ToBinary()],
            [first, second, new[] { 15, 13, 12, 10 }.ToBinary()],
            [first, second, new[] { 14, 5, 3, 1 }.ToBinary()]
        ];
    }
}