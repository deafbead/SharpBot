using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace TelegramBot.Bot.Games.Score
{
    class ScoreDTO
    {
        public virtual int Id { get; set; }

        public virtual UserDTO User { get; set; }

        public virtual int Points { get; set; }

        public virtual string GameId { get; set; }

        public class Mappings : ClassMap<ScoreDTO>
        {
            public Mappings()
            {
                Table("scores");
                Id(t => t.Id).Column("id");
                References(t => t.User).Column("user_id");
                Map(t => t.Points).Column("score");
                Map(t => t.GameId).Column("game_id");
            }
        }
    }
}
