using Amazon.S3;
using Amazon.S3.Multiplex;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AWSSDK.S3.Multiplex.Tests
{
    public class S3ClientPoolTest
    {
        private readonly Mock<ILogger<S3ClientPool>> _mockLogger;

        public S3ClientPoolTest()
        {
            _mockLogger = new Mock<ILogger<S3ClientPool>>();
        }


        [Fact]
        public void Initialize_Test()
        {
            var descriptor = new S3ClientDescriptor()
            {
                AccessKeyId = "minioadmin",
                SecretAccessKey = "minioadmin",
                ClientCount = 3,
                ClientBuilder = new System.Func<S3ClientDescriptor, IAmazonS3>(d =>
                {
                    return new AmazonS3Client(d.AccessKeyId, d.SecretAccessKey, new AmazonS3Config()
                    {
                        ServiceURL = "http://127.0.0.1"
                    });
                })
            };

            IS3ClientPool s3ClientPool = new S3ClientPool(_mockLogger.Object, descriptor);

            s3ClientPool.Initialize();
        }

    }
}
