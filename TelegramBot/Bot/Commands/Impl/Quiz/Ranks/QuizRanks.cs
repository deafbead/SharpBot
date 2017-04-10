using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Bot.Commands.Quiz.Ranks
{
    
    class QuizRankImpl : IQuizRank
    {
        public string Name { get; set; }
        public int PointsRequired { get; set; }

        public IQuizRank NextRank { get; set; }
    }

    internal static class QuizRankFactory
    {
        public static IEnumerable<QuizRankDTO> GetRanks()
        {
            string[] ranks = new[]
            {
                "рядовой", "ефрейтор", "сержант", "прапорщик", "лейтенант",
                "капитан", "майор", "подполковник", "полковник",
                "генерал-майор", "генерал-лейтенант", "генерал-полковник",
                "генерал армии", "маршал", "космический десантник",
                "орк-разрушитель", "эльф 80-го уровня",
                "мем-мастер", "президент", "галактический лорд",
                "хокаге", "император", "волшебник"
            };
            return ranks.Select((r, i) => new QuizRankDTO
            {
                Name = r,
                PointsRequired = i * 10 + ((i == 0) ? 0 : 10)
            }).ToList();
        }
    }
}
