using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.ChatProcessors
{
    internal static class CommonChat
    {
        public static long ChatId => Convert.ToInt64(ConfigurationManager.AppSettings["commonChatId"]);
    }
}
