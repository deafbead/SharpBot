using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Bot.Commands.Ochko;
using TelegramBot.Bot.Games.Cards;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Impl.Ochko
{
    class OchkoGame
    {
        public IDeck Deck { get; private set; }

        public int Bank { get; set; }

        public OchkoPlayer Banker { get; }
        public IEnumerable<OchkoPlayer> NonBankers => Players.Where(t => !ReferenceEquals(t, Banker));
        public IList<OchkoPlayer> Players { get; } = new List<OchkoPlayer>();

        public async Task BeginGame()
        {
            Deck = Decks.Standard36;
            Deck.Cards.Shuffle();

            Bank = 10;

            foreach (var player in Players)
            {
                GiveRandomCard(player);
            }

            Bank = Banker.Bank;
            Banker.Bank = 0;

            foreach (var player in NonBankers)
            {
                await player.AwaitBet();
            }


            List<OchkoPlayer> survivors = new List<OchkoPlayer>();
            foreach (var player in NonBankers)
            {
                while (await player.TakesCard())
                {
                    GiveRandomCard(player);
                    int sum = HandSum(player);
                    if (sum <= 21)
                    {
                        survivors.Add(player);
                    }
                    else
                    {
                        Bank += player.Bet;
                        player.Bet = 0;
                        player.IsAlive = false;
                        break;
                    }
                }
            }

            while (await Banker.TakesCard())
            {
                GiveRandomCard(Banker);

            }

            var openCard = PickCard();
            bool gameOver;
            do
            {
                var player = NextPlayer();
                var bet = await player.AwaitBet();


            } while (!gameOver);
            


        }

        private void GiveRandomCard(OchkoPlayer player)
        {
            player.Hand.Add(PickCard());
        }

        private Card PickCard()
        {
            var card = Deck.Cards.PickRandom();
            Deck.Cards.Remove(card);
            return card;
        }

        private int _currentPlayerIndex = -1;
        private OchkoPlayer NextPlayer()
        {
            _currentPlayerIndex++;
            if (_currentPlayerIndex == Players.Count)
            {
                _currentPlayerIndex = 0;
            }
            return Players[_currentPlayerIndex];
        }

        private int HandSum(OchkoPlayer player)
        {
            return player.Hand.Select(c => OchkoValue.Get(c.Value)).Sum();
        }
    }
}
