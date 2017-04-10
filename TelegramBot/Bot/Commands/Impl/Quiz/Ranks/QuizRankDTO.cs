using FluentNHibernate.Mapping;

namespace TelegramBot.Bot.Commands.Quiz.Ranks
{
    class QuizRankDTO
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int PointsRequired { get; set; }

        public class Mappings : ClassMap<QuizRankDTO>
        {
            public Mappings()
            {
                Table("quiz_ranks");
                Id(t => t.Id).GeneratedBy.Increment();
                Map(t => t.Name).Column("name");
                Map(t => t.PointsRequired).Column("required_points");
            }
        }

    }
}
