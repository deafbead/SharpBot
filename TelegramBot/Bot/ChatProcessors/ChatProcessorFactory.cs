using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.ChatProcessors
{
    class ChatProcessorInvoker
    {
        public IChatProcessor Processor { get; set; }
        public Func<long, bool> ShouldInvoke { get; set; }
    }

    class ChatProcessorFactory : IChatProcessorFactory
    {
        private readonly IList<ChatProcessorInvoker> _invokers = new List<ChatProcessorInvoker>();

        public IEnumerable<IChatProcessor> GetProcessors(long chatId)
        {
            return _invokers.Where(t => t.ShouldInvoke(chatId)).Select(t => t.Processor);
        }

        public void Add(IChatProcessor processor, Func<long, bool> shouldInvoke)
        {
            _invokers.Add(new ChatProcessorInvoker()
            {
                Processor = processor,
                ShouldInvoke = shouldInvoke
            });
        }
        
    }
}
