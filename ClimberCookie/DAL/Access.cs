using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimberCookie.DAL
{
    public static class Access
    {
        private static byte[] Hash(string word)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA256;
            const int pbkdfIterCount = 1000;
            const int pbkdf2SubkeyLength = 256 / 8;
            const int saltSize = 128 / 8;

            byte[] salt = new byte[saltSize];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(word, salt, Pbkdf2Prf, pbkdfIterCount, pbkdf2SubkeyLength);

            var outputBytes = new byte[1 + saltSize + pbkdf2SubkeyLength];
            outputBytes[0] = 0x00;
            Buffer.BlockCopy(salt, 0, outputBytes, 1, saltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + saltSize, pbkdf2SubkeyLength);
            return outputBytes;

        }

        public static string CreateNew(string user, string word)
        {
            //check if user is already in database, then hash and store as user
            // write to db with username and password
            return "success";

        }

        public static string Login(string user, string word)
        {
            // check if user is in db
            // then hash and compare for login
            return "success";
        }

    }
}
