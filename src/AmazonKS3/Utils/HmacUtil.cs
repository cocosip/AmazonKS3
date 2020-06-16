using System.Security.Cryptography;
using System.Text;

namespace AmazonKS3.Utils
{
    /// <summary>HMAC算法工具类
    /// </summary>
    public static class HmacUtil
    {
        /// <summary>获取HMAC-SHA1加密
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>Base64编码后的字符串</returns>
        public static byte[] GetHmacSha1(string source, string key, string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacSha1(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return hashBytes;
        }


        /// <summary>HMAC-SHA1加密
        /// </summary>
        /// <param name="sourceBytes">数据二进制</param>
        /// <param name="keyBytes">密钥二进制</param>
        /// <returns></returns>
        public static byte[] GetHmacSha1(byte[] sourceBytes, byte[] keyBytes)
        {
            using (var hmacSha1 = new HMACSHA1(keyBytes))
            {
                var hashBytes = hmacSha1.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }

    }
}
