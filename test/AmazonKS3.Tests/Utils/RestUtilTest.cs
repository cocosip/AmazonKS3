using AmazonKS3.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AmazonKS3.Tests.Utils
{
    public class RestUtilTest
    {
        [Theory]
        [InlineData(@"/", false)]
        [InlineData(@"/xyxxx", true)]
        [InlineData(@"/qqq.33", true)]
        [InlineData(@"/xxaq/we3", false)]
        [InlineData(@"/qwe/123.jpg",false)]
        public void ShouldEndWithSprit_Test(string input, bool expected)
        {
            var actual = RestUtil.ShouldEndWithSprit(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"//", false)]
        [InlineData(@"/www", false)]
        [InlineData(@"/xxaq/xxx", true)]
        [InlineData(@"/qwe/xxwqq/123.jpg", true)]
        public void IsOSSKey_Test(string input, bool expected)
        {
            var actual = RestUtil.IsOSSKey(input);
            Assert.Equal(expected, actual);
        }
    }
}
