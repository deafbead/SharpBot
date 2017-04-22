using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Owin;
using RestSharp;
using TelegramBot.API;

namespace TelegramBot.WebHost.Telegram
{
    public static class TelegramWebHook
    {
        public static IAppBuilder UseTelegramWebHook(this IAppBuilder app)
        {
            var client = new ApiClient(Token);
            client.SetWebHook(Host, Certificate).GetAwaiter().GetResult();
            return app;
        }
        
        public static string Token => ConfigurationManager.AppSettings["token"];

        private static string Host => ConfigurationManager.AppSettings["host-root"];

        private static byte[] Certificate => null;


    }
}