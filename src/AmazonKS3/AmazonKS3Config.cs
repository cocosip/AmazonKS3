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


        public static AmazonKS3Config GetDefault()
        {
            var config = new AmazonKS3Config()
            {
                ServiceURL = "http://ks3-cn-beijing.ksyun.com",
                ForcePathStyle = true
            };
            return config;
        }

    }
}
