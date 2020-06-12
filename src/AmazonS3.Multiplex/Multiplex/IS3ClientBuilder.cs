namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端创建器
    /// </summary>
    public interface IS3ClientBuilder
    {
        /// <summary>供应商类型
        /// </summary>
        S3VendorType VendorType { get; }

        /// <summary>创建客户端
        /// </summary>
        IAmazonS3 BuildClient(S3ClientDescriptor descriptor);
    }
}
