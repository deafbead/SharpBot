using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Quiz.States
{
    class QuizBeginGameState : ICommandState
    {
        private readonly QuizCommand _command;

        public QuizBeginGameState(QuizCommand command)
        {
            _command = command;
        }

        public bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return input.MessageEquals("викторина");
        }

        public Task<IEnumerable<IReply>> Invoke(TelegramMessageEventArgs input)
        {

            var gs = new QuizMidGameState(_command);
            _command.CurrentState = gs;
            return gs.BeginGame(input);
        }
    }
}
