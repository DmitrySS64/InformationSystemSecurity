namespace InformationSystemSecurity.domain;

public class PBlockCipher 
{
    public static string Encrypt(string text, int roundNumber)
    {
        if (text.Length != 16)
            throw new ArgumentException("Input text must be 16 characters long.");
        
        //TODO: исп. MagicSquare.GetDefaultSet()[roundNumber % 3]
        // LB2B и B2LB, думаю, не нужны, так как мы можем работать с бинарными, там пара строк выйдет
    }
    
    public static string Decrypt(string text, int roundNumber)
    {
        if (text.Length != 16)
            throw new ArgumentException("Input text must be 16 characters long.");
        
        //TODO
    }
}