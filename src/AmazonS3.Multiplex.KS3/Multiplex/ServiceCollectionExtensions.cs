using Microsoft.Extensions.DependencyInjection;

namespace Amazon.S3.Multiplex
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加KS3的依赖注入
        /// </summary>
        public static IServiceCollection AddS3MultiplexKS3Builder(this IServiceCollection services)
        {
            services.AddTransient<IS3ClientBuilder, KS3ClientBuilder>();
            return services;
        }
    }
}
