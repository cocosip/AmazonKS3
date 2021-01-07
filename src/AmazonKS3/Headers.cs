using System;

namespace AmazonKS3
{
    /// <summary>KS3头部
    /// </summary>
    public static class Headers
    {
        /// <summary>
        /// Cache-Control
        /// </summary>
        public static string CACHE_CONTROL = "Cache-Control";

        /// <summary>
        /// Content-Disposition
        /// </summary>
        public static string CONTENT_DISPOSITION = "Content-Disposition";

        /// <summary>
        /// Content-Encoding
        /// </summary>
        public static string CONTENT_ENCODING = "Content-Encoding";

        /// <summary>
        /// Content-Length
        /// </summary>
        public static string CONTENT_LENGTH = "Content-Length";

        /// <summary>
        /// Content-MD5
        /// </summary>
        public static string CONTENT_MD5 = "Content-MD5";

        /// <summary>
        /// Content-Type
        /// </summary>
        public static string CONTENT_TYPE = "Content-Type";

        /// <summary>
        /// Date
        /// </summary>
        public static string DATE = "Date";

        /// <summary>
        /// ETag
        /// </summary>
        public static string ETAG = "ETag";

        /// <summary>
        /// Last-Modified
        /// </summary>
        public static string LAST_MODIFIED = "Last-Modified";

        /// <summary>
        /// Server
        /// </summary>
        public static string SERVER = "Server";

        /// <summary>
        /// User-Agent
        /// </summary>
        public static string USER_AGENT = "User-Agent";

        /// <summary>
        /// Host
        /// </summary>
        public static string HOST = "Host";

        /// <summary>
        /// Connection
        /// </summary>
        public static string CONNECTION = "Connection";

        /// <summary>
        /// Authorization
        /// </summary>
        public static string AUTHORIZATION = "Authorization";

        /** Prefix for general KS3 headers: x-kss- */
        public static string KS3_PREFIX = "x-kss-";

        /** 金山的客户端User-Agent **/
        public static string KS3_USER_AGENT = "KS3 User";

        /** KS3's canned ACL header: x-amz-acl */
        public static string KS3_CANNED_ACL = "x-kss-acl";

        /** KS3's alternative date header: x-kss-date */
        public static string KS3_ALTERNATE_DATE = "x-kss-date";

        /** Prefix for KS3 user metadata: x-kss-meta- */
        public static string KS3_USER_METADATA_PREFIX = "x-kss-meta-";

        /** KS3 response header for a request's request ID */
        public static string REQUEST_ID = "x-kss-request-id";

        /** Range header for the get object request */
        public static string RANGE = "Range";

        /** Modified since constraint header for the get object request */
        public static string GET_OBJECT_IF_MODIFIED_SINCE = "If-Modified-Since";

        /** Unmodified since constraint header for the get object request */
        public static string GET_OBJECT_IF_UNMODIFIED_SINCE = "If-Unmodified-Since";

        /** ETag matching constraint header for the get object request */
        public static String GET_OBJECT_IF_MATCH = "If-Match";

        /** ETag non-matching constraint header for the get object request */
        public static string GET_OBJECT_IF_NONE_MATCH = "If-None-Match";

        /** Header for optional redirect location of an object */
        public static string REDIRECT_LOCATION = "x-kss-website-redirect-location";

        /** Header for the FULL_CONTROL permission */
        public static string PERMISSION_FULL_CONTROL = "x-kss-grant-full-control";

        /** Header for the READ permission */
        public static string PERMISSION_READ = "x-kss-grant-read";

        /** Header for the WRITE permission */
        public static string PERMISSION_WRITE = "x-kss-grant-write";

        /** Header for the READ_ACP permission */
        public static string PERMISSION_READ_ACP = "x-kss-grant-read-acp";

        /** Header for the WRITE_ACP permission */
        public static String PERMISSION_WRITE_ACP = "x-kss-grant-write-acp";
        /** Header for the copy object*/
        public static string XKssCopySource = "x-kss-copy-source";

        /// <summary>
        /// AWS copy头 x-amz-copy-source
        /// </summary>
        public static string XAmzCopySource = "x-amz-copy-source";

        /// <summary>
        /// callback kss-async-process
        /// </summary>
        public static string AsynchronousProcessingList = "kss-async-process";

        /// <summary>
        /// kss-notifyurl
        /// </summary>
        public static string NotifyURL = "kss-notifyurl";

        /// <summary>
        /// TaskID
        /// </summary>
        public static string TaskId = "TaskID";

        /// <summary>
        /// 默认头部 application/octet-stream
        /// </summary>
        public static string DEFAULT_MIMETYPE = "application/octet-stream";
    }
}
