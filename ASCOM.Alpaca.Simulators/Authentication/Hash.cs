using ASCOM.Alpaca.Simulators;
using System;
using System.Security.Cryptography;

namespace ASCOM.Alpaca
{
    internal static class Hash
    {
        internal const int iters = 1000;
        internal const int salt_length = 16;
        internal const int key_length = 20;

        internal static string GetStoragePassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[salt_length]);

            byte[] hash = new Rfc2898DeriveBytes(password, salt, iters).GetBytes(key_length);

            byte[] hashBytes = new byte[key_length + salt_length];
            Array.Copy(salt, 0, hashBytes, 0, salt_length);
            Array.Copy(hash, 0, hashBytes, salt_length, key_length);

            return Convert.ToBase64String(hashBytes);
        }

        internal static bool Validate(string stored, string password)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(stored);

                byte[] salt = new byte[salt_length];

                Array.Copy(hashBytes, 0, salt, 0, salt_length);

                byte[] hash = new Rfc2898DeriveBytes(password, salt, iters).GetBytes(key_length);
                bool same = true;

                for (int i = 0; i < key_length; i++)
                    if (hashBytes[i + salt_length] != hash[i])
                        same = false;

                return same;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }
            return false;
        }
    }
}