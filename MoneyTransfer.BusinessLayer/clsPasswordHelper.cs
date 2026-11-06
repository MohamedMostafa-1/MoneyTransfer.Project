using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransfer.BusinessLayer
{
    internal class clsPasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        private static bool _FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diff = 0;

            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }

            return diff == 0;
        }

        public static string HashPassword(string OrginalPassword) 
        {
            byte[] Salt = new byte[SaltSize];
            using (var Generator = RandomNumberGenerator.Create()) 
            {
                Generator.GetBytes(Salt);
            }

            byte[] Hash;
            using (var PBKDF2 = new  Rfc2898DeriveBytes(
                OrginalPassword ,
                Salt,
                Iterations,
                HashAlgorithmName.SHA256
                ))
            {
                Hash = PBKDF2.GetBytes(HashSize);
            }

            return "PBKDF2$" + Iterations + "$"
                   + Convert.ToBase64String(Salt) + "$"
                   + Convert.ToBase64String(Hash);
        }

        public static bool VerifyPassword(string Password , string StoredHash)
        {

            string[] Parts = StoredHash.Split('$');

            if (Parts.Length != 4 || Parts[0] != "PBKDF2$")
                return false;

            //Store Old Data
            int Iterations = int.Parse(Parts[1]);
            byte[] Salt = Convert.FromBase64String(Parts[2]);
            byte[] OrginalHash = Convert.FromBase64String(Parts[3]);

            byte[] NewHash;
            using (var PBKDF2 = new Rfc2898DeriveBytes(Password , Salt , Iterations , HashAlgorithmName.SHA256))
            {
                NewHash = PBKDF2.GetBytes(OrginalHash.Length);
            }

            return _FixedTimeEquals(NewHash, OrginalHash);
        }
      

    }
}
