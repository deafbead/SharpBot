using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.API.Models;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands;
using TelegramBot.Bot.Replies;
using TelegramBot.Bot.Updates;

namespace TelegramBot.Bot.ChatProcessors
{
    class ChatProcessor : IChatProcessor
    {
        private readonly ICommandInvoker _invoker;
        private readonly IReplySender _replySender;

        public ChatProcessor(ICommandInvoker invoker, IReplySender replySender)
        {
            _invoker = invoker;
            _replySender = replySender;
        }

        public async Task ProcessUpdate(Update update)
        {
            var args = new TelegramMessageEventArgs
            {
                ChatId = update.Message.Chat.Id,
                MessageId = update.Message.MessageId,
                From = update.Message.From,
                Message = update.Message
            };

            var results = await _invoker.Invoke(args);
            foreach (var result in results)
            {
                await _replySender.Send(result);
                await Task.Delay(300);
            }
        }
    }
}
