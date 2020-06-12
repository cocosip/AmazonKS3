using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端连接池创建器
    /// </summary>
    public class S3ClientPoolBuilder : IS3ClientPoolBuilder
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public S3ClientPoolBuilder(ILogger<S3ClientPoolBuilder> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>创建S3客户端连接池
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public IS3ClientPool BuildS3ClientPool(S3ClientDescriptor descriptor)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var injectDescriptor = scope.ServiceProvider.GetService<S3ClientDescriptor>();

                injectDescriptor.VendorType = descriptor.VendorType;
                injectDescriptor.AccessKeyId = descriptor.AccessKeyId;
                injectDescriptor.SecretAccessKey = descriptor.SecretAccessKey;
                injectDescriptor.Config = descriptor.Config;
                injectDescriptor.ClientCount = descriptor.ClientCount;

                _logger.LogInformation("新建客户端连接池,AK:'{0}',SK:'{1}'.", descriptor.AccessKeyId, descriptor.SecretAccessKey);

                return scope.ServiceProvider.GetService<IS3ClientPool>();
            }
        }
    }
}
