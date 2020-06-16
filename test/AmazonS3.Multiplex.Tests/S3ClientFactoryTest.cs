using Amazon.S3.Multiplex;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace AmazonS3.Multiplex.Tests
{
    public class S3ClientFactoryTest
    {
        private readonly Mock<ILogger<S3ClientFactory>> _mockLogger;

        public S3ClientFactoryTest()
        {
            _mockLogger = new Mock<ILogger<S3ClientFactory>>();
        }

        [Fact]
        public void Initialize_Test()
        {
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption()
            {

            });
            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);

            clientFactory.Initialize();

            mockS3ClientPoolBuilder.Verify(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()), Times.Never);
        }

        [Fact]
        public void GetClient_Empty_Test()
        {
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption()
            {

            });
            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                clientFactory.GetClient("minio", "minio");
            });

        }


    }
}
