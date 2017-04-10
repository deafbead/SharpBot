using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace TelegramBot.Bot.Commands.Quiz.Ranks
{
    class QuizRanksProvider : IQuizRanksProvider
    {
        private readonly ISession _session;

        public QuizRanksProvider(ISession session)
        {
            _session = session;
        }

        public IQuizRank GetRank(int points)
        {
            var ranks = _session.QueryOver<QuizRankDTO>().List().OrderBy(t => t.PointsRequired).ToList();
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
