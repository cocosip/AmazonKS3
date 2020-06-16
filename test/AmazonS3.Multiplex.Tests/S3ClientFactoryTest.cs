using Amazon.Runtime;
using Amazon.S3;
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
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption());
            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                clientFactory.GetClient("minio", "minio");
            });

        }

        [Fact]
        public void GetClient_Test()
        {
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption());

            var id = Util.GetIdentifierHash("minioadmin", "minioadmin");
            var descriptor = new S3ClientDescriptor()
            {
                VendorType = S3VendorType.Amazon,
                AccessKeyId = "minioadmin",
                SecretAccessKey = "minioadmin",
                ClientCount = 4,
                Config = new AmazonS3Config()
                {
                    ServiceURL = "http://192.168.0.4"
                }
            };

            var mockS3ClientPool = new Mock<IS3ClientPool>();
            mockS3ClientPool.Setup(x => x.Identifier)
                .Returns(id);
            mockS3ClientPool.Setup(x => x.Descriptor)
                .Returns(descriptor);
            mockS3ClientPool.Setup(x => x.GetClient())
                .Returns(new AmazonS3Client("minioadmin", "minioadmin", new AmazonS3Config()
                {
                    ServiceURL = "http://192.168.0.4"
                }));


            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            mockS3ClientPoolBuilder.Setup(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()))
                .Returns(mockS3ClientPool.Object);

            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);
            clientFactory.Register(descriptor);

            var s3Client = clientFactory.GetClient("minioadmin", "minioadmin");
            Assert.Equal("http://192.168.0.4", s3Client.Config.ServiceURL);

            mockS3ClientPool.Verify(x => x.GetClient(), Times.Once);
            mockS3ClientPoolBuilder.Verify(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()), Times.Once);
        }


        [Fact]
        public void Register_Test()
        {
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption());

            var mockS3ClientPool = new Mock<IS3ClientPool>();
            mockS3ClientPool.Setup(x => x.Identifier).Returns("123456");

            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            mockS3ClientPoolBuilder.Setup(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()))
                .Returns(mockS3ClientPool.Object);

            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);
            clientFactory.Register(new S3ClientDescriptor()
            {
                VendorType = S3VendorType.Amazon,
                ClientCount = 5,
                AccessKeyId = "minioadmin",
                SecretAccessKey = "minioadmin",
                Config = new Amazon.S3.AmazonS3Config()
                {

                }
            });
            mockS3ClientPoolBuilder.Verify(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()), Times.Once);
            mockS3ClientPool.Verify(x => x.Identifier, Times.AtLeastOnce);
        }

        [Fact]
        public void IsRegistered_Test()
        {
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption());

            var mockS3ClientPool = new Mock<IS3ClientPool>();
            var id = Util.GetIdentifierHash("minioadmin", "minioadmin");
            mockS3ClientPool.Setup(x => x.Identifier).Returns(id);

            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            mockS3ClientPoolBuilder.Setup(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()))
                .Returns(mockS3ClientPool.Object);

            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);

            var isRegistered = clientFactory.IsRegistered("minioadmin", "minioadmin");
            Assert.False(isRegistered);

            clientFactory.Register(new S3ClientDescriptor()
            {
                VendorType = S3VendorType.Amazon,
                ClientCount = 5,
                AccessKeyId = "minioadmin",
                SecretAccessKey = "minioadmin",
                Config = new Amazon.S3.AmazonS3Config()
                {

                }
            });
            var isRegistered2 = clientFactory.IsRegistered("minioadmin", "minioadmin");
            Assert.True(isRegistered2);
        }

        [Fact]
        public void GetOrAddClient_Test()
        {
            IOptions<MultiplexOption> options = Options.Create<MultiplexOption>(new MultiplexOption());
            var mockS3ClientPool = new Mock<IS3ClientPool>();
            mockS3ClientPool.Setup(x => x.Identifier)
                .Returns("123456");
            mockS3ClientPool.Setup(x => x.GetClient())
                .Returns(new AmazonS3Client("minioadmin", "minioadmin", new AmazonS3Config()
                {
                    ServiceURL = "http://127.0.0.1"
                }));


            var mockS3ClientPoolBuilder = new Mock<IS3ClientPoolBuilder>();
            mockS3ClientPoolBuilder.Setup(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()))
                .Returns(mockS3ClientPool.Object);

            IS3ClientFactory clientFactory = new S3ClientFactory(_mockLogger.Object, options, mockS3ClientPoolBuilder.Object);

            var s3Client = clientFactory.GetOrAddClient("minioadmin", "minioadmin", () =>
            {
                return new S3ClientDescriptor()
                {
                    VendorType = S3VendorType.Amazon,
                    AccessKeyId = "minioadmin",
                    SecretAccessKey = "minioadmin",
                    Config = new Amazon.S3.AmazonS3Config()
                    {

                    },
                    ClientCount = 10
                };
            });


            Assert.Equal("http://127.0.0.1", s3Client.Config.ServiceURL);
            mockS3ClientPoolBuilder.Verify(x => x.BuildS3ClientPool(It.IsAny<S3ClientDescriptor>()), Times.Once);
            mockS3ClientPool.Verify(x => x.GetClient(), Times.Once);
        }




    }
}
