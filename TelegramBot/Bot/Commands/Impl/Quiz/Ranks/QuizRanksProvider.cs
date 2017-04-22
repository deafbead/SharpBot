using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TelegramBot.Persistence;

namespace TelegramBot.Bot.Commands.Quiz.Ranks
{
    class QuizRanksProvider : IQuizRanksProvider
    {
        private readonly IRepository<QuizRankDTO> _persistence;

        public QuizRanksProvider(IRepository<QuizRankDTO> persistence)
        {
            _persistence = persistence;
        }

        public IQuizRank GetRank(int points)
        {
            var ranks = _persistence.GetAll().OrderBy(t => t.PointsRequired).ToList();
            return GetRankInternal(points, ranks);
        }

        private IQuizRank GetRankInternal(int points, IList<QuizRankDTO> orderedRanks)
        {
            var rank = orderedRanks.LastOrDefault(t => t.PointsRequired <= points);
            if (rank == null) return null;
            var nextRank = orderedRanks.FirstOrDefault(t => t.PointsRequired > points);
            return new QuizRankImpl()
            {
                Name = rank.Name,
                PointsRequired = rank.PointsRequired,
                NextRank = nextRank == null ? null : GetRankInternal(nextRank.PointsRequired, orderedRanks)
            };
        }
    }
}
