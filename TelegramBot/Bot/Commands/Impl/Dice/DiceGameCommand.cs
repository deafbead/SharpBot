using System.Collections.Generic;
using System.Linq;
using TelegramBot.Bot.Commands.Attributes;
using TelegramBot.Bot.Commands.Impl.Dice;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Dice
{
    [PersonalCommand]
    class DiceGameCommand : StatefulCommand
    {
        public sealed override ICommandState CurrentState { get; set; } 

        public DiceGameCommand()
        {
            CurrentState = new DiceBeginGameState(this);
        }

        public DiceBet CurrentBet { get; set; }

        public IList<DicePlayer> Players { get; set; } = new List<DicePlayer>();
        public int InitialDiceCount => 5;

        public IEnumerable<TextReply> ReplyAll(string message)
        {
            return Players.Select(p => new TextReply(p.ChatId, message));
        }

        
    }
}
