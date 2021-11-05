using System;
using System.Collections.Generic;
using System.Text;
namespace OmahaMTG.Challenge.SolitaireChallenge
{
    /// <summary>
    /// Represents the rank of a card.
    /// </summary>
    public enum Rank
    {
        None = -1,
        Ace = 0,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
    /// <summary>
    /// Represents the suit of a card.
    /// </summary>
    public enum Suit
    {
        None = -1,
        Clubs = 0,
        Diamonds,
        Spades,
        Hearts
    }
    /// <summary>
    /// Represents a card in solitaire.
    /// </summary>
    public sealed class Card
    {
        /// <summary>
        /// A blank card that represents an empty spot.
        /// </summary>
        public static readonly Card None = new Card();
        internal int rank, suit, color, odd, value;
        internal bool up;

        private Card()
        {
            value = -1;
            rank = -1;
            suit = -1;
            up = true;
            color = 0;
            odd = 0;
        }
        internal Card(int hash)
        {
            value = hash;
            rank = hash % 13;
            suit = hash / 13;
            up = false;
            color = suit & 1;
            odd = rank & 1;
        }
        internal Card(Card c)
        {
            value = c.value;
            rank = c.rank;
            suit = c.suit;
            up = c.up;
            color = c.color;
            odd = c.odd;
        }
        internal void Set(int hash)
        {
            value = hash;
            rank = hash % 13;
            suit = hash / 13;
            up = false;
            color = suit & 1;
            odd = rank & 1;
        }
        /// <summary>
        /// The rank of this card.
        /// </summary>
        public Rank Rank
        {
            get { return (Rank)rank; }
        }
        /// <summary>
        /// The suit of this card.
        /// </summary>
        public Suit Suit
        {
            get { return (Suit)suit; }
        }
        /// <summary>
        /// Returns true if this card is turned over face up.
        /// </summary>
        public bool IsFaceUp
        {
            get { return up; }
        }
        /// <summary>
        /// Returns true if this cards' suit is red(Diamonds,Hearts).
        /// </summary>
        public bool IsRed
        {
            get { return color == 1; }
        }
        /// <summary>
        /// Returns true if this cards' suit is black(Clubs,Spades).
        /// </summary>
        public bool IsBlack
        {
            get { return color == 0; }
        }
        /// <summary>
        /// Returns true if this cards' rank is odd.
        /// </summary>
        public bool IsOdd
        {
            get { return odd == 0; }
        }
        /// <summary>
        /// Returns true if this cards' rank is even.
        /// </summary>
        public bool IsEven
        {
            get { return odd == 1; }
        }
        /// <summary>
        /// Returns a string representing this cards' rank, suit, and if it is face up. (+/-)RS.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ranks = "A23456789TJQK";
            string suits = "CDSH";
            return (up ? "+" : "-") + ((rank < 0 || suit < 0) ? "XX" : "" + ranks[rank] + suits[suit]);
        }
        /// <summary>
        /// Returns a value representing this card.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value;
        }
        /// <summary>
        /// Returns true if the object is a card and has the same rank and suit.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == typeof(Card) && ((Card)obj).value == value;
        }
        /// <summary>
        /// Returns true if the card has the same rank and suit.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool Equals(Card card)
        {
            return card != null && card.value == value;
        }
    }
}