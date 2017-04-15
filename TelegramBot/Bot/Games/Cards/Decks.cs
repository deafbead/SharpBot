using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Games.Cards
{
    interface IDeck
    {
        IList<Card> Cards { get; }
    }

    class Deck : IDeck
    {
        public IList<Card> Cards { get; } = new List<Card>();
    }

    static class Decks
    {
        public static IDeck NoJokers52
        {
            get
            {
                var deck = new Deck();
                for (int value = (int)CardValue.Ace; value <= (int)CardValue.King; value++)
                {
                    for (int suit = 1; suit <= 4; suit++)
                    {
                        deck.Cards.Add(new Card((CardSuit)suit, (CardValue)value));
                    }
                }
                return deck;
            }
        }

        public static IDeck Standard36
        {
            get
            {
                var deck = new Deck();
                for (int value = (int)CardValue.Six; value <= (int)CardValue.King; value++)
                {
                    for (int suit = 1; suit <= 4; suit++)
                    {
                        deck.Cards.Add(new Card((CardSuit)suit, (CardValue)value));
                    }
                }
                for (int suit = 1; suit <= 4; suit++)
                {
                    deck.Cards.Add(new Card((CardSuit)suit, CardValue.Ace));
                }
                return deck;
            }
        }
    }
}
