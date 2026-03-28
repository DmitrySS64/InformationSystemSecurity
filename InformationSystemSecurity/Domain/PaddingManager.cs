using InformationSystemSecurity.domain.Models;

namespace InformationSystemSecurity.domain;

public static class PaddingManager
{
    // todo: ! использовать значения ниже, а не магические числа ! 
    private const int BlockLength = 80;
    private const int PaddingLengthInfoLength = 7;
    private const int BlocksCountInfoLength = 10;
    
    private const int MinPaddingLength = 23;
    private const int MaxPaddingLength = 103;
    
    private const string PaddingStart = "100"; // для сравнения можно кастить к char[]
    private const string PaddingEnd = "001";
    
    
    // см. pad_message + produce_padding, produce_padding не переиспользуется
    // нет смысла в двух методах. Просто если не нужен паддинг, то не накладываем
    public static string PadMessage(string message)
    {
        //todo (используем константы
        // например, 57 = BlockLength - MinPaddingLength и т.д.)
    }

    public static string UnpadMessage(string message)
    {
        //todo
    }
    
    // см. check_padding
    private static PaddingInfo? GetPaddingInfo(byte[] message)
    {
        if (message.Length % BlockLength != 0)
            return null;

        // Дальше в коде по аналогии - если навушается условие паддинга, возвращаем null
        // TODO (должно быть намного короче, чем в маткад, по сути нужно спарсить правильно строку)
        
        return new PaddingInfo(blocksCount, paddingLength);
    }
}