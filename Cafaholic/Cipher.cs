using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cafaholic
{
    public static class Cipher
    {
        private const string EncryptionKey = "D34uddwehg34354jrjefh45h454g@&jhwej";

        private static byte[] GetToken()
        {
            return Encoding.UTF8.GetBytes(EncryptionKey);
        }

        public static byte[] Encrypt(string text)
        {
            return ProtectedData.Protect(Encoding.UTF8.GetBytes(text), GetToken());
        }

        public static string Decrypt(byte[] encrptedText)
        {
            var decryptedBytes = ProtectedData.Unprotect(encrptedText, GetToken());
            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
        }
    }

}
