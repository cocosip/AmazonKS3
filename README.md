# AmazonKS3 使用AWSSDK.S3 SDK调用金山云KS3存储

## 为什么要做适配

- 金山云KS3官方的c# SDK代码比较陈旧,不支持.net core,不支持异步操作

- 金山云KS3对S3协议的支持有限,没有做到太大的兼容性导致无法直接使用AWSSDK.S3 SDK

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu) [![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/cocosip/AmazonKS3/blob/master/LICENSE) ![GitHub last commit](https://img.shields.io/github/last-commit/cocosip/AmazonKS3.svg) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/cocosip/AmazonKS3.svg)

[![build and publish](https://github.com/cocosip/AmazonKS3/actions/workflows/publish.yml/badge.svg)](https://github.com/cocosip/AmazonKS3/actions/workflows/publish.yml)

| Package  | Version | Downloads|
| -------- | ------- | -------- |
| `AmazonKS3` | [![NuGet](https://img.shields.io/nuget/v/AmazonKS3.svg)](https://www.nuget.org/packages/AmazonKS3) |![NuGet](https://img.shields.io/nuget/dt/AmazonKS3.svg)|
