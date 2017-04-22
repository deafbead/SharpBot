using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.API;
using TelegramBot.API.Models;
using TelegramBot.Bot.ChatProcessors;
using TelegramBot.Bot.Extensions;
using TelegramBot.Bot.Updates;
using TelegramBot.Logging;

namespace TelegramBot.Bot
{
    class PollingBot : AbstractBot, IPollingBot
    {
        private readonly IPollingUpdatesProvider _updatesProvider;
        private readonly IChatProcessorFactory _chatProcessorFactory;

        public PollingBot(IPollingUpdatesProvider updatesProvider, IChatProcessorFactory chatProcessorFactory, ApiClient client) : base(client)
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
                        await _chatProcessorFactory.ProcessUpdate(update);
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
    }
}
