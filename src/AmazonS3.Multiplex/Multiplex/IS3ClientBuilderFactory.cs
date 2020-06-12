namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端创建器工厂
    /// </summary>
    public interface IS3ClientBuilderFactory
    {
        /// <summary>获取客户端创建器
        /// </summary>
        IS3ClientBuilder GetClientBuilder(S3VendorType vendorType);
    }
}
