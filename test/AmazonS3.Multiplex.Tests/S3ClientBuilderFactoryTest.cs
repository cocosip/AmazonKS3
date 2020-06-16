using Amazon.S3.Multiplex;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AmazonS3.Multiplex.Tests
{
    public class S3ClientBuilderFactoryTest
    {
        [Fact]
        public void GetClientBuilder_Empty_Test()
        {

            var mockS3ClientBuilder = new Mock<IS3ClientBuilder>();
            mockS3ClientBuilder.Setup(x => x.VendorType).Returns(S3VendorType.KS3);
            var list = new List<IS3ClientBuilder>
            {
                mockS3ClientBuilder.Object
            };

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceProvider.Setup(x => x.GetService(typeof(IEnumerable<IS3ClientBuilder>)))
                .Returns(list);

            IS3ClientBuilderFactory clientBuilderFactory = new S3ClientBuilderFactory(mockServiceProvider.Object);

            Assert.Throws<ArgumentException>(() =>
            {
                clientBuilderFactory.GetClientBuilder(S3VendorType.Amazon);
            });

            mockServiceProvider.Verify(x => x.GetService(typeof(IEnumerable<IS3ClientBuilder>)), Times.Once);
        }

        [Fact]
        public void GetClientBuilder_Test()
        {

            var mockS3ClientBuilder = new Mock<IS3ClientBuilder>();
            mockS3ClientBuilder.Setup(x => x.VendorType).Returns(S3VendorType.Amazon);
            var list = new List<IS3ClientBuilder>
            {
                mockS3ClientBuilder.Object
            };

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceProvider.Setup(x => x.GetService(typeof(IEnumerable<IS3ClientBuilder>)))
                .Returns(list);

            IS3ClientBuilderFactory clientBuilderFactory = new S3ClientBuilderFactory(mockServiceProvider.Object);

            var s3ClientBuilder = clientBuilderFactory.GetClientBuilder(S3VendorType.Amazon);

            Assert.Equal(S3VendorType.Amazon, s3ClientBuilder.VendorType);

            mockServiceProvider.Verify(x => x.GetService(typeof(IEnumerable<IS3ClientBuilder>)), Times.Once);

        }





    }
}
