using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using TelegramBot.Bot.Games.Score;

namespace TelegramBot.Bot.Games
{
    class UserDTO
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Username { get; set; }

        public virtual IList<ScoreDTO> Scores { get; set; }

        public class Mappings : ClassMap<UserDTO>
        {
            public Mappings()
            {
                Table("users");
                Id(t => t.Id).GeneratedBy.Assigned().Column("id");
                HasMany(t => t.Scores).Cascade.All();
                Map(t => t.FirstName).Column("first_name");
                Map(t => t.LastName).Column("last_name");
                Map(t => t.Username).Column("username");
            }
        }
    }
}
