using Amazon.S3;

namespace AmazonKS3
{
    /// <summary>KS3配置
    /// </summary>
    public class AmazonKS3Config : AmazonS3Config
    {
        public override string UserAgent => "KS3 User";

        public AmazonKS3Config()
        {
            SignatureVersion = "2";
        }

    }
}
