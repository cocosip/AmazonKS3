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
