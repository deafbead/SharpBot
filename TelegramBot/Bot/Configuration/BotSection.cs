using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Configuration
{
    class BotSection : ConfigurationSection
    {
        [ConfigurationProperty("bots")]
        public BotConfigurationElementCollection Bots => this["bots"] as BotConfigurationElementCollection;

        public static BotSection Get()
        {
            return ConfigurationManager.GetSection("botSettings") as BotSection;
        }
    }
}
