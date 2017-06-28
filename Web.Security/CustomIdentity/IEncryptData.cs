using System;

namespace Web.Security.CustomIdentity
{
    public interface IEncryptData
    {
        Tuple<string, string> Encrypt(string data);
        string Decrypt(string data, string biv);
        string WindowsEncrypted(string text);
        string WindowsDecrypted(string text);
    }
}