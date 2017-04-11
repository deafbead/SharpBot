using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Bot.ChatProcessors
{

    class ChatConfiguration
    {
        private readonly IChatProcessorFactory _factory;

        public ChatConfiguration(IChatProcessorFactory factory)
        {
            _factory = factory;
        }

        public BindProcessorConfiguration BindProcessor(IChatProcessor processor)
        {
            return new BindProcessorConfiguration(processor, _factory);
        }
    }

    class BindProcessorConfiguration
    {
        private readonly IChatProcessor _processor;
        private readonly IChatProcessorFactory _factory;

        public BindProcessorConfiguration(IChatProcessor processor, IChatProcessorFactory factory)
        {
            _processor = processor;
            _factory = factory;
        }

        public void ToChats(params long[] chatIds)
        {
            _factory.Add(_processor, id => chatIds.Contains(id));
        }



        public void ToEverything()
        {
            _factory.Add(_processor, id => true);
        }

        public void ToEverythingExcept(params long[] ids)
        {
            _factory.Add(_processor, id=>!ids.Contains(id));
        }

        public void ToNothing()
        {
            _factory.Add(_processor, id => false);
        }
    }
}
