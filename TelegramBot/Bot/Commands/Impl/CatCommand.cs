using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Replies;
using TelegramBot.Logging;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands
{
    class CatCommand : Command
    {
        [Inject]
        public ILogger Logger { get; set; }

        public override bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            OneRequestPer(TimeSpan.FromSeconds(5));
            return input.MessageContains("кот");
        }

        protected override async Task<IEnumerable<IReply>> OnInvoke(TelegramMessageEventArgs input)
        {
            byte[] image = await TryGetRandomCat(attempts: 10);
            if (image == null)
            {
                return new IReply[]
                {
                    input.TextReply("Кажется, котобот сломался..."), 
                };
            }

            string replyText = "Кто-то сказал " + FindCatWord(input.Message.Text) + "???";
            IReply reply = IsGif(image)
                ? (IReply) input.VideoReply(image, replyText)
                : input.ImageReply(image, replyText);

            return reply.Yield();
        }

        private async Task<byte[]> TryGetRandomCat(int attempts)
        {
            for (int attemptIndex = 0; attemptIndex < attempts; attemptIndex++)
            {
                try
                {
                    var image = await GetRandomCatImage();
                    return image;
                }
                catch (WebException ex)
                {
                    Logger.Log(ex);
                }
            }

            return null;
        }

        private static Task<byte[]> GetRandomCatImage()
        {
            using (var client = new WebClient())
            {
                return client.DownloadDataTaskAsync("http://thecatapi.com/api/images/get");
            }
        }

        private string FindCatWord(string input)
        {
            string[] words = input.Split(' ', ',', ':', ';', '(', ')', '[', ']');
            foreach (var word in words)
            {
                if (word.ToUpper().Contains("КОТ"))
                {
                    return Regex.Replace(word, "кот", "КОТ", RegexOptions.IgnoreCase);
                }
            }

            return null;
        }

        protected override string GetOverThrottleText(TimeSpan remainingTime)
        {
            return "Прости, следующий котик будет только через " + remainingTime.ToHmsString();
        }

        private bool IsGif(byte[] bytes)
        {
            return bytes.Take(3).SequenceEqual(new byte[]
                   {
                       (byte)'G', (byte)'I', (byte)'F'
                   });
        }
    }
}
