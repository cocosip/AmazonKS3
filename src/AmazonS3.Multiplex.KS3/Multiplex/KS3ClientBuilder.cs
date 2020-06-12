using AmazonKS3;
using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>KS3客户端创建器
    /// </summary>
    public class KS3ClientBuilder : IS3ClientBuilder
    {
        /// <summary>KS3
        /// </summary>
        public S3VendorType VendorType => S3VendorType.KS3;


        /// <summary>创建客户端
        /// </summary>
        public IAmazonS3 BuildClient(S3ClientDescriptor descriptor)
        {
            if (typeof(AmazonKS3Config) != descriptor.Config.GetType())
            {
                throw new ArgumentException($"KS3ClientBuilder can't create client,S3VendorType is '{VendorType}',config type is '{descriptor.Config.GetType()}'.");
            }

            IAmazonS3 client = new AmazonKS3Client(descriptor.AccessKeyId, descriptor.SecretAccessKey, (AmazonKS3Config)descriptor.Config);
            return client;
        }
    }
}
