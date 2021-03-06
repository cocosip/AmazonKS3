﻿using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonKS3.Sample
{
    class Program
    {


        private static SampleAppService _sampleAppService;

        static void Main(string[] args)
        {
            Console.WriteLine("使用AWSSDK.S3 SDK调用金山云KS3");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("2a57e28c-ddb6-4dca-858b-c028fefed42d");


            var configuration = builder.Build();
            var services = new ServiceCollection();
            services
                .AddLogging(l => l.AddConsole())
                .Configure<SampleAppOptions>(configuration.GetSection("SampleAppOptions"))
                .AddSingleton<SampleAppService>();

            var provider = services.BuildServiceProvider();

            _sampleAppService = provider.GetService<SampleAppService>();
            Run();
            Console.ReadLine();
        }




        public static async void Run()
        {
            //列出Bucket
            await _sampleAppService.ListBucketsAsync();

            //列出对象
            var objects = await _sampleAppService.ListObjectsAsync();

            //100001/100001001/20191203/5de60411c3b3175954a29588.dcm
            if (objects.Any())
            {
                var url0 = _sampleAppService.GetPreSignedURL(objects.FirstOrDefault());
            }

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

            //下载文件
            await _sampleAppService.SimpleGetObjectAsync(simpleUploadKey);

            //拷贝文件key
            var copyKey = await _sampleAppService.CopyObjectAsync(simpleUploadKey);

            await _sampleAppService.SimpleGetObjectAsync(copyKey);

            //删除文件
            await _sampleAppService.DeleteObject(simpleUploadKey);
            await _sampleAppService.DeleteObject(copyKey);
            //分片上传
            var multipartUploadKey = await _sampleAppService.MultipartUploadAsync();

            //Url
            var multipartUrl = _sampleAppService.GeneratePreSignedURL(multipartUploadKey);

            //获取文件信息
            //await _sampleAppService.getme(multipartUploadKey);


        }



    }
}
