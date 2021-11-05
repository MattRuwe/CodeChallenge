using System;
using System.Collections.Generic;
using System.Text;
namespace OmahaMTG.Challenge.SolitaireChallenge
{
    /// <summary>
    /// Represents a move of cards from one pile to another in a game of solitaire.
    /// </summary>
    public sealed class Move
    {
        internal int from, to, cards, val;
        internal Move next, prev;

        /// <summary>
        /// Initialize a new instance of the Move class with the specified from and to pile and the number of cards to be moved.
        /// </summary>
        /// <param name="fromPile">The pile containing the cards to be moved.</param>
        /// <param name="toPile">The pile where the cards are being moved to.</param>
        /// <param name="cardsMoved">The number of cards being moved.</param>
        public Move(PileType fromPile, PileType toPile, int cardsMoved) : this((int)fromPile, (int)toPile, cardsMoved, 0) { }
        /// <summary>
        /// Initialize a new instance of the Move class with the specified from and to pile and the number of cards to be moved.
        /// </summary>
        /// <param name="fromPile">The pile containing the cards to be moved.</param>
        /// <param name="toPile">The pile to move the cards to.</param>
        /// <param name="cardsMoved">The number of cards being moved.</param>
        /// <param name="value">An additional value used to differentiate/sort moves.</param>
        public Move(PileType fromPile, PileType toPile, int cardsMoved, int value) : this((int)fromPile, (int)toPile, cardsMoved, value) { }
        internal Move(int fromPile, int toPile, int cardsMoved) : this(fromPile, toPile, cardsMoved, 0) { }
        internal Move(int fromPile, int toPile, int cardsMoved, int value)
        {
            from = fromPile;
            to = toPile;
            cards = cardsMoved;
            val = value;
            next = null;
            prev = null;
        }
        /// <summary>
        /// The pile containing the cards to be moved.
        /// </summary>
        public PileType From
        {
            get { return (PileType)from; }
            set { from = (int)value; }
        }
        /// <summary>
        /// The pile where the cards are being moved to.
        /// </summary>
        public PileType To
        {
            get { return (PileType)to; }
            set { to = (int)value; }
        }
        /// <summary>
        /// The number of cards being moved.
        /// </summary>
        public int CardsMoved
        {
            get { return cards; }
            set { cards = value; }
        }
        /// <summary>
        /// Value used to differentiate/sort moves.
        /// </summary>
        public int Value
        {
            get { return val; }
            set { val = value; }
        }
        /// <summary>
        /// Returns a value used to represent this move.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (val << 13) | (from << 9) | (to << 5) | cards;
        }
        /// <summary>
        /// Returns true if the specified object is a move and its from pile, to pile, and amount of cards being moved are the same.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == typeof(Move) && ((Move)obj).cards == cards && ((Move)obj).from == from && ((Move)obj).to == to;
        }
        /// <summary>
        /// Returns true if the specified moves' from pile, to pile, and amount of cards being moved are the same.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool Equals(Move move)
        {
            return move != null && move.cards == cards && move.from == from && move.to == to;
        }
        /// <summary>
        /// Returns a string representing this move.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (to == from) { sb.Append("[Flip ").Append((PileType)from).Append("]"); }
            else { sb.Append("[").Append((PileType)from).Append(" to ").Append((PileType)to).Append(" moving ").Append(cards).Append("]"); }
            return sb.ToString();
        }
    }
}