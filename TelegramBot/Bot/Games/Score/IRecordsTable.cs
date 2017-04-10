using System.Collections.Generic;
using TelegramBot.API.Models;

namespace TelegramBot.Bot.Games.Score
{
    interface IRecordsTable
    {
        void AddPoints(User user, int points, string gameId);
        IDictionary<User, int> Data(string gameId);
        int GetPoints(User user, string gameId);
    }
}