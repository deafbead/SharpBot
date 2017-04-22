using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.API;
using TelegramBot.API.Models;
using TelegramBot.Bot.Types;
using TelegramBot.Logging;

namespace TelegramBot.Bot.Updates
{
    class PollingUpdatesProvider : IPollingUpdatesProvider
    {
        private readonly ApiClient _client;

        [Inject]
        public ILogger Logger { get; set; }

        private int _updateOffset = 0;
        
        public PollingUpdatesProvider(ApiClient client)
        {
            _client = client;
        }

        public async Task<ICollection<Update>> GetUpdates()
        {
            var response = await _client.SendRequestAsync<Response>("getUpdates", new UpdatesRequest
            {
                Offset = _updateOffset
            });

            if (response == null)
            {
                throw new WebException("Unable to get updates from Telegram server");
            }

            if (response.Success && response.Updates.Any())
            {
                _updateOffset = response.Updates.Max(t=>t.UpdateId) + 1;
                LogUpdates(response.Updates);
                return response.Updates;
            }

            return new Update[] { };
        }

        private void LogUpdates(IEnumerable<Update> updates)
        {
            if (Logger == null) return;
            foreach (var update in updates)
            {
                var message = update.Message;
                if (message == null) continue;
                Logger.Log(LogLevel.Message,
                    $">>> {update.Message.From.FirstName} {update.Message.From.LastName}: {update.Message.Text}");
            }
        }
        
    }
}
