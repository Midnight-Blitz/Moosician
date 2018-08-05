using System;
using System.Collections.Generic;
using System.Linq;
using NadekoBot.Common;
using NadekoBot.Extensions;

namespace NadekoBot.Modules.Gambling.Common
{
    public class HarrowDeck : HDeck
    {
        protected override void RefillPool()
        {
            cardPool = new List<HCard>(54 * 1); ///A deck of 54, one of it
            for (var j = 1; j < 55; j++)
            {
                for (var i = 1; i < 5; i++)
                {
                    cardPool.Add(new HCard((CardSuit)i, j));
                }
            }
        }
    }

    public class HDeck
    {
        private static readonly Dictionary<int, string> cardNames = new Dictionary<int, string>() {
            { 1, "Ace" },
            { 2, "Two" },
            { 3, "Three" },
            { 4, "Four" },
            { 5, "Five" },
            { 6, "Six" },
            { 7, "Seven" },
            { 8, "Eight" },
            { 9, "Nine" },
            { 10, "Ten" },
            { 11, "Jack" },
            { 12, "Queen" },
            { 13, "King" },
            { 14, "Ace" },
            { 15, "Two" },
            { 16, "Three" },
            { 17, "Four" },
            { 18, "Five" },
            { 19, "Six" },
            { 20, "Seven" },
            { 21, "Eight" },
            { 22, "Nine" },
            { 23, "Ten" },
            { 24, "Jack" },
            { 25, "Queen" },
            { 26, "King" },
            { 27, "Ace" },
            { 28, "Two" },
            { 29, "Three" },
            { 30, "Four" },
            { 31, "Five" },
            { 32, "Six" },
            { 33, "Seven" },
            { 34, "Eight" },
            { 35, "Nine" },
            { 36, "Ten" },
            { 37, "Jack" },
            { 38, "Queen" },
            { 39, "King" },
            { 40, "Seven" },
            { 41, "Ace" },
            { 42, "Two" },
            { 43, "Three" },
            { 44, "Four" },
            { 45, "Five" },
            { 46, "Six" },
            { 47, "Eight" },
            { 48, "Nine" },
            { 49, "Ten" },
            { 50, "Jack" },
            { 51, "Queen" },
            { 52, "King" },
            { 53, "Joker" },
            { 54, "Joker" }
        };
        private static Dictionary<string, Func<List<HCard>, bool>> handValues;

        public enum CardSuit
        {
            Spades = 1,
            Hearts = 2,
            Diamonds = 3,
            Clubs = 4
        }

        public class HCard : IComparable
        {
            public CardSuit Suit { get; } //Delete ASAP
            public int Number { get; }

            public string Name
            {
                get
                {
                    var str = "";

                    if (Number <= 10 && Number > 1)
                    {
                        str += "_" + Number;
                    }
                    else
                    {
                        str += GetName().ToLower();
                    }
                    return str + "_of_" + Suit.ToString().ToLower();
                }
            }

            public HCard(CardSuit s, int cardNum)
            {
                this.Suit = s;
                this.Number = cardNum;
            }

            public string GetName() => cardNames[Number];

            public override string ToString() => cardNames[Number] + " Of " + Suit;

            public int CompareTo(object obj)
            {
                if (!(obj is HCard)) return 0;
                var c = (HCard)obj;
                return this.Number - c.Number;
            }

            public HCard Parse(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new ArgumentNullException(nameof(input));

                if (input.Length != 2
                    || !_numberCharToNumber.TryGetValue(input[0], out var n)
                    || !_suitCharToSuit.TryGetValue(input[1].ToString(), out var s))
                {
                    throw new ArgumentException(nameof(input));
                }

                return new HCard(s, n);
            }

            public string GetEmojiString()
            {
                var str = "";

                str += _regIndicators[this.Number - 1];
                str += _suitToSuitChar[this.Suit];

                return str;
            }
            private readonly string[] _regIndicators = new[]
            {
                "🇦",
                ":two:",
                ":three:",
                ":four:",
                ":five:",
                ":six:",
                ":seven:",
                ":eight:",
                ":nine:",
                ":keycap_ten:",
                "🇯",
                "🇶",
                "🇰"
            };
            private static IReadOnlyDictionary<CardSuit, string> _suitToSuitChar = new Dictionary<CardSuit, string>
            {
                     {CardSuit.Diamonds, "♦"},
                     {CardSuit.Clubs, "♣"},
                     {CardSuit.Spades, "♠"},
                     {CardSuit.Hearts, "♥"},
            };
            private static IReadOnlyDictionary<string, CardSuit> _suitCharToSuit = new Dictionary<string, CardSuit>
            {
                     {"♦", CardSuit.Diamonds },
                     {"d", CardSuit.Diamonds },
                     {"♣", CardSuit.Clubs },
                     {"c", CardSuit.Clubs },
                     {"♠", CardSuit.Spades },
                     {"s", CardSuit.Spades },
                     {"♥", CardSuit.Hearts },
                     {"h", CardSuit.Hearts },
            };
            private static IReadOnlyDictionary<char, int> _numberCharToNumber = new Dictionary<char, int>()
            {
                     {'a', 1 },
                     {'2', 2 },
                     {'3', 3 },
                     {'4', 4 },
                     {'5', 5 },
                     {'6', 6 },
                     {'7', 7 },
                     {'8', 8 },
                     {'9', 9 },
                     {'t', 10 },
                     {'j', 11 },
                     {'q', 12 },
                     {'k', 13 },
            };
        }
        protected List<HCard> cardPool;
        public List<HCard> CardPool
        {
            get { return cardPool; }
            set { cardPool = value; }
        }

        /// <summary>
        /// Creates a new instance of the BlackJackGame, this allows you to create multiple games running at one time.
        /// </summary>
        public HDeck()
        {
            RefillPool();
        }
        static HDeck()
        {
            InitHandValues();
        }
        /// <summary>
        /// Restart the game of blackjack. It will only refill the pool for now. Probably wont be used, unless you want to have only 1 bjg running at one time,
        /// then you will restart the same game every time.
        /// </summary>
        public void Restart() => RefillPool();

        /// <summary>
        /// Removes all cards from the pool and refills the pool with all of the possible cards. NOTE: I think this is too expensive.
        /// We should probably make it so it copies another premade list with all the cards, or something.
        /// </summary>
        protected virtual void RefillPool()
        {
            cardPool = new List<HCard>(52);
            //foreach suit
            for (var j = 1; j < 14; j++)
            {
                // and number
                for (var i = 1; i < 5; i++)
                {
                    //generate a card of that suit and number and add it to the pool

                    // the pool will go from ace of spades,hears,diamonds,clubs all the way to the king of spades. hearts, ...
                    cardPool.Add(new HCard((CardSuit)i, j));
                }
            }
        }
        private Random r = new NadekoRandom();
        /// <summary>
        /// Take a card from the pool, you either take it from the top if the deck is shuffled, or from a random place if the deck is in the default order.
        /// </summary>
        /// <returns>A card from the pool</returns>
        public HCard Draw()
        {
            if (CardPool.Count == 0)
                Restart();
            //you can either do this if your deck is not shuffled

            var num = r.Next(0, cardPool.Count);
            var c = cardPool[num];
            cardPool.RemoveAt(num);
            return c;

            // if you want to shuffle when you fill, then take the first one
            /*
            Card c = cardPool[0];
            cardPool.RemoveAt(0);
            return c;
            */
        }
        /// <summary>
        /// Shuffles the deck. Use this if you want to take cards from the top of the deck, instead of randomly. See DrawACard method.
        /// </summary>
        private void Shuffle()
        {
            if (cardPool.Count <= 1) return;
            var orderedPool = cardPool.Shuffle();
            cardPool = cardPool as List<HCard> ?? orderedPool.ToList();
        }
        public override string ToString() => string.Concat(cardPool.Select(c => c.ToString())) + Environment.NewLine;

        private static void InitHandValues()
        {
            Func<List<HCard>, bool> hasPair =
                                  cards => cards.GroupBy(card => card.Number)
                                                .Count(group => group.Count() == 2) == 1;
            Func<List<HCard>, bool> isPair =
                                  cards => cards.GroupBy(card => card.Number)
                                                .Count(group => group.Count() == 3) == 0
                                           && hasPair(cards);

            Func<List<HCard>, bool> isTwoPair =
                                  cards => cards.GroupBy(card => card.Number)
                                                .Count(group => group.Count() == 2) == 2;

            Func<List<HCard>, bool> isStraight =
                cards =>
                {
                    if (cards.GroupBy(card => card.Number).Count() != cards.Count())
                        return false;
                    var toReturn = (cards.Max(card => (int)card.Number)
                                        - cards.Min(card => (int)card.Number) == 4);
                    if (toReturn || cards.All(c => c.Number != 1)) return toReturn;

                    var newCards = cards.Select(c => c.Number == 1 ? new HCard(c.Suit, 14) : c);
                    return (newCards.Max(card => (int)card.Number)
                            - newCards.Min(card => (int)card.Number) == 4);
                };

            Func<List<HCard>, bool> hasThreeOfKind =
                                  cards => cards.GroupBy(card => card.Number)
                                                .Any(group => group.Count() == 3);

            Func<List<HCard>, bool> isThreeOfKind =
                                  cards => hasThreeOfKind(cards) && !hasPair(cards);

            Func<List<HCard>, bool> isFlush =
                                  cards => cards.GroupBy(card => card.Suit).Count() == 1;

            Func<List<HCard>, bool> isFourOfKind =
                                  cards => cards.GroupBy(card => card.Number)
                                                .Any(group => group.Count() == 4);

            Func<List<HCard>, bool> isFullHouse =
                                  cards => hasPair(cards) && hasThreeOfKind(cards);

            Func<List<HCard>, bool> hasStraightFlush =
                                  cards => isFlush(cards) && isStraight(cards);

            Func<List<HCard>, bool> isRoyalFlush =
                                  cards => cards.Min(card => card.Number) == 1 &&
                                           cards.Max(card => card.Number) == 13
                                           && hasStraightFlush(cards);

            Func<List<HCard>, bool> isStraightFlush =
                                  cards => hasStraightFlush(cards) && !isRoyalFlush(cards);

            handValues = new Dictionary<string, Func<List<HCard>, bool>>
            {
                { "Royal Flush", isRoyalFlush },
                { "Straight Flush", isStraightFlush },
                { "Four Of A Kind", isFourOfKind },
                { "Full House", isFullHouse },
                { "Flush", isFlush },
                { "Straight", isStraight },
                { "Three Of A Kind", isThreeOfKind },
                { "Two Pairs", isTwoPair },
                { "A Pair", isPair }
            };
        }

        public static string GetHandValue(List<HCard> cards)
        {
            if (handValues == null)
                InitHandValues();
            foreach (var kvp in handValues.Where(x => x.Value(cards)))
            {
                return kvp.Key;
            }
            return "High card " + (cards.FirstOrDefault(c => c.Number == 1)?.GetName() ?? cards.Max().GetName());
        }
    }
}