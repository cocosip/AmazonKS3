using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AmazonKS3.Utils
{
    /// <summary>Md5计算工具
    /// </summary>
    public static class Md5Util
    {

        /// <summary>Md5加密
        /// </summary>
        public static string GetMd5(string sourceString)
        {
            var data = Encoding.UTF8.GetBytes(sourceString);
            return GetMd5(data);
        }

        /// <summary>Md5加密
        /// </summary>
        public static string GetMd5(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                //其他的代码写法: md5.ComputeHash(data).Aggregate("", (current, b) => current + b.ToString("X2"))
                return BitConverter.ToString(md5.ComputeHash(data)).Replace("-", "");
            }
        }

        /// <summary>Md5加密,返回byte[]
        /// </summary>
        public static byte[] GetMd5Buffer(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        /// <summary>Md5加密,返回byte[]
        /// </summary>
        public static byte[] GetMd5Buffer(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(stream);
            }
        }
    }
}
