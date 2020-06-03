using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端工厂
    /// </summary>
    public interface IS3ClientFactory
    {
        /// <summary>初始化
        /// </summary>
        void Initialize();

        /// <summary>获取客户端
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <returns></returns>
        IAmazonS3 GetClient(string accessKeyId, string secretAccessKey);

        /// <summary>获取客户端如果不存在就新增客户端
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <param name="factory">客户端配置</param>
        /// <returns></returns>
        IAmazonS3 GetOrAddClient(string accessKeyId, string secretAccessKey, Func<S3ClientDescriptor> factory);

        /// <summary>判断是否已经注册了客户端
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <returns></returns>
        bool IsRegistered(string accessKeyId, string secretAccessKey);

        /// <summary>注册一个新的客户端
        /// </summary>
        void Register(S3ClientDescriptor descriptor);
    }
}
