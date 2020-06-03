using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>客户端配置信息描述
    /// </summary>
    public class S3ClientDescriptor
    {
        /// <summary>AK
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>SK
        /// </summary>
        public string SecretAccessKey { get; set; }

        /// <summary>客户端数量
        /// </summary>
        public int ClientCount { get; set; } = 10;

        /// <summary>创建客户端工厂
        /// </summary>
        public Func<S3ClientDescriptor, IAmazonS3> ClientBuilder { get; set; }
    }

}
