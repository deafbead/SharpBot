using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands.Dice;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Impl.Dice
{
    class DiceBeginGameState : ICommandState
    {
        private readonly DiceGameCommand _command;

        public DiceBeginGameState(DiceGameCommand command)
        {
            _command = command;
        }

        public bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return input.MessageEquals("/dice");
        }

        public Task<IEnumerable<IReply>> Invoke(TelegramMessageEventArgs input)
        {
            return Task.FromResult(InvokeSync(input));
        }

        private IEnumerable<IReply> InvokeSync(TelegramMessageEventArgs input)
        {
            var results = new List<IReply>();
            if (_command.Players.All(p => !p.User.Equals(input.From)))
            {
                var player = new DicePlayer(input.From, input.ChatId);
                _command.Players.Add(player);
                results.AddRange(_command.ReplyAll($"Игрок {player.Treat} добавлен."));
            }

            if (_command.Players.Count == 2)
            {
                DiceMidGameState state = new DiceMidGameState(_command);
                _command.CurrentState = state;
                results.AddRange(_command.ReplyAll($"Игра началась."));
                results.AddRange(state.BeginGame());
            }

            return results;
        }
    }
}
