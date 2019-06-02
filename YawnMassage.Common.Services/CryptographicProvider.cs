using System;
using System.Security.Cryptography;

namespace YawnMassage.Common.Services
{
    public static class CryptographicProvider
    {
        public static string GenerateUserPINHash(string userPIN)
        {
            byte[] userPINByteArray = new byte[userPIN.Length];
            for (int i = 0; i < userPIN.Length; i++)
                userPINByteArray[i] = (byte)userPIN[i];

            var sha = new SHA1CryptoServiceProvider();
            byte[] pin = sha.ComputeHash(userPINByteArray);

            return Convert.ToBase64String(pin, 0, pin.Length);
        }
    }
}
