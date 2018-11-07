using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Help
{
    public static class LoginProtector
    {
        private static readonly byte[] salt = Encoding.Unicode.GetBytes("Sklefjslf3");
        private static readonly int iterations = 2000;

        public static string Encrypt(string plainText, string pass)
        {
            byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, iterations);
            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);

            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decript(string plianText, string pass)
        {
            byte[] cryptoBytes = Convert.FromBase64String(plianText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, iterations);
            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);

            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cryptoBytes, 0, cryptoBytes.Length);
            }
            return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
}
