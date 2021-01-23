using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// How to compute and compare hash values by using Visual C#
// https://support.microsoft.com/en-us/help/307020/how-to-compute-and-compare-hash-values-by-using-visual-c


namespace DbgHelpers.Utilities
{
    public class HashHelper
    {
        public static byte[] HashGenerate(string SourceData)
        {            
            byte[] SourceBytes;
            byte[] hashArr;
                        
            SourceBytes = UnicodeEncoding.Unicode.GetBytes(SourceData);

            //Compute hash based on source data
            hashArr = new MD5CryptoServiceProvider().ComputeHash(SourceBytes);
            
            return hashArr;            
        }

        public static bool HashCompare( byte[] hash1, byte[] hash2)
        {            
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i != hash1.Length; i++)
                if (hash1[i] != hash2[i])
                    return false;

            return true;            
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }



    }
}
