using System.Text;

namespace InformationSystemSecurity.domain;

public class SBlockCipher
{
    private const int KeySize = 16;
    
    private readonly int[] _shiftVector = [1, -1, 1, 2, -2, 1, 1, 3, -1, 2];
    
    private readonly ICipher _cipher;
    private readonly string _key;
    private readonly bool _roundKey;
    private readonly bool _merge;

    public SBlockCipher(ICipher cipher, string key, bool roundKey = false, bool merge = false)
    {
        _cipher = cipher;
        if (key.Length != KeySize)
            throw new ArgumentException($"Key must be {KeySize} characters long.");
        
        _key = key;
        _roundKey = roundKey;
        _merge = merge;
    }

    public string Encrypt(string text)
    {
        ValidateInput(text);
        
        var result = new StringBuilder();

        for (var i = 0; i < text.Length; i += TextConverter.BlockSize)
        {
            var block = text.Substring(i, TextConverter.BlockSize);
            
            if (_merge)
                block = MergeBlock(block, _key); // Первый мёрдж
            
            var keyForEncrypting = _roundKey
                ? GenerateRoundKey()
                : _key;
            
            var encryptedBlock = _cipher.Encrypt(block, keyForEncrypting);
            
            if (_merge)
                encryptedBlock = MergeBlock(encryptedBlock, _key); // Второй мёрдж после щифрования
            
            result.Append(encryptedBlock);
        }

        return result.ToString();
    }

    public string Decrypt(string text)
    {
        ValidateInput(text);
        
        var result = new StringBuilder();

        for (var i = 0; i < text.Length; i += TextConverter.BlockSize)
        {
            var block = text.Substring(i, TextConverter.BlockSize);
            
            if (_merge)
                block = MergeBlock(block, _key, reverse: true); // Первый мёрдж
            
            var keyForDecrypting = _roundKey
                ? GenerateRoundKey()
                : _key;
            
            var decryptedBlock = _cipher.Decrypt(block, keyForDecrypting);
            
            if (_merge)
                decryptedBlock = MergeBlock(decryptedBlock, _key, reverse: true); // Второй мёрдж после щифрования
            
            result.Append(decryptedBlock);
        }

        return result.ToString();
    }
    
    public static string MergeBlock(string blockIn, string key, bool reverse = false)
    {
        if (blockIn.Length != TextConverter.BlockSize)
            throw new ArgumentException("input_error");
        
        int[] m = [0, 1, 2, 3];
        
        var keyArray = key.ToNumArray();
  
        var sum = 0; 
        for (var i = 0; i < 16; i++) // Свёртка ключа в одно число
        {
            var sign = i % 2 == 0 
                ? 1 
                : -1;
            sum = (24 + sum + sign * keyArray[i]) % 24;
        }
        
        for (var k = 0; k < 3; k++) // Перемешивание массива m в зависимости ключа
        {
            var denom = 4 - k;
            var t = ((sum % denom) + denom) % denom; // нормализация отрицательных значений
            sum = (sum - t) / denom;
            
            (m[k], m[k + t]) = (m[k + t], m[k]);
        }
        
        var result = blockIn.ToNumArray();

        // Проход по массиву m в прямом или обратном порядке
        var start = reverse ? 3 : 0;
        var end = reverse ? -1 : 4;
        var step = reverse ? -1 : 1;

        for (var j = start; j != end; j += step)
        {
            var a = m[j & 3];
            var b = m[(j + 1) & 3];
            result[b] = reverse
                ? (32 + result[b] - result[a]) % 32
                : (result[b] + result[a]) % 32;
        }
        
        return result.ToText();
    }
    
    private string GenerateRoundKey()
    {
        if (_key.Length != KeySize)
            throw new ArgumentException("input_error");
        
        var keyTmp = "";
        var keyExit = _key + _key; // удвоенный ключ

        for (var i = 0; i < TextConverter.BlockSize * 2; i++)
        {
            var slice = keyExit.Substring(i * 2, 4);

            var aTmp = new StringBuilder();
            // Преобразуем в числовой массив
            var bTmp = slice.ToNumArray();

            // Генерация нового блока
            for (var k = 0; k < TextConverter.BlockSize; k++)
            {
                var x = (2 * i + k) % 10;
                var aTmpNum = (64 + k + _shiftVector[x] * bTmp[k]) % 32;
                aTmp.Append(aTmpNum.ToChar());
            }
            keyTmp = TextConverter.AddTexts(keyTmp, aTmp.ToString());
        }

        return keyTmp;
    }
    
    private static void ValidateInput(string text)
    {
        if (text.Length % TextConverter.BlockSize != 0)
            throw new ArgumentException($"Text length must be a multiple of {TextConverter.BlockSize} characters.");
    }
}