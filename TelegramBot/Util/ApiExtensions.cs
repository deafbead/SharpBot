using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelegramBot.API.Models;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Replies;

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

        public static bool MessageContains(this TelegramMessageEventArgs args, params string[] values)
        {
            return values.Any(y => args?.Message?.Text?.ToUpper().Contains(y.ToUpper()) ?? false);
        }


        public static bool MessageStartsWith(this TelegramMessageEventArgs args, params string[] values)
        {
            string message = args?.Message?.Text?.ToUpperInvariant().Trim();
            if (message == null) return false;
            return values.Any(v => message.StartsWith(v.Trim().ToUpperInvariant()));
        }

        public static bool MessageMatches(this TelegramMessageEventArgs args, params string[] regex)
        {
            string message = args?.Message?.Text;
            if (message == null) return false;
            return regex.Any(r => Regex.IsMatch(message, r));
        }
        public static TextReply TextReply(this TelegramMessageEventArgs input, string text)
        {
            return new TextReply(input.ChatId, text);
        }

        public static ImageReply ImageReply(this TelegramMessageEventArgs input, byte[] image, string caption = null)
        {
            return new ImageReply(input.ChatId, image, caption);
        }

        public static DocumentReply DocumentReply(this TelegramMessageEventArgs input, byte[] document,
            string caption = null)
        {
            return new DocumentReply(input.ChatId, document, caption);
        }

        public static VideoReply VideoReply(this TelegramMessageEventArgs input, byte[] video, string caption = null)
        {
            return new VideoReply(input.ChatId, video, caption);
        }

        public static ButtonsReply ButtonsReply(this TelegramMessageEventArgs input, string title,
            KeyboardButton[][] buttons)
        {
            return new ButtonsReply(input.ChatId, title, buttons);
        }

        public static IEnumerable<TextReply> ReplyAll(this IEnumerable<User> users, string text)
        {
            return users.Select(u => new TextReply(u.Id, text));
        }

    }
}
