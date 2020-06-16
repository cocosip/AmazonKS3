using AmazonKS3.Utils;
using System;
using Xunit;

namespace AmazonKS3.Tests.Utils
{
    public class HmacUtilTest
    {
        [Fact]
        public void GetHmacSha1_Test()
        {
            var source = "helloworld";
            var buffer = HmacUtil.GetHmacSha1(source, "111111");
            var actual= Convert.ToBase64String(buffer);

            Assert.Equal("LY9OPidfBXHYmNJQ7ht4I49xYMU=", actual);
        }

    }
}
