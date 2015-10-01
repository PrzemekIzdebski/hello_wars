﻿using System;
using System.Net;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class WebClientHelper
    {
        public static TResponse PostData<TParam, TResponse>(string url, TParam parameter)
        {
            var webClient = CreateWebClient();
            var response = webClient.UploadString(url, JsonHelper.Serialize(parameter));

            return JsonHelper.Deserialize<TResponse>(response);
        }

        public static async Task<TResponse> PostDataAsync<TParam, TResponse>(string url, TParam parameter)
        {
            var webClient = CreateWebClient();
            var response = await webClient.UploadStringTaskAsync(new Uri(url), JsonHelper.Serialize(parameter));

            return JsonHelper.Deserialize<TResponse>(response);
        }

        public static TResponse GetData<TResponse>(string url)
        {
            var webClient = CreateWebClient();
            webClient.DownloadString(url);
            var downloadedString = webClient.DownloadString(url);

            return JsonHelper.Deserialize<TResponse>(downloadedString);
        }

        public static async Task<TResponse> GetDataAsync<TResponse>(string url)
        {
            var webClient = CreateWebClient();
            var downloadedString = await webClient.DownloadStringTaskAsync(url);
            return JsonHelper.Deserialize<TResponse>(downloadedString);
        }

        private static WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("Content-Type", "application/json");
            return webClient;
        }
    }
}