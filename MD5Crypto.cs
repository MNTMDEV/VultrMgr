using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace VultrMgr
{
    /// <summary>
    /// MD5加密解密类
    /// </summary>
    class MD5Crypto
    {
        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="str">明文</param>
        /// <returns>MD5哈希值</returns>
        public static string EncryptString(string str)
        {
            try
            {
                HashAlgorithmProvider alg=HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
                IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(str,BinaryStringEncoding.Utf8);
                IBuffer hash=alg.HashData(buffer);
                string result = CryptographicBuffer.EncodeToHexString(hash);
                return result;
            }
            catch(Exception)
            {
                return null;
            }
        }

    }
}
