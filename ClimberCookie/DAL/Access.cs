using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.Text;

namespace ClimberCookie.DAL
{
    public static class Access
    {
        private static HashSalt Hash(string word)
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

            return new HashSalt(outputBytes,salt);

        }

        public static string CreateNew(string user, string word)
        {
            if (UserExists(user))
            {
                return "User Exists";
            }

            else
            {
                HashSalt hashSalt = Hash(word);
                string hash = Convert.ToBase64String(hashSalt.hash);
                string salt = Convert.ToBase64String(hashSalt.salt);


                var connectionstring = "Host=localhost;Username=postgres;Password=ClimberCookie!93;Database=ClimberCookie";
                using (var conn = new NpgsqlConnection(connectionstring))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("insert into users (username,pword,salt) values (@username,@pword,@salt)", conn))
                    {
                        cmd.Parameters.AddWithValue("username", user);
                        cmd.Parameters.AddWithValue("pword", hash);
                        cmd.Parameters.AddWithValue("salt", salt);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            return $"Failed with {ex.ToString()}";
                        }
                    }
                }
            }
            //check if user is already in database, then hash and store as user
            // write to db with username and password
            return "success";

        }

        public static string Login(string user, string word)
        {
            if (UserExistsWrongPassword(user))
            {
                return "User Exists";
            }

            HashSalt hashSalt = Hash(word);
            string hash = Convert.ToBase64String(hashSalt.hash);
            string salt = Convert.ToBase64String(hashSalt.salt);
            string storedHash;
            string storedSalt;

            var connectionstring = "Host=localhost;Username=postgres;Password=ClimberCookie!93;Database=ClimberCookie";
            using (var conn = new NpgsqlConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("select * from users where username = (@username)", conn))
                {
                    cmd.Parameters.AddWithValue("username", user);

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                storedHash = reader.GetString(2);
                                storedSalt = reader.GetString(3);
                                if (hash + salt == storedHash + storedSalt) ;
                                return "login!";
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        return $"Failed with {ex.ToString()}";
                    }
                }
            }

            // check if user is in db
            // then hash and compare for login
            return "success";
        }

        public static bool UserExistsWrongPassword(string user)
        {

        }

        public static bool UserExists(string user)
        {
            var connectionstring = "Host=localhost;Username=postgres;Password=ClimberCookie!93;Database=ClimberCookie";
            using (var conn = new NpgsqlConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("Select * from users where username = (@p)", conn))
                {
                    cmd.Parameters.AddWithValue("p", user);

                    using (var read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            return true;
                        }
                        
                        return false;
                    }
                }
            }
        }

        private class HashSalt
        {
            public byte[] hash { get; set; }
            public byte[] salt { get; set; }

            public HashSalt(byte[] hash,byte[] salt)
            {
                this.hash = hash;
                this.salt = salt;
            }
        }

    }
}
