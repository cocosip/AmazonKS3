using Amazon.S3;
using Amazon.S3.Multiplex;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AmazonS3.Multiplex.Tests
{
    public class S3ClientPoolTest
    {
        private readonly Mock<ILogger<S3ClientPool>> _mockLogger;

        public S3ClientPoolTest()
        {
            _mockLogger = new Mock<ILogger<S3ClientPool>>();
        }


        [Fact]
        public void GetClient_Test()
        {
            var descriptor = new S3ClientDescriptor()
            {
                VendorType = S3VendorType.Amazon,
                AccessKeyId = "minioadmin",
                SecretAccessKey = "minioadmin",
                ClientCount = 5,
                Config = new Amazon.S3.AmazonS3Config()
                {
                    ServiceURL = "http://127.0.0.1"
                }
            };
            var s3Client = new AmazonS3Client("minioadmin", "minioadmin", new AmazonS3Config()
            {
                ServiceURL = "http://127.0.0.1"
            }); 

            var mockS3ClientBuilder = new Mock<IS3ClientBuilder>();
            mockS3ClientBuilder.Setup(x => x.BuildClient(It.IsAny<S3ClientDescriptor>()))
                .Returns(s3Client);

            var mockS3ClientBuilderFactory = new Mock<IS3ClientBuilderFactory>();
            mockS3ClientBuilderFactory.Setup(x => x.GetClientBuilder(It.IsAny<S3VendorType>()))
                .Returns(mockS3ClientBuilder.Object);

       

            var s3ClientPool = new S3ClientPool(_mockLogger.Object, descriptor, mockS3ClientBuilderFactory.Object);
            Assert.Equal(descriptor, s3ClientPool.Descriptor);
            var s3Client2 = s3ClientPool.GetClient();
            Assert.Equal("http://127.0.0.1", s3Client.Config.ServiceURL);
            Assert.Equal(s3Client2.Config.ServiceURL, s3Client.Config.ServiceURL);
            Assert.Equal(s3Client2.Config.SignatureVersion, s3Client.Config.SignatureVersion);
            Assert.Equal(s3Client2.Config.ServiceVersion, s3Client.Config.ServiceVersion);

            mockS3ClientBuilderFactory.Verify(x => x.GetClientBuilder(It.IsAny<S3VendorType>()), Times.Once);
            mockS3ClientBuilder.Verify(x => x.BuildClient(descriptor), Times.Once);


        }

    }
}
