namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端连接池
    /// </summary>
    public interface IS3ClientPool
    {
        /// <summary>身份标志
        /// </summary>
        string Identifier { get; }

        /// <summary>配置信息
        /// </summary>
        S3ClientDescriptor Descriptor { get; }

        /// <summary>初始化
        /// </summary>
        //void Initialize();

        /// <summary>获取客户端
        /// </summary>
        IAmazonS3 GetClient();
    }
}
