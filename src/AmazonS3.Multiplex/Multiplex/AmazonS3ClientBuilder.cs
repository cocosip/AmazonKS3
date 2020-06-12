namespace Amazon.S3.Multiplex
{
    /// <summary>Amazon S3客户端创建器
    /// </summary>
    public class AmazonS3ClientBuilder : IS3ClientBuilder
    {
        /// <summary>S3创建器
        /// </summary>
        public S3VendorType VendorType => S3VendorType.Amazon;


        /// <summary>创建客户端
        /// </summary>
        public IAmazonS3 BuildClient(S3ClientDescriptor descriptor)
        {
            IAmazonS3 client = new AmazonS3Client(descriptor.AccessKeyId, descriptor.SecretAccessKey, descriptor.Config);
            return client;
        }
    }
}
