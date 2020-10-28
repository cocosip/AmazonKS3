using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AmazonKS3.Sample
{
    public class SampleAppService
    {
        private IAmazonS3 _client = null;

        private readonly SampleAppOptions _options;
        public SampleAppService(IOptions<SampleAppOptions> options)
        {
            _options = options.Value;
        }


        protected IAmazonS3 GetClient()
        {
            if (_client == null)
            {
                _client = new AmazonKS3Client(_options.AccessKeyId, _options.SecretAccessKey, new AmazonKS3Config()
                {
                    ServiceURL = _options.ServerUrl,
                    ForcePathStyle = false,
                    SignatureVersion = "2.0"
                });
            }
            return _client;
        }


        /// <summary>列出Bucket
        /// </summary>
        public async Task ListBucketsAsync()
        {
            var listBucketsResponse = await GetClient().ListBucketsAsync();
            Console.WriteLine("---列出Buckets---,OwnerId:{0}", listBucketsResponse.Owner.Id);
            foreach (var bucket in listBucketsResponse.Buckets)
            {
                Console.WriteLine("BucketName:{0},创建时间:{1}", bucket.BucketName, bucket.CreationDate.ToString("yyyy-MM-dd HH:mm"));
            }
        }

        /// <summary>获取Bucket权限
        /// </summary>
        public async Task GetAclAsync()
        {
            Console.WriteLine("---获取当前Bucket的权限---");

            var getACLResponse = await GetClient().GetACLAsync(new GetACLRequest()
            {
                BucketName = _options.DefaultBucket
            });

            foreach (var grant in getACLResponse.AccessControlList.Grants)
            {
                Console.WriteLine("当前Bucket权限:{0}", grant.Permission.Value);
            }
        }


        /// <summary>列出对象
        /// </summary>
        public async Task ListObjectsV2Async(string prefix = "", string delimiter = "", int count = 10)
        {
            //查询文件
            Console.WriteLine("---列出Bucket,'Prefix:{0}','Delimiter:{1}','Count:{2}'---", prefix, delimiter, count);

            var listObjectsV2Response = await GetClient().ListObjectsV2Async(new ListObjectsV2Request()
            {
                BucketName = _options.DefaultBucket,
                Prefix = prefix,
                Delimiter = delimiter,
                MaxKeys = count
            });
            foreach (var s3Object in listObjectsV2Response.S3Objects)
            {
                Console.WriteLine("S3对象Key:{0}", s3Object.Key);
            }
        }


        /// <summary>列出对象
        /// </summary>
        public async Task ListObjectsAsync(string prefix = "", string delimiter = "", int count = 10)
        {
            //查询文件
            Console.WriteLine("---列出Bucket前10个文件---");

            var listObjectsResponse = await GetClient().ListObjectsAsync(new ListObjectsRequest()
            {
                BucketName = _options.DefaultBucket,
                //Prefix = prefix,
                //Delimiter = delimiter,
                MaxKeys = count
            });

            foreach (var s3Object in listObjectsResponse.S3Objects)
            {
                Console.WriteLine("S3对象Key:{0}", s3Object.Key);
            }
        }



        /// <summary>获取指定的文件
        /// </summary>
        private async Task GetObjectAsync(string key)
        {
            //查询文件
            Console.WriteLine("---获取指定的文件---");

            var getObjectResponse = await GetClient().GetObjectAsync(new GetObjectRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key,
            });

            Console.WriteLine("获取对象返回:对象Key:{0},ETag:{1}", getObjectResponse?.Key, getObjectResponse?.ETag);
        }

        /// <summary>简单上传文件
        /// </summary>
        public async Task<string> SimpleUploadAsync()
        {
            var key = $"{Guid.NewGuid()}.dcm";
            Console.WriteLine("---上传简单文件,上传Key:{0}---", key);

            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = _options.DefaultBucket,
                AutoCloseStream = true,
                Key = key,
                FilePath = _options.SimpleUploadFilePath,
                CannedACL = S3CannedACL.Private,
            };
            //进度条
            putObjectRequest.StreamTransferProgress += (sender, args) =>
            {
                Console.WriteLine("ProgressCallback - Progress: {0}%, TotalBytes:{1}, TransferredBytes:{2} ",
                    args.TransferredBytes * 100 / args.TotalBytes, args.TotalBytes, args.TransferredBytes);
            };

            var putObjectResponse = await GetClient().PutObjectAsync(putObjectRequest);
            Console.WriteLine("简单上传成功,Etag:{0}", putObjectResponse.ETag);
            return key;
        }

        /// <summary>简单下载文件
        /// </summary>
        public async Task SimpleGetObjectAsync(string key)
        {
            Console.WriteLine("---简单下载文件,Key:{0}---", key);
            var getObjectRequest = new GetObjectRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key
            };

            var getObjectResponse = await GetClient().GetObjectAsync(getObjectRequest);

            Console.WriteLine("简单下载文件成功,Key:{0}", getObjectResponse.Key);
        }


        /// <summary>获取预授权地址
        /// </summary>
        public string GetPreSignedURL(string key)
        {
            //获取文件下载地址
            Console.WriteLine("---获取预授权地址---");

            var url = GetClient().GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key,
                Expires = DateTime.Now.AddMinutes(5),
            });
            Console.WriteLine("获取预授权地址:{0}", url);
            return url;
        }

        /// <summary>生成预授权地址
        /// </summary>
        public string GeneratePreSignedURL(string key)
        {
            Console.WriteLine("---生成预授权地址---");

            var url = GetClient().GeneratePreSignedURL(_options.DefaultBucket, key, DateTime.Now.AddMinutes(10), null);
            Console.WriteLine("生成预授权地址:{0}", url);
            return url;
        }


        /// <summary>拷贝文件
        /// </summary>
        public async Task<string> CopyObjectAsync(string key)
        {
            var destinationKey = $"copyfiles/{Guid.NewGuid()}.dcm";
            Console.WriteLine("---拷贝文件,目标:{0}---", destinationKey);
            var copyObjectResponse = await GetClient().CopyObjectAsync(new CopyObjectRequest()
            {
                SourceBucket = _options.DefaultBucket,
                DestinationBucket = _options.DefaultBucket,
                SourceKey = key,
                DestinationKey = destinationKey
            });

            Console.WriteLine("拷贝文件成功,RequestId:{0},拷贝目标Key:{1}", copyObjectResponse.ResponseMetadata.RequestId, destinationKey);

            return destinationKey;

        }

        /// <summary>删除文件
        /// </summary>
        public async Task DeleteObject(string key)
        {
            Console.WriteLine("---删除文件---,Key:{0}", key);

            var deleteObjectResponse = await GetClient().DeleteObjectAsync(new DeleteObjectRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key
            });

            Console.WriteLine("删除文件成功,DeleteMarker:{0}", deleteObjectResponse.DeleteMarker);
        }


        /// <summary>分片上传
        /// </summary>
        public async Task<string> MultipartUploadAsync()
        {
            var key = $"{Guid.NewGuid()}.dcm";
            Console.WriteLine("---分片上传文件,上传Key:{0}---", key);

            //初始化分片上传
            var initiateMultipartUploadResponse = await GetClient().InitiateMultipartUploadAsync(new InitiateMultipartUploadRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key,
            });

            //上传Id
            var uploadId = initiateMultipartUploadResponse.UploadId;
            // 计算分片总数。
            var partSize = 5 * 1024 * 1024;
            var fi = new FileInfo(_options.MultipartUploadFilePath);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }
            // 开始分片上传。partETags是保存partETag的列表，OSS收到用户提交的分片列表后，会逐一验证每个分片数据的有效性。 当所有的数据分片通过验证后，OSS会将这些分片组合成一个完整的文件。
            var partETags = new List<PartETag>();

            var uploadPartTasks = new List<Task>();

            using (var fs = File.Open(_options.MultipartUploadFilePath, FileMode.Open))
            {
                for (var i = 0; i < partCount; i++)
                {
                    var skipBytes = (long)partSize * i;
                    // 定位到本次上传起始位置。
                    //fs.Seek(skipBytes, 0);
                    // 计算本次上传的片大小，最后一片为剩余的数据大小。
                    var size = (int)((partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes));

                    byte[] buffer = new byte[size];
                    fs.Read(buffer, 0, size);

                    uploadPartTasks.Add(Task.Run<UploadPartResponse>(() =>
                    {
                        return GetClient().UploadPartAsync(new UploadPartRequest()
                        {
                            BucketName = _options.DefaultBucket,
                            UploadId = uploadId,
                            Key = key,
                            InputStream = new MemoryStream(buffer),
                            PartSize = size,
                            PartNumber = i + 1
                        });

                    }).ContinueWith(t =>
                    {
                        partETags.Add(new PartETag(t.Result.PartNumber, t.Result.ETag));
                        Console.WriteLine("finish {0}/{1}", partETags.Count, partCount);
                    }));

                    //分片上传
                    //var uploadPartResponse = await Client.UploadPartAsync(new UploadPartRequest()
                    //{
                    //    BucketName = BucketName,
                    //    UploadId = uploadId,
                    //    Key = key,
                    //    InputStream = new MemoryStream(buffer),
                    //    PartSize = size,
                    //    PartNumber = i + 1
                    //});

                    //partETags.Add(new PartETag(uploadPartResponse.PartNumber, uploadPartResponse.ETag));
                    //Console.WriteLine("finish {0}/{1}", partETags.Count, partCount);
                }
                //Console.WriteLine("分片上传完成");
            }

            Task.WaitAll(uploadPartTasks.ToArray());

            Console.WriteLine("共:{0}个PartETags", partETags.Count);

            //列出所有分片
            Console.WriteLine("---列出所有分片,UploadId:{0}---", uploadId);

            var listPartsResponse = await GetClient().ListPartsAsync(new ListPartsRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key,
                UploadId = uploadId
            });
            foreach (var part in listPartsResponse.Parts)
            {
                Console.WriteLine("分片序号:{0},分片ETag:{1}", part.PartNumber, part.ETag);
            }


            var completeMultipartUploadResponse = await GetClient().CompleteMultipartUploadAsync(new CompleteMultipartUploadRequest()
            {
                BucketName = _options.DefaultBucket,
                Key = key,
                UploadId = uploadId,
                PartETags = partETags
            });

            Console.WriteLine("分片上传完成,Key:{0}", completeMultipartUploadResponse.Key);
            return completeMultipartUploadResponse.Key;
        }

    }
}
