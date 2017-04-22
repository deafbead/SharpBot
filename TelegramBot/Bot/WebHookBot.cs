using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.API;
using TelegramBot.API.Models;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.ChatProcessors;
using TelegramBot.Bot.Commands;
using TelegramBot.Bot.Extensions;
using TelegramBot.Bot.Replies;
using TelegramBot.Bot.Updates;
using TelegramBot.Logging;

namespace TelegramBot.Bot
{
    public class WebHookBot : AbstractBot, IWebHookBot
    {
        private readonly IChatProcessorFactory _chatProcessorFactory;

        public WebHookBot(IChatProcessorFactory chatProcessorFactory, ApiClient client) : base(client)
        {
            _chatProcessorFactory = chatProcessorFactory;
        }


        public async Task ProcessUpdate(Update update)
        {
            try
            {
                await _chatProcessorFactory.ProcessUpdate(update);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
        }
    }
}