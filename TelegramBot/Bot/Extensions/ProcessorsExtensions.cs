using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.API.Models;
using TelegramBot.Bot.ChatProcessors;

namespace TelegramBot.Bot.Extensions
{
    internal static class ProcessorsExtensions
    {
        public static async Task ProcessUpdate(this IChatProcessorFactory factory, Update update)
        {
            var processors = factory.GetProcessors(update.Message.Chat.Id);
            foreach (var processor in processors)
            {
                await processor.ProcessUpdate(update);
            }
        }
    }
}
