using Amazon.S3.Multiplex;
using Xunit;

namespace AmazonS3.Multiplex.Tests
{
    public class AmazonS3ClientBuilderTest
    {
        [Fact]
        public void BuildClient_Test()
        {
            IS3ClientBuilder builder = new AmazonS3ClientBuilder();
            Assert.Equal(S3VendorType.Amazon, builder.VendorType);

            var descriptor = new S3ClientDescriptor()
            {
                AccessKeyId = "minioadmin",
                SecretAccessKey = "minioadmin",
                ClientCount = 5,
                VendorType = S3VendorType.Amazon,
                Config = new Amazon.S3.AmazonS3Config()
                {
                    ServiceURL = "http://127.0.0.1",
                    ForcePathStyle = true,
                    SignatureVersion = "3.0"
                }
            };

            var client = builder.BuildClient(descriptor);
            Assert.Equal("http://127.0.0.1", client.Config.ServiceURL);
            Assert.Equal("3.0", client.Config.SignatureVersion);
        }
    }
}
