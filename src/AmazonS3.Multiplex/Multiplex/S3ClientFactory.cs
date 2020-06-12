using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端工厂
    /// </summary>
    public class S3ClientFactory : IS3ClientFactory
    {
        private bool _isInitialized = false;
        private readonly object syncObject = new object();
        private readonly ConcurrentDictionary<string, IS3ClientPool> _poolDict;

        private readonly ILogger _logger;
        private readonly MultiplexOption _option;
        private readonly IS3ClientPoolBuilder _s3ClientPoolBuilder;

        public S3ClientFactory(ILogger<S3ClientFactory> logger, IOptions<MultiplexOption> options, IS3ClientPoolBuilder s3ClientPoolBuilder)
        {
            _logger = logger;
            _option = options.Value;
            _s3ClientPoolBuilder = s3ClientPoolBuilder;
            _poolDict = new ConcurrentDictionary<string, IS3ClientPool>();
        }

        /// <summary>初始化
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
            {
                _logger.LogInformation("S3ClientFactory已经初始化,请不要重复初始化。");
                return;
            }

            foreach (var descriptor in _option.Descriptors)
            {
                Register(descriptor);
            }
            _isInitialized = true;
        }


        /// <summary>获取客户端
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <returns></returns>
        public IAmazonS3 GetClient(string accessKeyId, string secretAccessKey)
        {
            var hash = Util.GetIdentifierHash(accessKeyId, secretAccessKey);
            if (!_poolDict.TryGetValue(hash, out IS3ClientPool clientPool))
            {
                throw new ArgumentNullException("该AK,SK对应的连接池还未被注册");
            }
            return clientPool.GetClient();
        }

        /// <summary>获取客户端如果不存在就新增客户端
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <param name="factory">客户端配置</param>
        /// <returns></returns>
        public IAmazonS3 GetOrAddClient(string accessKeyId, string secretAccessKey, Func<S3ClientDescriptor> factory)
        {
            var hash = Util.GetIdentifierHash(accessKeyId, secretAccessKey);
            if (!_poolDict.TryGetValue(hash, out IS3ClientPool clientPool))
            {
                lock (syncObject)
                {
                    if (!_poolDict.TryGetValue(hash, out clientPool))
                    {
                        var descriptor = factory.Invoke();
                        clientPool = _s3ClientPoolBuilder.BuildS3ClientPool(descriptor);
                        if (!_poolDict.TryAdd(clientPool.Identifier, clientPool))
                        {
                            _logger.LogWarning("'GetOrAddClient'添加新建的客户端连接池'{0}'失败!", clientPool.Identifier);
                        }
                    }
                }
            }

            if (clientPool == null)
            {
                throw new ArgumentException($"无法获取客户端连接池,AK:'{accessKeyId}',SK:'{secretAccessKey}'");
            }

            return clientPool.GetClient();
        }

        /// <summary>判断是否已经注册了客户端
        /// </summary>
        /// <param name="accessKeyId">AK</param>
        /// <param name="secretAccessKey">SK</param>
        /// <returns></returns>
        public bool IsRegistered(string accessKeyId, string secretAccessKey)
        {
            var hash = Util.GetIdentifierHash(accessKeyId, secretAccessKey);
            return _poolDict.ContainsKey(hash);
        }


        /// <summary>注册一个新的客户端
        /// </summary>
        public void Register(S3ClientDescriptor descriptor)
        {
            var hash = Util.GetIdentifierHash(descriptor.AccessKeyId, descriptor.SecretAccessKey);

            if (!_poolDict.ContainsKey(hash))
            {
                lock (syncObject)
                {
                    if (!_poolDict.ContainsKey(hash))
                    {
                        var clientPool = _s3ClientPoolBuilder.BuildS3ClientPool(descriptor);
                        if (!_poolDict.TryAdd(clientPool.Identifier, clientPool))
                        {
                            _logger.LogWarning("'GetOrAddClient'注册新建的客户端连接池'{0}'失败!", clientPool.Identifier);
                        }
                        else
                        {
                            _logger.LogDebug("注册新的客户端连接池:'{0}'成功!", clientPool.Identifier);
                        }
                    }
                }
            }
        }

    }
}
