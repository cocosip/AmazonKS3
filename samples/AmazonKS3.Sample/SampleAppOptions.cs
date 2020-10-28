namespace AmazonKS3.Sample
{
    public class SampleAppOptions
    {
        /// <summary>AK
        /// </summary>
        public string AccessKeyId { get; set; }
    
        /// <summary>SK
        /// </summary>
        public string SecretAccessKey { get; set; }
    
        public string ServerUrl { get; set; }

        public string DefaultBucket { get; set; }

        public string SimpleUploadFilePath { get; set; }

        public string MultipartUploadFilePath { get; set; }
    }
}
