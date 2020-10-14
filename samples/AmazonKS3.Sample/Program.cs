using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AmazonKS3.Sample
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
                .AddUserSecrets("2a57e28c-ddb6-4dca-858b-c028fefed42d");


            var configuration = builder.Build();
            _services = new ServiceCollection();
            _services
                .AddLogging(l => l.AddConsole())
                .Configure<SampleAppOption>(configuration.GetSection("SampleAppOption"))
                .AddSingleton<SampleAppService>();

            _provider = _services.BuildServiceProvider();


            _sampleAppService = _provider.GetService<SampleAppService>();
            Run();
            Console.ReadLine();
        }




        public static async void Run()
        {
            ////列出Bucket
            //await _sampleAppService.ListBucketsAsync();

            ////列出对象
            //await _sampleAppService.ListObjectsAsync();

            //// 获取Bucket权限
            //await _sampleAppService.GetAclAsync();

            ////简单上传
            //var simpleUploadKey = await _sampleAppService.SimpleUploadAsync();

            ////下载文件
            //await _sampleAppService.SimpleGetObjectAsync(simpleUploadKey);

            //获取预授权地址
            var url1 = _sampleAppService.GetPreSignedURL("100001/100001001/20191203/5de6040ec3b3175954a29581.dcm");
            //生成预授权地址
            var url2 = _sampleAppService.GeneratePreSignedURL("100001/100001001/20191203/5de6040ec3b3175954a29581.dcm");

            ////拷贝文件key
            //var copyKey = await _sampleAppService.CopyObjectAsync(simpleUploadKey);

            ////下载Copy
            //await _sampleAppService.SimpleGetObjectAsync(copyKey);

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



    }
}
