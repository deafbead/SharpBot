using System.Collections.Generic;
using System.Linq;

namespace TelegramBot.Bot.Commands.Dice
{
    class DiceBet : IDiceResult
    {
        public int Count { get; }
        public int Value { get; }

        public DicePlayer Player { get; }

        public DiceBet(DicePlayer player, int count, int value)
        {
            Player = player;
            Count = count;
            Value = value;
        }

        public bool IsExact(IEnumerable<DicePlayer> players)
        {
            return players.SelectMany(p => p.Dices).Count(d => d.Value == Value) == Count;
        }

        public bool IsAtLeast(IEnumerable<DicePlayer> players)
        {
            return players.SelectMany(p => p.Dices).Count(d => d.Value == Value) >= Count;
        }
    }

    enum DiceHitResult
    {
        True,
        LiarLiarPantsOnFire
    }
    class DiceHit : IDiceResult
    {
        public DiceHit(DicePlayer player, DiceHitResult result)
        {
            Result = result;
            Player = player;
        }

        public DicePlayer Player { get; }

        public DiceHitResult Result { get; }
    }

    interface IDiceResult
    {
        DicePlayer Player { get; }
    }
}
