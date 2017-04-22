using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Configuration
{
    public class BotConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name => (string)this["name"];

        [ConfigurationProperty("token", IsRequired = true)]
        public string Token => (string)this["token"];
    }
}
