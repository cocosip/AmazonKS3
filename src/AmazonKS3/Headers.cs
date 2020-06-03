using System;

namespace AmazonKS3
{
    /// <summary>KS3头部
    /// </summary>
    public static class Headers
    {
        public static string CACHE_CONTROL = "Cache-Control";
        public static string CONTENT_DISPOSITION = "Content-Disposition";
        public static string CONTENT_ENCODING = "Content-Encoding";
        public static string CONTENT_LENGTH = "Content-Length";
        public static string CONTENT_MD5 = "Content-MD5";
        public static string CONTENT_TYPE = "Content-Type";
        public static string DATE = "Date";
        public static string ETAG = "ETag";
        public static string LAST_MODIFIED = "Last-Modified";
        public static string SERVER = "Server";
        public static string USER_AGENT = "User-Agent";
        public static string HOST = "Host";
        public static string CONNECTION = "Connection";

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

        //AWS copy头
        public static string XAmzCopySource = "x-amz-copy-source";

        /**callback */
        public static string AsynchronousProcessingList = "kss-async-process";
        public static string NotifyURL = "kss-notifyurl";
        public static string TaskId = "TaskID";

        //默认头部
        public static string DEFAULT_MIMETYPE = "application/octet-stream";
    }
}
