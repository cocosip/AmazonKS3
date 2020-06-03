using Microsoft.Extensions.DependencyInjection;
using System;

namespace Amazon.S3.Multiplex
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加S3多路复用
        /// </summary>
        public static IServiceCollection AddS3Multiplex(this IServiceCollection services, Action<MultiplexOption> configure = null)
        {
            if (configure == null)
            {
                configure = o => { };
            }

            services.Configure<MultiplexOption>(configure)
                .AddSingleton<IS3ClientFactory, S3ClientFactory>()
                .AddSingleton<IS3ClientPoolBuilder, S3ClientPoolBuilder>()
                .AddScoped<IS3ClientPool, S3ClientPool>()
                .AddScoped<S3ClientDescriptor>()
                ;
            return services;
        }
    }
}
