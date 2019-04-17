using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;

namespace VultrMgr
{
    /// <summary>
    /// Aes加密类
    /// </summary>
    class AesCrypto
    {
        /// <summary>
        /// Aes加密函数
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>加密结果</returns>
        public static string EncryptString(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            if (key.Length != 32)
                return null;
            try
            {
                SymmetricKeyAlgorithmProvider provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                IBuffer buffEncrypt = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
                IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
                CryptographicKey cKey = provider.CreateSymmetricKey(buffKey);
                IBuffer buffResult = CryptographicEngine.Encrypt(cKey, buffEncrypt, null);
                string strResult = CryptographicBuffer.EncodeToBase64String(buffResult);
                return strResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Aes解密函数
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>解密结果</returns>
        public static string DecryptString(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            if (key.Length != 32)
                return null;
            try
            {
                SymmetricKeyAlgorithmProvider provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                IBuffer buffDecrypt = CryptographicBuffer.DecodeFromBase64String(str);
                IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
                CryptographicKey cKey = provider.CreateSymmetricKey(buffKey);
                IBuffer buffResult = CryptographicEngine.Decrypt(cKey, buffDecrypt, null);
                string strResult=CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffResult);
                return strResult;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
