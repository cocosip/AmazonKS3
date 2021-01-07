using Amazon.Runtime.Internal.Auth;
using Amazon.S3;

namespace AmazonKS3
{
    /// <summary>Amazon KS3客户端
    /// </summary>
    public class AmazonKS3Client : AmazonS3Client
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AmazonKS3Client()
        {

        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="awsAccessKeyId"></param>
        /// <param name="awsSecretAccessKey"></param>
        /// <param name="clientConfig"></param>
        /// <returns></returns>
        public AmazonKS3Client(string awsAccessKeyId, string awsSecretAccessKey, AmazonKS3Config clientConfig) : base(awsAccessKeyId, awsSecretAccessKey, clientConfig)
        {

        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public AmazonKS3Client(AmazonKS3Config config) : base(config)
        {

        }

        /// <summary>使用KS3签名加密
        /// </summary>
        protected override AbstractAWSSigner CreateSigner()
        {
            return new KS3Signer();
        }

    }
}
