using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TelegramBot.API;
using TelegramBot.API.Models;
using TelegramBot.Bot.Types;
using TelegramBot.Logging;

namespace TelegramBot.Bot.Updates
{
    class UpdatesProvider : IUpdatesProvider
    {
        private readonly ApiClient _client;
        private readonly ILogger _logger;

        private int _updateOffset = 0;
        private List<int> _processedUpdates = new List<int>();

        public UpdatesProvider(ApiClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
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
            foreach (var update in updates)
            {
                var message = update.Message;
                if (message == null) continue;
                _logger.Log(LogLevel.Message,
                    $">>> {update.Message.From.FirstName} {update.Message.From.LastName}: {update.Message.Text}");
            }
        }
    }
}
