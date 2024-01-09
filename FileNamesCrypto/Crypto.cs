using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileNamesCrypto
{
    public class Crypto
    {
        private readonly string password;

        private byte[] key, iv;

        public Crypto(string password)
        {
            this.password = password;
            calculateKeyAndIV();
        }

        public string Encrypt(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            ICryptoTransform encryptor = getCryptoTransform(true);
            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                }

                return ConvertToString(memoryStream.ToArray());
            }
        }
                
        public string Decrypt(string encrypted)
        {
            byte[] encryptedBytes = ConvertFromString(encrypted);

            ICryptoTransform decryptor = getCryptoTransform(false);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        private ICryptoTransform getCryptoTransform(bool getEncryptor)
        {
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            if (getEncryptor)
                return aes.CreateEncryptor();
            else
                return aes.CreateDecryptor();
        }

        private void calculateKeyAndIV()
        {
            try
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000, HashAlgorithmName.SHA256);
                
                key = derivedBytes.GetBytes(32);
                iv = derivedBytes.GetBytes(16);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }

        private string ConvertToString(byte[] data)
        {
            string base64 = Convert.ToBase64String(data);
            string replaced = base64.Replace('/', '-');
            return replaced;
        }

        private byte[] ConvertFromString(string base64)
        {
            string replaced = base64.Replace('-', '/');
            byte[] data = Convert.FromBase64String(replaced);
            return data;
        }
    }
}
