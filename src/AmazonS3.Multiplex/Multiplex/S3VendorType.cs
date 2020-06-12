namespace Amazon.S3.Multiplex
{
    /// <summary>S3存储类型
    /// </summary>
    public enum S3VendorType
    {
        /// <summary>Amazon存储,或者能完全兼容Amazon的存储
        /// </summary>
        Amazon = 1,

        /// <summary>KS3存储
        /// </summary>
        KS3 = 2
    }
}
