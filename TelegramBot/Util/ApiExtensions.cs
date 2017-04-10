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

        private static bool StringEquals(string x, string y)
        {
            return x != null && y != null && string.Equals(x.Trim(), y.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool MessageEquals(this TelegramMessageEventArgs args, params string[] values)
        {
            return values.Any(y => StringEquals(args?.Message?.Text, y));
        }
    }
}
