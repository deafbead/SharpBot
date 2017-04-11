using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.ChatProcessors
{
    class ChatProcessorFactoryBuilder
    {
        private readonly IChatProcessorFactory _factory = new ChatProcessorFactory();

        public static ChatProcessorFactoryBuilder Create()
        {
            return new ChatProcessorFactoryBuilder();   
        }

        public IChatProcessorFactory BuildFactory()
        {
            return _factory;
        }

        public ChatProcessorFactoryBuilder ConfigureChat(Action<ChatConfiguration> configure)
        {
            var config = new ChatConfiguration(_factory);
            configure(config);
            return this;
        }
    }
}
