using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3.Multiplex
{
    /// <summary>SHA1算法工具类
    /// </summary>
    public static class Util
    {

        /// <summary>获取AK,SK的Hash值
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <returns></returns>
        public static string GetIdentifierHash(string accessKeyId, string secretAccessKey)
        {
            var source = $"{accessKeyId}#{secretAccessKey}";
            return GetSHA1Hash(source);
        }

        /// <summary>获取字符串的SHA1
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static string GetSHA1Hash(string source)
        {
            var sourceBuffer = Encoding.UTF8.GetBytes(source);
            using (var sha1 = SHA1.Create())
            {
                var hashBuffer = sha1.ComputeHash(sourceBuffer);
                return hashBuffer.Aggregate("", (current, b) => current + b.ToString("X2"));
            }
        }
    }
}
