using AmazonKS3.Utils;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace AmazonKS3.Tests.Utils
{
    public class Md5UtilTest
    {
        [Fact]
        public void GetMd5Buffer_Test()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("helloworld"));
            var buffer = Md5Util.GetMd5Buffer(stream);

            var actual = BitConverter.ToString(buffer).Replace("-", "");

            Assert.Equal("FC5E038D38A57032085441E7FE7010B0", actual);
        }
    }
}
