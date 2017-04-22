using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.ChatProcessors;
using TelegramBot.Bot.Commands.Attributes;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands
{
    [PersonalCommand]
    class SayCommand : Command
    {
        private const string Prefix = "/say ";

        public override bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return input.MessageStartsWith(Prefix);
        }

        protected override Task<IEnumerable<IReply>> OnInvoke(TelegramMessageEventArgs input)
        {
            string message = input.Message.Text.Substring(Prefix.Length);
            return FromResult(new TextReply(CommonChat.ChatId, message));
        }
    }
}
