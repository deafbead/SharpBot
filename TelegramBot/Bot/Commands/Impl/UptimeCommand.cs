using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands
{
    class UptimeCommand : Command
    {
        private DateTime StartDateTime { get; }
        public UptimeCommand()
        {
            StartDateTime = DateTime.Now;
        }

        public override bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return input.MessageEquals("аптайм", "uptime", "/uptime");
        }

        protected override Task<IEnumerable<IReply>> OnInvoke(TelegramMessageEventArgs input)
        {
            TimeSpan diff = DateTime.Now - StartDateTime;
            return FromResult(input.TextReply(diff.ToString(@"hh\:mm\:ss")));
        }
    }
}
