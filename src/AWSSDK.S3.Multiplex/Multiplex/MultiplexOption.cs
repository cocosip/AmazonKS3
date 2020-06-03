using System.Collections.Generic;

namespace Amazon.S3.Multiplex
{
    /// <summary>多路复用配置
    /// </summary>
    public class MultiplexOption
    {
        /// <summary>配置信息
        /// </summary>
        public List<S3ClientDescriptor> Descriptors { get; set; }

        public MultiplexOption()
        {
            Descriptors = new List<S3ClientDescriptor>();
        }
    }
}
