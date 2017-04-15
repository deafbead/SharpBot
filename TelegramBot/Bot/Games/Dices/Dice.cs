using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Games.Dices
{
    class Dice
    {
        private static Random _random = new Random();

        public int Min { get; }
        public int Max { get; }

        public Dice(int min=1, int max=6)
        {
            Min = min;
            Max = max;
        }

        public int Roll()
        {
            Value = _random.Next(Min, Max + 1);
            return Value;
        }

        public int Value { get; private set; }
    }
}
