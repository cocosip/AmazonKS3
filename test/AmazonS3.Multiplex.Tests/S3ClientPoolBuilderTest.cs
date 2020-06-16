using Amazon.S3.Multiplex;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace AmazonS3.Multiplex.Tests
{
    public class S3ClientPoolBuilderTest
    {
        private readonly Mock<ILogger<S3ClientPoolBuilder>> _mockLogger;

        public S3ClientPoolBuilderTest()
        {
            _mockLogger = new Mock<ILogger<S3ClientPoolBuilder>>();
        }

        [Fact]
        public void BuildS3ClientPool_Test()
        {
            var descriptor1 = new S3ClientDescriptor()
            {

            };

            var descriptor2 = new S3ClientDescriptor()
            {
                VendorType = S3VendorType.Amazon,
                AccessKeyId = "minioadmin1",
                SecretAccessKey = "minioadmin1",
                ClientCount = 2,
                Config = new Amazon.S3.AmazonS3Config()
                {
                    ServiceURL = "http://192.168.0.2"
                }
            };

            var mockS3ClientPool = new Mock<IS3ClientPool>();
            mockS3ClientPool.Setup(x => x.Descriptor)
                .Returns(descriptor2);

            var mockScopeServiceProvider = new Mock<IServiceProvider>();
            mockScopeServiceProvider.Setup(x => x.GetService(typeof(S3ClientDescriptor)))
                .Returns(descriptor1);
            mockScopeServiceProvider.Setup(x => x.GetService(typeof(IS3ClientPool)))
            .Returns(mockS3ClientPool.Object);

        

            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(x => x.ServiceProvider)
                .Returns(mockScopeServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory.Setup(x => x.CreateScope())
                .Returns(mockScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(mockServiceScopeFactory.Object);


            IS3ClientPoolBuilder s3ClientPoolBuilder = new S3ClientPoolBuilder(_mockLogger.Object, mockServiceProvider.Object);

            var s3ClientPool = s3ClientPoolBuilder.BuildS3ClientPool(descriptor2);

            Assert.Equal(mockS3ClientPool.Object.Descriptor.AccessKeyId, s3ClientPool.Descriptor.AccessKeyId);
            Assert.Equal(mockS3ClientPool.Object.Descriptor.SecretAccessKey, s3ClientPool.Descriptor.SecretAccessKey);

            mockScopeServiceProvider.Verify(x => x.GetService(typeof(S3ClientDescriptor)), Times.Once);
            mockScopeServiceProvider.Verify(x => x.GetService(typeof(IS3ClientPool)), Times.Once);

        }


    }
}
