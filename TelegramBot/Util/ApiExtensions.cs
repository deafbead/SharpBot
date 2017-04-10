using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;

namespace TelegramBot.Util
{
    internal static class ApiExtensions
    {
        public static bool HasText(this TelegramMessageEventArgs args)
        {
            return args?.Message?.Text != null;
        }
    }
}
