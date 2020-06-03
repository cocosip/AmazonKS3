using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>配置S3多路复用
        /// </summary>
        public static IServiceProvider ConfigureS3Multiplex(this IServiceProvider provider, Action<MultiplexOption> configure = null)
        {
            if (configure != null)
            {
                var option = provider.GetService<IOptions<MultiplexOption>>().Value;
                configure(option);
            }

            var s3ClientFactory = provider.GetService<IS3ClientFactory>();
            s3ClientFactory.Initialize();
            return provider;
        }

    }
}
