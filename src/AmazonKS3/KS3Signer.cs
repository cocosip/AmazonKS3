using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;
using AmazonKS3.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AmazonKS3
{
    /// <summary>
    /// KS3 Signaer
    /// </summary>
    public class KS3Signer : AWS4Signer
    {

        /// <summary>
        /// Sign method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clientConfig"></param>
        /// <param name="metrics"></param>
        /// <param name="awsAccessKeyId"></param>
        /// <param name="awsSecretAccessKey"></param>
        public override void Sign(IRequest request, IClientConfig clientConfig, RequestMetrics metrics, string awsAccessKeyId, string awsSecretAccessKey)
        {

            base.Sign(request, clientConfig, metrics, awsAccessKeyId, awsSecretAccessKey);
            //内容Md5
            SetContentMd5(request);
            //移除Md5
            //RemoveContentMd5(request);

            //设置Copy头
            SetCopySource(request);

            //设置User-Agent
            SetUserAgent(request);

            //resourcePath
            var resourcePath = GetResourcePath(request);
            //待签名字符串
            var canonicalString = RestUtil.MakeKS3CanonicalString(request.HttpMethod, resourcePath, request, null);

            var hmacBytes = HmacUtil.GetHmacSha1(canonicalString, awsSecretAccessKey);
            //签名
            var signature = Convert.ToBase64String(hmacBytes);

            //默认头部添加
            if (!request.Headers.ContainsKey(Headers.CONTENT_TYPE))
            {
                request.Headers.Add(Headers.CONTENT_TYPE, Headers.DEFAULT_MIMETYPE);
            }
            //如果已经添加了认证,就移除
            if (request.Headers.ContainsKey(Headers.AUTHORIZATION))
            {
                request.Headers.Remove(Headers.AUTHORIZATION);
            }
            request.Headers.Add("Authorization", "KSS " + awsAccessKeyId + ":" + signature);
        }


        /// <summary>计算头部的MD5
        /// </summary>
        private void SetContentMd5(IRequest request)
        {
            //上传文件数据
            if (request.ContentStream != null && !request.Headers.ContainsKey(Headers.CONTENT_MD5))
            {
                if (request.ContentStream is MD5Stream stream1)
                {
                    var stream = stream1.GetNonWrapperBaseStream();
                    var md5Buffer = Md5Util.GetMd5Buffer(stream);
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }
                    var contentMd5 = Convert.ToBase64String(md5Buffer);
                    request.Headers.Add(Headers.CONTENT_MD5, contentMd5);

                    //不包含内容
                    if (!request.Headers.ContainsKey(Headers.CONTENT_TYPE))
                    {
                        request.Headers.Add(Headers.CONTENT_TYPE, Headers.DEFAULT_MIMETYPE);
                    }
                }
            }
        }

        ///// <summary>是否需要移除Md5
        ///// </summary>
        //private void RemoveContentMd5(IRequest request)
        //{
        //    if (request.Headers.ContainsKey(Headers.CONTENT_MD5) && request.ContentStream == null)
        //    {
        //        request.Headers.Remove(Headers.CONTENT_MD5);
        //    }
        //}

        /// <summary>获取ResourcePath
        /// </summary>
        private string GetResourcePath(IRequest request)
        {
            var sb = new StringBuilder(request.ResourcePath);

            //非上传才需要处理
            if (RestUtil.ShouldEndWithSprit(sb.ToString()))
            {
                sb.Append("/");
            }
            //如果包含SubResource
            if (request.SubResources.Count > 0)
            {
                sb.Append("?");
                var sortedDict = new SortedDictionary<string, string>(request.SubResources);
                foreach (var item in sortedDict)
                {
                    sb.Append(item.Key);
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        sb.Append($"={item.Value}&");
                    }
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>设置Copy
        /// </summary>
        private void SetCopySource(IRequest request)
        {
            //判断是否为拷贝
            if (RestUtil.IsCopyRequest(request))
            {
                var copySource = request.GetHeaderValue(Headers.XAmzCopySource);
                if (!copySource.StartsWith("/"))
                {
                    copySource = "/" + copySource;
                }
                request.Headers.Add(Headers.XKssCopySource, copySource);
            }
        }

        /// <summary>设置金山客户端List
        /// </summary>
        private void SetUserAgent(IRequest request)
        {
            if (request.Headers.ContainsKey(Headers.USER_AGENT))
            {
                request.Headers.Remove(Headers.USER_AGENT);
            }
            request.Headers.Add(Headers.USER_AGENT, Headers.KS3_USER_AGENT);
        }

        //private bool ShouldEndWithSprit(IRequest request)
        //{
        //    return string.IsNullOrWhiteSpace(request.OriginalRequest);
        //}

    }
}
