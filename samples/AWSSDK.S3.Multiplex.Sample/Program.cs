using Amazon.S3;
using Amazon.S3.Multiplex;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace AWSSDK.S3.Multiplex.Sample
{
    class Program
    {
        static IServiceProvider ServiceProvider;
        static ILogger Logger;
        static void Main(string[] args)
        {
            Console.WriteLine("多线程创建客户端测试!");

            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging(l =>
                {
                    l.AddConsole();
                })
                .AddS3Multiplex();

            ServiceProvider = services.BuildServiceProvider();
            Logger = ServiceProvider.GetService<ILogger<Program>>();


            Run();

            Console.ReadLine();
        }


        static void Run()
        {
            var s3ClientFactory = ServiceProvider.GetService<IS3ClientFactory>();
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
                                AccessKeyId = "minioadmin",
                                SecretAccessKey = "minioadmin",
                                ClientBuilder = descriptor =>
                                {
                                    return new AmazonS3Client(descriptor.AccessKeyId, descriptor.SecretAccessKey, new AmazonS3Config()
                                    {
                                        ServiceURL = $"http://127.0.0.1/{Guid.NewGuid()}",
                                        ForcePathStyle = true,
                                        SignatureVersion = "2.0"
                                    });
                                },
                                ClientCount = 10
                            };
                        });
                        Logger.LogInformation("获取到了客户端,:{0}", client.Config.ServiceURL);

                        Thread.Sleep(10);
                    }

                }, TaskCreationOptions.LongRunning);
            }
        }
    }
}
