using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using TelegramBot.API;

namespace TelegramBot.WebHost.Telegram
{
    internal static class ApiClientExtensions
    {
        public static async Task SetWebHook(this ApiClient client, string host, byte[] certificate = null)
        {
            RestRequest restRequest = new RestRequest("setWebhook")
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddParameter("url", host.TrimEnd('/') + $"/webhook/{client.Token}");
            //restRequest.AddParameter("allowed_updates", new []{"message"});
            if (certificate != null)
            {
                restRequest.AddFile("certificate", certificate, "file");
            }
            var response = await client.Post<object>(restRequest);
        }
    }
}