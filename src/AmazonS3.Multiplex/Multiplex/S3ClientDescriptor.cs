namespace Amazon.S3.Multiplex
{
    /// <summary>客户端配置信息描述
    /// </summary>
    public class S3ClientDescriptor
    {
        /// <summary>S3存储类型
        /// </summary>
        public S3VendorType VendorType { get; set; }

        /// <summary>AK
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>SK
        /// </summary>
        public string SecretAccessKey { get; set; }

        /// <summary>配置信息
        /// </summary>
        public AmazonS3Config Config { get; set; }

        /// <summary>客户端数量
        /// </summary>
        public int ClientCount { get; set; } = 10;


        public override string ToString()
        {
            return $"[S3VendorType:'{VendorType}',AK:'{AccessKeyId}',SK:'{SecretAccessKey}']";
        }

    }

}
