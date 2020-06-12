namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端连接池创建器
    /// </summary>
    public interface IS3ClientPoolBuilder
    {
        /// <summary>创建S3客户端连接池
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        IS3ClientPool BuildS3ClientPool(S3ClientDescriptor descriptor);
    }
}
