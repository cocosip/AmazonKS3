using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;

namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端连接池
    /// </summary>
    public class S3ClientPool : IS3ClientPool
    {
        private int _sequence = 1;
        private readonly int _clientCount = 1;
        private readonly ConcurrentDictionary<int, IAmazonS3> _clientDict;

        private readonly ILogger _logger;

        /// <summary>身份标志
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>配置信息
        /// </summary>
        public S3ClientDescriptor Descriptor { get; private set; }

        /// <summary>Ctor
        /// </summary>
        public S3ClientPool(ILogger<S3ClientPool> logger, S3ClientDescriptor descriptor)
        {
            _logger = logger;
            Descriptor = descriptor;
            _clientCount = descriptor.ClientCount;
            Identifier = Util.GetIdentifierHash(descriptor.AccessKeyId, descriptor.SecretAccessKey);
            _clientDict = new ConcurrentDictionary<int, IAmazonS3>();
        }

        /// <summary>初始化
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < _clientCount; i++)
            {
                var client = Descriptor.ClientBuilder(Descriptor);
                if (!_clientDict.TryAdd(i, client))
                {
                    _logger.LogWarning("添加客户端'{0}'失败,AK:'{1}',SK:'{2}'.", i, Descriptor.AccessKeyId, Descriptor.SecretAccessKey);
                }
            }
        }


        /// <summary>获取客户端
        /// </summary>
        public IAmazonS3 GetClient()
        {
            var index = _sequence % _clientCount;
            var client = _clientDict[index];
            Interlocked.Increment(ref _sequence);
            return client;
        }

    }
}
