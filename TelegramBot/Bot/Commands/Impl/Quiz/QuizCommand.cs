using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.API.Models;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands.Quiz.Ranks;
using TelegramBot.Bot.Commands.Quiz.States;
using TelegramBot.Bot.Games.Score;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Quiz
{
    internal class QuizCommand : StatefulCommand
    {
        public override ICommandState CurrentState { get; set; }
        public const string GameName = "quiz";

        public IRecordsTable RecordsTable { get; }
        public IQuizRanksProvider RanksProvider { get; }

        public QuizCommand(IRecordsTable recordsTable, IQuizRanksProvider ranksProvider)
        {
            RecordsTable = recordsTable;
            RanksProvider = ranksProvider;
            CurrentState = new QuizBeginGameState(this);
        }

        public string Treat(User user)
        {
            int points = RecordsTable.GetPoints(user, GameName);
            string rank = RanksProvider.GetRank(points).Name;
            return $"{rank} {user.FirstName} {user.LastName}";
        }

        public int GetScore(User user)
        {
            return RecordsTable.GetPoints(user, GameName);
        }

        public IQuizRank GetRank(User user)
        {
            int points = GetScore(user);
            return RanksProvider.GetRank(points);
        }

        public void AddPoints(User user, int points)
        {
            RecordsTable.AddPoints(user, points, GameName);
        }
    }
}

