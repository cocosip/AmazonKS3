using Amazon.S3;
using Amazon.S3.Multiplex;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazonS3.Multiplex.Sample
{
    class Program
    {
        private static IServiceCollection _services;
        private static IServiceProvider _provider;

        private static SampleAppService _sampleAppService;

        static void Main(string[] args)
        {
            Console.WriteLine("使用AWSSDK.S3 SDK调用金山云KS3");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("b1b10965-6797-4543-b5ed-3d56a7d8a132");


            var configuration = builder.Build();
            _services = new ServiceCollection();
            _services
                .AddLogging(l => l.AddConsole())
                .AddS3Multiplex()
                .AddS3MultiplexKS3Builder()
                .Configure<SampleAppOption>(configuration.GetSection("SampleAppOption"))
                .AddSingleton<SampleAppService>();

            _provider = _services.BuildServiceProvider();


            _sampleAppService = _provider.GetService<SampleAppService>();
            //Run();
            ClientBuilderTest();
            Console.ReadLine();
        }




        public static async void Run()
        {
            //列出Bucket
            await _sampleAppService.ListBucketsAsync();

            //列出对象
            await _sampleAppService.ListObjectsAsync();

            // 获取Bucket权限
            await _sampleAppService.GetAclAsync();

            //简单上传
            var simpleUploadKey = await _sampleAppService.SimpleUploadAsync();

            //下载文件
            await _sampleAppService.SimpleGetObjectAsync(simpleUploadKey);

            //获取预授权地址
            var url1 = _sampleAppService.GetPreSignedURL(simpleUploadKey);
            //生成预授权地址
            var url2 = _sampleAppService.GeneratePreSignedURL(simpleUploadKey);

            //拷贝文件key
            var copyKey = await _sampleAppService.CopyObjectAsync(simpleUploadKey);

            //下载Copy
            await _sampleAppService.SimpleGetObjectAsync(copyKey);

            ////删除文件
            //await DeleteObject(simpleUploadKey);
            //await DeleteObject(copyKey);
            ////分片上传
            //var multipartUploadKey = await MultipartUpload();

            ////Url
            //var multipartUrl = GeneratePreSignedURL(multipartUploadKey);

            ////获取文件信息
            //await GetMetadata(multipartUploadKey);

            ////获取ACL
            //await GetACL(multipartUploadKey);

        }




        #region 客户端生成测试
        static void ClientBuilderTest()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging(l =>
                {
                    l.SetMinimumLevel(LogLevel.Debug);
                    l.AddConsole();
                })
                .AddS3Multiplex()
                .AddS3MultiplexKS3Builder();

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Program>>();

            var s3ClientFactory = serviceProvider.GetService<IS3ClientFactory>();
            for (int i = 0; i < 5; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        var client = s3ClientFactory.GetOrAddClient("minioadmin", "minioadmin", () =>
                        {
                            return new S3ClientDescriptor()
                            {
                                VendorType = S3VendorType.Amazon,
                                AccessKeyId = "minioadmin",
                                SecretAccessKey = "minioadmin",
                                Config = new AmazonS3Config()
                                {
                                    ServiceURL = $"http://127.0.0.1/{Guid.NewGuid()}",
                                    ForcePathStyle = true,
                                    SignatureVersion = "2.0"
                                },
                                ClientCount = 10
                            };
                        });
                        logger.LogInformation("获取到了客户端,:{0}", client.Config.ServiceURL);

                        Thread.Sleep(10);
                    }

                }, TaskCreationOptions.LongRunning);
            }
        }
        #endregion
    }
}
