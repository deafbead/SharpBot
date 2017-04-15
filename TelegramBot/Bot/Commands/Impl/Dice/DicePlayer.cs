using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.API.Models;
using TelegramBot.Bot.Replies;

namespace TelegramBot.Bot.Commands.Dice
{
    class DicePlayer
    {
        public DicePlayer(User user, long chatId)
        {
            User = user;
            ChatId = chatId;
        }

        public User User { get; set; }

        public long ChatId { get; set; }

        public IList<Games.Dices.Dice> Dices { get; set; } = new List<Games.Dices.Dice>();

        public IList<int> Roll()
        {
            return Dices.Select(d => d.Roll()).ToList();
        }

        public string Treat
        {
            get { return User.FirstName + " " + User.LastName; }
        }

        public TextReply Message(string text)
        {
            return new TextReply(ChatId, text);
        }
    }
}

