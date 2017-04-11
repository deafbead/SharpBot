using System;
using System.Collections.Generic;

namespace TelegramBot.Bot.ChatProcessors
{
    public interface IChatProcessorFactory
    {
        void Add(IChatProcessor processor, Func<long, bool> shouldInvoke);
        IEnumerable<IChatProcessor> GetProcessors(long chatId);
    }
}