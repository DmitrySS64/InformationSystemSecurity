namespace InformationSystemSecurity.domain;

public interface ICipher
{
    string Encrypt(string text, string key);
    string Decrypt(string text, string key);
}