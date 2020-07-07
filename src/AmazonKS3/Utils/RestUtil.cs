using Amazon.Runtime.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AmazonKS3.Utils
{
    /// <summary>KS3
    /// </summary>
    public static class RestUtil
    {
        private static readonly IList<string> SignedParameters = new List<string> {
            "acl","adp","torrent", "logging", "location", "policy", "requestPayment", "versioning",
            "versions", "versionId", "notification", "uploadId", "uploads", "partNumber", "website",
            "delete", "lifecycle", "tagging", "cors", "restore",
            "response-cache-contro", "response-content-disposition", "response-content-encoding",
            "response-content-language", "response-content-type", "response-expires"
        };

        /// <summary>Calculate the canonical string for a REST/HTTP request to KS3.
        /// </summary>
        /// <param name="method">请求方法(GET/POST)</param>
        /// <param name="resource">源地址</param>
        /// <param name="request">请求</param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        public static string MakeKS3CanonicalString(string method, string resource, IRequest request, string expires)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(method + "\n");

            // Add all interesting headers to a list, then sort them.  "Interesting"
            // is defined as Content-MD5, Content-Type, Date, and x-kss-
            IDictionary<string, string> headers = request.Headers;
            IDictionary<string, string> interestingHeaders = new SortedDictionary<string, string>();
            if (headers != null && headers.Count > 0)
            {
                foreach (string name in headers.Keys)
                {
                    var value = headers[name];

                    var lname = name.ToLower();

                    // Ignore any headers that are not particularly interesting.
                    if (lname.Equals(Headers.CONTENT_TYPE.ToLower()) || lname.Equals(Headers.CONTENT_MD5.ToLower()) || lname.Equals(Headers.DATE.ToLower()) ||
                        lname.StartsWith(Headers.KS3_PREFIX))
                        interestingHeaders.Add(lname, value);
                }
            }

            // Remove default date timestamp if "x-kss-date" is set.
            if (interestingHeaders.ContainsKey(Headers.KS3_ALTERNATE_DATE))
            {
                interestingHeaders[Headers.DATE.ToLower()] = "";
            }

            // Use the expires value as the timestamp if it is available. This trumps both the default
            // "date" timestamp, and the "x-kss-date" header.
            if (expires != null)
            {
                interestingHeaders[Headers.DATE.ToLower()] = expires;
            }
            // These headers require that we still put a new line in after them,
            // even if they don't exist.
            if (!interestingHeaders.ContainsKey(Headers.CONTENT_TYPE.ToLower()))
            {
                interestingHeaders.Add(Headers.CONTENT_TYPE.ToLower(), "");
            }
            if (!interestingHeaders.ContainsKey(Headers.CONTENT_MD5.ToLower()))
            {
                interestingHeaders.Add(Headers.CONTENT_MD5.ToLower(), "");
            }
            // Any parameters that are prefixed with "x-kss-" need to be included
            // in the headers section of the canonical string to sign
            foreach (var name in request.Parameters.Keys)
            {
                if (name.StartsWith(Headers.KS3_PREFIX))
                {
                    var value = request.Parameters[name];
                    interestingHeaders[name] = value;
                }
            }

            // Add all the interesting headers (i.e.: all that startwith x-kss- ;-))
            foreach (string name in interestingHeaders.Keys)
            {
                if (name.StartsWith(Headers.KS3_PREFIX))
                {
                    buf.Append(name + ":" + interestingHeaders[name]);
                }
                else
                {
                    buf.Append(interestingHeaders[name]);
                }
                buf.Append("\n");
            }

            // Add all the interesting parameters
            resource = resource.Replace("%5C", "/").Replace("//", "/%2F");
            resource = resource.EndsWith("%2F") ? resource.Substring(0, resource.Length - 3) : resource;
            buf.Append(resource);

            string[] parameterNames = request.Parameters.Keys.ToArray();
            Array.Sort(parameterNames);
            char separator = '?';
            foreach (var parameterName in parameterNames)
            {
                // Skip any parameters that aren't part of the canonical signed string
                if (!SignedParameters.Contains(parameterName))
                {
                    continue;
                }
                buf.Append(separator);
                buf.Append(parameterName);
                var parameterValue = request.Parameters[parameterName];
                if (parameterValue != null)
                {
                    buf.Append("=" + parameterValue);
                }
                separator = '&';
            }

            return buf.ToString();
        }


        /// <summary>是否需要以斜杠结尾
        /// </summary>
        public static bool ShouldEndWithSprit(string resourcePath)
        {
            //return resourcePath.StartsWith("/") && resourcePath.Length > 1 && resourcePath.LastIndexOf("/") == 0;
            //  /xxxx/
            var pattern = @"^/([A-Za-z0-9_\-\.]){4,256}$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        /// <summary>根据Resource判断是否为Key
        /// </summary>
        public static bool IsOSSKey(string resourcePath)
        {
            var pattern = @"^/[A-Za-z0-9]{1,36}([/]){1}([\w\W]*)$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        /// <summary>是否为拷贝
        /// </summary>
        public static bool IsCopyRequest(IRequest request)
        {
            return request.Headers.ContainsKey(Headers.XAmzCopySource);
        }


    }
}
