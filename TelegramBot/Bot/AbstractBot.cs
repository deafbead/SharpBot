using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.API;
using TelegramBot.Logging;

namespace TelegramBot.Bot
{
    public abstract class AbstractBot : IBot
    {
        protected AbstractBot(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        [Inject]
        public ILogger Logger { get; set; }

        protected void ProcessException(Exception ex)
        {
            Logger?.Log(LogLevel.Fatal, ex.Message);
        }

        public ApiClient ApiClient { get; }
        
    }
}
