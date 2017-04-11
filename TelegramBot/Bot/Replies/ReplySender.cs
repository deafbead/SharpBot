using System.Threading.Tasks;
using TelegramBot.API;
using TelegramBot.Bot.Types;

namespace TelegramBot.Bot.Replies
{
    class ReplySender : IReplyVisitor<long, Task>, IReplySender
    {
        private readonly ApiClient _client;

        public ReplySender(ApiClient client)
        {
            _client = client;
        }

        public Task Send(IReply reply, long chatId)
        {
            return reply.AcceptVisitor(this, chatId);
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
            return _client.SendDocument(chatId, reply.Document);
        }

        public Task VisitVideo(VideoReply reply, long chatId)
        {
            return _client.SendVideo(chatId, reply.Video);
        }
    }
}
