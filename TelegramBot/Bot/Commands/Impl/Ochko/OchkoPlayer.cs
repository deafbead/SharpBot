using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.API.Models;
using TelegramBot.Bot.Games.Cards;

namespace TelegramBot.Bot.Commands.Ochko
{
    class OchkoPlayer
    {
        public User User { get; }

        public IList<Card> Hand { get; } = new List<Card>();

        public int Bank { get; set; }

        public int Bet { get; set; }

        public bool IsAlive { get; set; }

        public OchkoPlayer(User user)
        {
            User = user;
        }

        public Task AwaitBet()
        {
            
        }

        public Task<bool> TakesCard()
        {
            if (Hand.Count == 5) return Task.FromResult(false);
        }
    }
}
