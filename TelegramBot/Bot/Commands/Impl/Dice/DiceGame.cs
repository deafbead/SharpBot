using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Dice
{
    class DiceGame
    {
        private int _initialDiceCount;

        public DiceGame(int initialDiceCount = 5)
        {
            _initialDiceCount = initialDiceCount;
        }

        public IList<DicePlayer> Players { get; } = new List<DicePlayer>();

        private DicePlayer Winner { get; set; }

        public DiceBet Bet { get; set; }

        //public async Task Begin()
        //{
            //do
            //{
            //    Winner = null;
            //    ResetDices();
            //    RollDices();

            //    do
            //    {
            //        foreach (var player in Players)
            //        {
            //            var result = await player.GetBet();
            //            ProcessResult(result);
            //            if (Winner != null) break;
            //        }
            //    } while (Winner == null);

            //    foreach (var player in Players.Where(p => p != Winner && p.Dices.Any()))
            //    {
            //        player.Dices.RemoveAt(0);
            //    }

            //    Players.RemoveAll(t => t.Dices.Count == 0);

            //} while (Players.Count > 1);
        //}

        //private void ProcessResult(IDiceResult result)
        //{
        //    var betResult = result as DiceBet;
        //    if (betResult != null)
        //    {
        //        Bet = betResult;
        //    }

        //    var hit = result as DiceHit;
        //    if (hit != null)
        //    {
        //        Winner = GetWinner(hit);
        //    }
        //}

        //private DicePlayer GetWinner(DiceHit hit)
        //{
        //    switch (hit.Result)
        //    {
        //        case DiceHitResult.True:
        //            return Bet.IsExact(this) ? hit.Player : Bet.Player;
        //        case DiceHitResult.LiarLiarPantsOnFire:
        //            return Bet.IsAtLeast(this) ? Bet.Player : hit.Player;
        //    }
        //    throw new ArgumentOutOfRangeException();
        //}

        private void ResetDices()
        {
            foreach (var player in Players)
            {
                player.Dices.Clear();
                for (int i = 0; i < _initialDiceCount; i++)
                {
                    player.Dices.Add(new Games.Dices.Dice());
                }
            }
        }

        private void RollDices()
        {
            foreach (var player in Players)
            {
                player.Roll();
            }
        }
    }
}
