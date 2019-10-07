using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace EWarehouse.Services
{
    public class HashPasswordService
    {
        public static string GetHshedPassword(string password)
        {
            byte[] salt = new byte[] { 65, 66, 67 };
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
           password: password,
           salt: salt,
           prf: KeyDerivationPrf.HMACSHA1,
           iterationCount: 10000,
           numBytesRequested: 256 / 8));

            return hashed;
        }

    }

}
