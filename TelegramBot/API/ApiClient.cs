using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TelegramBot.Util;

namespace TelegramBot.API
{
    public class ApiClient
    {
        string BaseApiAddress = @"https://api.telegram.org/bot";

        private readonly IRestClient _client;

        public ApiClient(string token)
        {
            _client = new RestClient(BaseApiAddress + token);
        }

        public Task<TResult> SendRequestAsync<TResult>(string method, object obj = null)
        {
            var request = new RestRequest(method, Method.POST) {RequestFormat = DataFormat.Json};
            if (obj != null)
            {
                request.AddHeader("Content-type", "application/json");
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(obj);
            }
            return Post<TResult>(request);
        }

        public Task SendPhoto(long chatId, byte[] bytes, string caption = null)
        {
              return SendData<object>("photo", chatId, bytes, caption);
        }

        public Task SendDocument(long chatId, byte[] bytes, string caption = null)
        {
               return SendData<object>("document", chatId, bytes, caption);
        }

        public Task SendVideo(long chatId, byte[] bytes, string caption = null)
        {
            return SendData<object>("video", chatId, bytes, caption);
        }

        private Task<TResult> SendData<TResult>(string dataId, long chatId, byte[] bytes, string caption)
        {
            RestRequest restRequest = new RestRequest("send" + dataId)
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            if (!string.IsNullOrEmpty(caption))
            {
                restRequest.AddParameter("caption", caption);
            }
            restRequest.AddParameter("chat_id", chatId);
            restRequest.AddFile(dataId, bytes, "file");
            return Post<TResult>(restRequest);
        }

        private async Task<TResult> Post<TResult>(IRestRequest request)
        {
            var response = await _client.ExecutePostTaskAsync(request);
            var result = JsonConvert.DeserializeObject<TResult>(response.Content);
            return result;
        }
    }
}