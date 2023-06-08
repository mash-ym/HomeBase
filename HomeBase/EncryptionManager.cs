using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HomeBase
{
    public class EncryptionManager
    {
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("ThisIsMySalt");

        public string Encrypt(string data)
        {
            byte[] encryptedBytes;

            using (var passwordDerivation = new Rfc2898DeriveBytes(data, Salt))
            {
                byte[] key = passwordDerivation.GetBytes(32);
                byte[] iv = passwordDerivation.GetBytes(16);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                using (var streamWriter = new StreamWriter(cryptoStream))
                                {
                                    streamWriter.Write(data);
                                }

                                encryptedBytes = memoryStream.ToArray();
                            }
                        }
                    }
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            string decryptedData;

            using (var passwordDerivation = new Rfc2898DeriveBytes(encryptedData, Salt))
            {
                byte[] key = passwordDerivation.GetBytes(32);
                byte[] iv = passwordDerivation.GetBytes(16);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (var memoryStream = new MemoryStream(encryptedBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                using (var streamReader = new StreamReader(cryptoStream))
                                {
                                    decryptedData = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }

            return decryptedData;
        }

    }

}
