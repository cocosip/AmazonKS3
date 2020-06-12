using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Amazon.S3.Multiplex
{
    /// <summary>S3客户端连接池
    /// </summary>
    public class S3ClientPool : IS3ClientPool
    {
        private int _sequence = 1;
        private readonly object syncObject = new object();
        private readonly ConcurrentDictionary<int, IAmazonS3> _clientDict;
        private readonly ILogger _logger;

        /// <summary>身份标志
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>配置信息
        /// </summary>
        public S3ClientDescriptor Descriptor { get; private set; }

        private readonly IS3ClientBuilderFactory _s3ClientBuilderFactory;

        /// <summary>Ctor
        /// </summary>
        public S3ClientPool(ILogger<S3ClientPool> logger, S3ClientDescriptor descriptor, IS3ClientBuilderFactory s3ClientBuilderFactory)
        {
            _logger = logger;
            Descriptor = descriptor;
            _s3ClientBuilderFactory = s3ClientBuilderFactory;

            Identifier = Util.GetIdentifierHash(descriptor.AccessKeyId, descriptor.SecretAccessKey);
            _clientDict = new ConcurrentDictionary<int, IAmazonS3>();
        }

        /// <summary>获取客户端
        /// </summary>
        public IAmazonS3 GetClient()
        {
            //序号
            var index = _sequence % Descriptor.ClientCount;

            //无法获取客户端
            if (!_clientDict.TryGetValue(index, out IAmazonS3 client))
            {
                lock (syncObject)
                {
                    if (!_clientDict.TryGetValue(index, out client))
                    {
                        var builder = _s3ClientBuilderFactory.GetClientBuilder(Descriptor.VendorType);
                        client = builder.BuildClient(Descriptor);
                        _logger.LogInformation("新增客户端,Index:'{0}',Descriptor:'{1}'.", index, Descriptor);

                        if (!_clientDict.TryAdd(index, client))
                        {
                            _logger.LogWarning("无法添加新增的客户端:Index:'{0}',Descriptor:'{1}'.", index, Descriptor);
                        }
                        Interlocked.Increment(ref _sequence);
                    }
                }
            }
            if (client == null)
            {
                throw new ArgumentException($"获取IAmazonS3客户端异常,{Descriptor}");
            }
            return client;

        }

    }
}
