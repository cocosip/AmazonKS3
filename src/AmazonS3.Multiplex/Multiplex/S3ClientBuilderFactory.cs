using Microsoft.Extensions.DependencyInjection;
using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端创建器工厂
    /// </summary>
    public class S3ClientBuilderFactory : IS3ClientBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>Ctor
        /// </summary>
        public S3ClientBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>获取客户端创建器
        /// </summary>
        public IS3ClientBuilder GetClientBuilder(S3VendorType vendorType)
        {
            var services = _serviceProvider.GetServices<IS3ClientBuilder>();
            foreach (var service in services)
            {
                if (service.VendorType == vendorType)
                {
                    return service;
                }
            }
            throw new ArgumentException($"Can't find any inject for 'IS3ClientBuilder',and 'S3VendorType' is {vendorType}.");
        }
    }
}
