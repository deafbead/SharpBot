using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Bot.Games.Cards;

namespace TelegramBot.Bot.Commands.Impl.Ochko
{
    static class OchkoValue
    {
        public static int Get(CardValue value)
        {
            switch (value)
            {
                case CardValue.Ace:
                    return 11;
                case CardValue.Jack:
                    return 2;
                case CardValue.Queen:
                    return 3;
                case CardValue.King:
                    return 4;
                case CardValue.Null:
                    throw new ArgumentException("value can not be Null", nameof(value));
                default:
                    return (int)value;
            }
        }
    }
}
