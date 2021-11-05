using System;
using System.Collections.Generic;
using System.Text;
namespace OmahaMTG.Challenge.SolitaireChallenge
{
    /// <summary>
    /// Represents each of the piles in a game of solitaire.
    /// </summary>
    public enum PileType
    {
        Waste = 0,
        Tableau1,
        Tableau2,
        Tableau3,
        Tableau4,
        Tableau5,
        Tableau6,
        Tableau7,
        Stock,
        Foundation1,
        Foundation2,
        Foundation3,
        Foundation4
    }
    /// <summary>
    /// Represents a Pile of cards in a game of Solitaire.
    /// </summary>
    public sealed class Pile
    {
        internal int size, top;
        internal Card[] cards;
        private PileType type;

        private Pile()
        {
            cards = null;
            size = 0;
            top = -1;
            type = (PileType)(-1);
        }
        internal Pile(int max,PileType pileType)
        {
            cards = new Card[max];
            size = 0;
            top = -1;
            type = pileType;
        }

        /// <summary>
        /// Returns the number of face up cards on this pile.
        /// </summary>
        public int FaceUpCount
        {
            get { return top >= 0 ? size - top : 0; }
        }
        /// <summary>
        /// Returns true if this pile has no cards in it.
        /// </summary>
        public bool IsEmpty
        {
            get { return size == 0; }
        }
        /// <summary>
        /// The top most card on this pile regardless if it is turned over or not or Card.None if there are no cards.
        /// </summary>
        public Card TopCard
        {
            get { return size > 0 ? cards[size - 1] : Card.None; }
        }
        /// <summary>
        /// The top most cards' rank on this pile regardless if it is turned over or not or Rank.None if there are no cards.
        /// </summary>
        public Rank TopRank
        {
            get { return size > 0 ? (Rank)cards[size - 1].rank : Rank.None; }
        }
        /// <summary>
        /// The top most cards' suit on this pile regardless if it is turned over or not or Suit.None if there are no cards.
        /// </summary>
        public Suit TopSuit
        {
            get { return size > 0 ? (Suit)cards[size - 1].suit : Suit.None; }
        }
        /// <summary>
        /// The top most cards' index on this pile regardless if it is turned over or not or -1 if there are no cards.
        /// </summary>
        public int TopIndex
        {
            get { return size - 1; }
        }
        /// <summary>
        /// The highest ranked card that is turned over on this pile or Card.None if there are no cards or no turned over cards.
        /// </summary>
        public Card HighCard
        {
            get { return top >= 0 ? cards[top] : Card.None; }
        }
        /// <summary>
        /// The highest ranked cards' rank that is turned over on this pile or Rank.None if there are no cards or no turned over cards.
        /// </summary>
        public Rank HighRank
        {
            get { return top >= 0 ? (Rank)cards[top].rank : Rank.None; }
        }
        /// <summary>
        /// The highest ranked cards' suit that is turned over on this pile or Suit.None if there are no cards or no turned over cards.
        /// </summary>
        public Suit HighSuit
        {
            get { return top >= 0 ? (Suit)cards[top].suit : Suit.None; }
        }
        /// <summary>
        /// The highest ranked cards' index that is turned over on this pile or -1 if there are no cards or no turned over cards.
        /// </summary>
        public int HighIndex
        {
            get { return top; }
        }
        /// <summary>
        /// The lowest ranked card that is turned over on this pile or Card.None if there are no cards or no turned over cards.
        /// </summary>
        public Card LowCard
        {
            get { return top >= 0 ? cards[size - 1] : Card.None; }
        }
        /// <summary>
        /// The lowest ranked cards' rank that is turned over on this pile or Rank.None if there are no cards or no turned over cards.
        /// </summary>
        public Rank LowRank
        {
            get { return top >= 0 ? (Rank)cards[size - 1].rank : Rank.None; }
        }
        /// <summary>
        /// The lowest ranked cards' suit that is turned over on this pile or Suit.None if there are no cards or no turned over cards.
        /// </summary>
        public Suit LowSuit
        {
            get { return top >= 0 ? (Suit)cards[size - 1].suit : Suit.None; }
        }
        /// <summary>
        /// The lowest ranked cards' index that is turned over on this pile or -1 if there are no cards or no turned over cards.
        /// </summary>
        public int LowIndex
        {
            get { return top >= 0 ? size - 1 : -1; }
        }
        /// <summary>
        /// The PileType of this pile.
        /// </summary>
        public PileType Type
        {
            get { return type; }
        }
        /// <summary>
        /// Returns the count of cards in this pile.
        /// </summary>
        public int Size
        {
            get { return size; }
        }
        /// <summary>
        /// Returns the index of the specified card in this pile or -1 if not found.
        /// </summary>
        /// <param name="card">The card to be found.</param>
        /// <returns>Index of the specified card or -1 if not found.</returns>
        public int IndexOf(Card card)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (card == cards[i])
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Returns the card at the specified index or Card.None if the index is out of bounds. 0 would be the bottom most card in the pile.
        /// </summary>
        /// <param name="index">The index of the card to be returned.</param>
        /// <returns></returns>
        public Card this[int index]
        {
            get
            {
                if (index >= 0 && index < size)
                {
                    return cards[index];
                }
                return Card.None;
            }
        }
        /// <summary>
        /// Returns the card at the specified index or Card.None if the index is out of bounds. Bounds [0,size-1] = [bottom,top] most card in the pile.
        /// </summary>
        /// <param name="index">The index of the card to be returned.</param>
        /// <returns></returns>
        public Card CardAt(int index)
        {
            if (index >= 0 && index < size)
            {
                return cards[index];
            }
            return Card.None;
        }
        /// <summary>
        /// Returns the card at the specified index or Card.None if the index is out of bounds from the top of the pile. Bounds [0,size-1] = [top,bottom] most card in the pile.
        /// </summary>
        /// <param name="index">The index of the card to be returned.</param>
        /// <returns></returns>
        public Card CardFromTop(int index)
        {
            if (index >= 0 && index < size)
            {
                return cards[size - index - 1];
            }
            return Card.None;
        }
        /// <summary>
        /// A string representation of this pile listing out all cards from top to bottom going left to right.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(4 * size);
            for (int i = size - 1; i >= 0; i--)
            {
                sb.Append(cards[i].ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// Returns true if the specified object is a pile and it has the same cards.
        /// </summary>
        /// <param name="obj">The object to be checked for equality.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Pile)) { return false; }
            Pile pile = (Pile)obj;
            if (pile.size != size) { return false; }
            for (int i = 0; i < size; i++)
            {
                if (cards[i].value != pile.cards[i].value)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Returns true if the specified object is a pile and it has the same cards.
        /// </summary>
        /// <param name="pile">The pile to be checked for equality.</param>
        /// <returns></returns>
        public bool Equals(Pile pile)
        {
            if (pile.size != size) { return false; }
            for (int i = 0; i < size; i++)
            {
                if (cards[i].value != pile.cards[i].value)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Returns the hashcode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        internal void Clear()
        {
            size = 0;
        }
        internal void Add(Card c)
        {
            cards[size++] = c;
            c.up = false;
        }
        internal void Flip()
        {
            if (size > 0)
            {
                Card temp = cards[size - 1];
                if ((temp.up = !temp.up))
                {
                    top = size - 1;
                    return;
                }
                top = -1;
            }
        }
        internal void Remove(Pile to, int count)
        {
            if (to.top < 0) { to.top = to.size; }
            for (int i = size - count; i < size; i++)
            {
                to.cards[to.size++] = cards[i];
            }
            size -= count;
            if (top >= size) { top = -1; }
        }
        internal void Remove(Pile to)
        {
            if (to.top < 0) { to.top = to.size; }
            to.cards[to.size++] = cards[--size];
            if (top == size) { top = -1; }
        }
        internal void RemoveTop(Pile to, int count)
        {
            if (count == 1)
            {
                size--;
                cards[size].up = !cards[size].up;
                to.cards[to.size++] = cards[size];
                return;
            }
            size -= count;
            for (int i = size + count - 1; i >= size; i--)
            {
                cards[i].up = !cards[i].up;
                to.cards[to.size++] = cards[i];
            }
        }
    }
}