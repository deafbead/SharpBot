using System.Configuration;

namespace TelegramBot.Bot.Configuration
{
    public class BotConfigurationElementCollection : ConfigurationElementCollection
    {
        public BotConfiguration this[int index] => base.BaseGet(index: index) as BotConfiguration;

        public new BotConfiguration this[string name] => base.BaseGet(key: name) as BotConfiguration;

        protected override ConfigurationElement CreateNewElement()
        {
            return new BotConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BotConfiguration)element).Name;
        }
    }
}