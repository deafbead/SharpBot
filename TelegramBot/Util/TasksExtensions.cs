using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Bot.Replies;

namespace TelegramBot.Util
{
    internal static class TasksExtensions
    {
        public static Task<T> AsTaskResult<T>(this T result)
        {
            return Task.FromResult(result);
        }

        public static Task<IEnumerable<IReply>> AsResult(this IReply reply)
        {
            return Task.FromResult(reply.Yield());
        }
    }
}
