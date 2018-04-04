using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace PUBGMobileConfigDecrypter
{
    internal sealed class Encryption
    {
        private const string Key = "01111001";
        public static char DecryptCharacter(string encryptedCharacter)
        {
            var xorAsInt = Convert.ToInt32(Key, 2);
            var stringAsInt = Convert.ToInt32(encryptedCharacter, 16);

            var decrypted = xorAsInt ^ stringAsInt;
            return (char)decrypted;
        }

        public static string EncryptCharacter(char plaintextCharacter)
        {
            var charAsInt = (int)plaintextCharacter;
            var xorAsInt = Convert.ToInt32(Key, 2);

            var encrypted = xorAsInt ^ charAsInt;
            return encrypted.ToString("X2");
        }
    }
}
