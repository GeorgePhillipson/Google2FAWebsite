using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Effortless.Net.Encryption;

namespace Web.Security.CustomIdentity
{
    public class EncryptData : IEncryptData
    {
        private string EncryptionKey()
        {
            var fileExists = File.Exists(ConfigurationManager.AppSettings["EncryptKey"]);

            if (!fileExists) throw new FileNotFoundException("FileNotFound");

            string encryptionKey;
            using (StreamReader reader = new StreamReader(ConfigurationManager.AppSettings["EncryptKey"]))
            {
                encryptionKey = reader.ReadLine();
            }

            if (string.IsNullOrEmpty(encryptionKey))
            {
                throw new NullReferenceException("TxtFileEmpty");
            }

            return encryptionKey;
        }
        public Tuple<string, string> Encrypt(string data)
        {
            byte[] iv = Bytes.GenerateIV();
            string encryptedData = Strings.Encrypt(data, Convert.FromBase64String(EncryptionKey()), iv);
            return new Tuple<string, string>(encryptedData, Convert.ToBase64String(iv));
        }

        public string Decrypt(string data, string biv)
        {

            byte[] key = Convert.FromBase64String(EncryptionKey());
            byte[] iv = Convert.FromBase64String(biv);
            string decrypted = Strings.Decrypt(data, key, iv);
            return decrypted;
        }

        //READ https://msdn.microsoft.com/en-us/library/ms995355.aspx for using this type of encryption 
        public string WindowsEncrypted(string text)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(text),null,DataProtectionScope.LocalMachine));
        }

        public string WindowsDecrypted(string text)
        {
            return Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(text),null,DataProtectionScope.LocalMachine));
        }
    }
}
