using Amazon.S3;
using Amazon.S3.Multiplex;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AWSSDK.S3.Multiplex.Tests
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


            var mockScopeServiceProvider = new Mock<IServiceProvider>();
            mockScopeServiceProvider.Setup(x => x.GetService(typeof(S3ClientDescriptor)))
                .Returns(new S3ClientDescriptor()
                {
                    AccessKeyId = "ak00001",
                    SecretAccessKey = "sk00001",
                    ClientCount = 5,
                    ClientBuilder = new Func<S3ClientDescriptor, Amazon.S3.IAmazonS3>(d =>
                    {
                        return new AmazonS3Client(d.AccessKeyId, d.SecretAccessKey, new AmazonS3Config()
                        {
                            ServiceURL = "http://192.168.0.101"
                        });
                    })
                });

            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.Setup(x => x.ServiceProvider)
                .Returns(mockScopeServiceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            IS3ClientPoolBuilder s3ClientPoolBuilder = new S3ClientPoolBuilder(_mockLogger.Object, mockServiceProvider.Object);

            var descriptor = new S3ClientDescriptor()
            {
                AccessKeyId = "minioadmin1",
                SecretAccessKey = "minioadmin1",
                ClientCount = 10,
                ClientBuilder = new Func<S3ClientDescriptor, IAmazonS3>(d =>
                {
                    return new AmazonS3Client(d.AccessKeyId, d.SecretAccessKey, new AmazonS3Config()
                    {
                        ServiceURL = "http://192.168.1.1"
                    });
                })
            };
            s3ClientPoolBuilder.BuildS3ClientPool(descriptor);


            mockScopeServiceProvider.Verify(x => x.GetService(typeof(S3ClientDescriptor)), Times.Once);
            mockScopeServiceProvider.Verify(x => x.GetService(typeof(IS3ClientPool)), Times.Once);

        }


    }
}
