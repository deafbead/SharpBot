using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using TelegramBot.WebHost.Telegram;

[assembly: OwinStartup(typeof(TelegramBot.WebHost.Startup))]

namespace TelegramBot.WebHost
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseTelegramWebHook();
        }
    }
}