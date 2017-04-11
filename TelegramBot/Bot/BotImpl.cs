using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.API;
using TelegramBot.API.Models;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.ChatProcessors;
using TelegramBot.Bot.Commands;
using TelegramBot.Bot.Replies;
using TelegramBot.Bot.Updates;
using TelegramBot.Logging;

namespace TelegramBot.Bot
{
    public class BotImpl : IBot
    {
        private readonly IUpdatesProvider _updatesProvider;
        private readonly IChatProcessorFactory _chatProcessorFactory;

        [Inject]
        public ILogger Logger { get; set; }

        public BotImpl(IUpdatesProvider updatesProvider, IChatProcessorFactory chatProcessorFactory)
        {
            _updatesProvider = updatesProvider;
            _chatProcessorFactory = chatProcessorFactory;
        }

        public async Task Start()
        {
            IsRunning = true;
            await UpdateRoutine();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public bool IsRunning { get; private set; }

        private async Task UpdateRoutine()
        {
            Logger?.Log(LogLevel.Message, "Бот запущен.");
            await SkipUpdatesToEnd();

            while (IsRunning)
            {
                await Task.Delay(1000);

                try
                {
                    var updates = await _updatesProvider.GetUpdates();

                    foreach (var update in updates)
                    {
                        if (update.Message == null) continue;

                        var processors = _chatProcessorFactory.GetProcessors(update.Message.Chat.Id);
                        foreach (var processor in processors)
                        {
                            await processor.ProcessUpdate(update);
                        }

                        //await ProcessMessage(update.Message);
                    }
                }
                catch (Exception ex)
                {
                    ProcessException(ex);
                }
              
            }
        }

        private async Task SkipUpdatesToEnd()
        {
            ICollection<Update> updates;
            do
            {
                updates = await _updatesProvider.GetUpdates();
            } while (updates.Count > 0);
        }

      
        
        private void ProcessException(Exception ex)
        {
            //   IsRunning = false;
            Logger?.Log(LogLevel.Fatal, ex.Message);
        }

    }
}