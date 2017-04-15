using System.Threading.Tasks;
using TelegramBot.API;
using TelegramBot.Bot.Types;
using TelegramBot.Logging;

namespace TelegramBot.Bot.Replies
{
    class ReplySender : IReplyVisitor<long, Task>, IReplySender
    {
        private readonly ApiClient _client;
        private readonly ILogger _logger;

        public ReplySender(ApiClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task Send(IReply reply)
        {
            _logger.Log(LogLevel.Message, $"<<< {reply.ToString()}");
            return reply.AcceptVisitor(this, reply.ChatId);
        }

        public Task VisitText(TextReply reply, long chatId)
        {
            return _client.SendRequestAsync<object>("sendMessage", new MessageToSend
            {
                ChatId = chatId.ToString(),
                Text = reply.Text,
                DisableWebPagePreview = false,
                DisableNotification = false,
                ReplayToMessageId = 0,
                ReplyMarkup = null
            });
        }

        public Task VisitImage(ImageReply reply, long chatId)
        {
            return _client.SendPhoto(chatId, reply.Image, reply.Caption);
        }

        public Task VisitButtons(ButtonsReply reply, long chatId)
        {
            return _client.SendRequestAsync<object>("sendMessage", new MessageToSend
            {
                ChatId = chatId.ToString(),
                Text = reply.Title,
                DisableWebPagePreview = false,
                DisableNotification = false,
                ReplayToMessageId = 0,
                ReplyMarkup = reply.Markup
            });
        }

        public Task VisitDocument(DocumentReply reply, long chatId)
        {
            return _client.SendDocument(chatId, reply.Document, reply.Caption);
        }

        public Task VisitVideo(VideoReply reply, long chatId)
        {
            return _client.SendVideo(chatId, reply.Video, reply.Caption);
        }
    }
}
