using System;
using System.Text;
[assembly: CLSCompliant(true)]
namespace OmahaMTG.Challenge.SolitaireChallenge
{
    /// <summary>
    /// Represents the classic card game of Solitaire.
    /// </summary>
    public sealed class Solitaire
    {
        private Card[] cards;
        private readonly Pile[] piles;
        private readonly int[] foundationOrder;
        private readonly MoveList moves;

        private readonly MoveList movesMade;
        private readonly int drawCount;

        private int redMin, blackMin, rounds, foundationCount;
        private readonly Rand random;
        private Func<Solitaire, PileType, PileType, int, int> moveValues;

        /// <summary>
        /// Initialize a new instance of the Solitaire class with a draw count of 1.
        /// </summary>
        public Solitaire() : this(1) { }
        /// <summary>
        /// Initialize a new instance of the Solitaire class with the specified draw count.
        /// </summary>
        /// <param name="drawCount">The number of cards that are turned over on each draw. Value can be 1, 2, or 3.</param>
        public Solitaire(int drawCount)
        {
            if (drawCount < 1 || drawCount > 3) { throw new ArgumentException("Draw Count can only be 1, 2, or 3."); }
            random = new Rand();
            cards = new Card[52];
            piles = new Pile[13];
            moveValues = delegate(Solitaire s, PileType from, PileType to, int Cards) { return 0; };
            foundationOrder = new int[] { (int)PileType.Foundation1, (int)PileType.Foundation2, (int)PileType.Foundation3, (int)PileType.Foundation4 };
            moves = new MoveList();
            movesMade = new MoveList();
            this.drawCount = drawCount;
            for (int i = 0; i < 52; i++)
            {
                cards[i] = new Card(i);
            }
            piles[(int)PileType.Stock] = new Pile(24, PileType.Stock);
            piles[(int)PileType.Waste] = new Pile(24, PileType.Waste);
            for (int i = (int)PileType.Tableau1, j = 0; i <= (int)PileType.Tableau7; i++, j++)
            {
                piles[i] = new Pile(13 + j, (PileType)i);
            }
            for (int i = (int)PileType.Foundation1; i <= (int)PileType.Foundation4; i++)
            {
                piles[i] = new Pile(13, (PileType)i);
            }
            Reset();
        }
        /// <summary>
        /// Returns a clone of this Solitaire object in its current state.
        /// </summary>
        /// <returns>A new Solitaire object.</returns>
        public Solitaire Clone()
        {
            Solitaire s = new Solitaire(drawCount);
            s.CardSet = CardSet;
            s.MakeMove(movesMade, false);
            return s;
        }
        /// <summary>
        /// Resets the game back to its initial state.
        /// </summary>
        public void Reset()
        {
            redMin = -1;
            blackMin = -1;
            rounds = 0;
            foundationCount = 0;
            for (int i = 0; i < 13; i++)
            {
                piles[i].Clear();
            }
            for (int j = (int)PileType.Tableau1, i = 0; j <= (int)PileType.Tableau7; j++)
            {
                for (int k = j; k <= (int)PileType.Tableau7; k++, i++)
                {
                    piles[k].Add(cards[i]);
                }
            }
            for (int i = 51; i >= 28; i--)
            {
                piles[(int)PileType.Stock].Add(cards[i]);
            }
            //turn over the last card on all of the base piles
            for (int j = (int)PileType.Tableau1; j <= (int)PileType.Tableau7; j++)
            {
                piles[j].Flip();
            }
            movesMade.Clear();
            UpdateMoves();
        }
        /// <summary>
        /// A list of moves that are available to be played based on the current state of the game.
        /// The list will be returned sorted descending based on the values of the moves.
        /// When a king can be played to multiple open spots only the first spot will be returned in this list.
        /// </summary>
        public MoveList MovesAvailable
        {
            get { return moves; }
        }
        /// <summary>
        /// A clone of the list of moves that have been made so far in the game.
        /// </summary>
        public MoveList MovesMade
        {
            get { return movesMade.Clone(); }
        }
        /// <summary>
        /// Function used to determine the values of moves in the list of moves available.
        /// int Func(Solitaire solitaire, PileType fromPile, PileType toPile,int cardsMoved)
        /// solitaire is this solitaire object.
        /// fromPile is the pile the card(s) is being moved from.
        /// toPile is the pile the card(s) is being moved to.
        /// cardsMoved is the number of cards being moved.
        /// returns an integer value of the move from the fromPile to the toPile.
        /// </summary>
        public Func<Solitaire, PileType, PileType, int, int> MoveValueCalc
        {
            get { return moveValues; }
            set
            {
                if (value != null)
                {
                    moveValues = value;
                }
            }
        }
        /// <summary>
        /// The number of actual moves made not including the "Turn", which is moving all cards in the waste pile back to the stock pile.
        /// </summary>
        public int TotalMovesMade
        {
            get { return movesMade.size - rounds; }
        }
        /// <summary>
        /// The number of times the waste pile has been moved back to the stock pile. Also know as the "Turn".
        /// </summary>
        public int RoundsPlayed
        {
            get { return rounds; }
        }
        /// <summary>
        /// The total number of cards in the foundation piles.
        /// </summary>
        public int FoundationCount
        {
            get { return foundationCount; }
        }
        /// <summary>
        /// Returns the minimum rank in the foundation piles for hearts and diamonds.
        /// </summary>
        public Rank FoundationMinRankRed
        {
            get { return (Rank)redMin; }
        }
        /// <summary>
        /// Returns the minimum rank in the foundation piles for spades and clubs.
        /// </summary>
        public Rank FoundationMinRankBlack
        {
            get { return (Rank)blackMin; }
        }
        public bool CanBeMovedToFoundation(Card c)
        {
            Card t = piles[foundationOrder[c.suit]].TopCard;
            return c.rank - t.rank == 1;
        }
        /// <summary>
        /// Returns true if this game of solitaire has been won, meaning all cards are in their foundation piles.
        /// </summary>
        /// <returns>True if the game is finished.</returns>
        public bool IsWon
        {
            get { return foundationCount == 52; }
        }
        /// <summary>
        /// The amount of cards that are drawn when moving from the stock to the waste pile.
        /// </summary>
        public int DrawCount
        {
            get { return drawCount; }
        }
        /// <summary>
        /// Returns the specified pile from the solitaire game.
        /// </summary>
        /// <param name="pile">The type of pile to be returned.</param>
        /// <returns>A pile in the solitaire game or null if the parameter is out of range.</returns>
        public Pile Pile(PileType pile)
        {
            int p = (int)pile;
            return p >= 0 && p < 13 ? piles[p] : null;
        }
        /// <summary>
        /// Deals a random solitaire game and returns the seed for this deal.
        /// </summary>
        public int Shuffle()
        {
            int seed = random.Next;
            random.SetSeed(seed);
            Shuffle(seed);
            return seed;
        }
        /// <summary>
        /// Deals a solitaire game based on the specified seed.
        /// </summary>
        public void Shuffle(int seed)
        {
            Card temp;
            random.SetSeed(seed);
            for (int i = 0; i < 52; i++)
            {
                cards[i].Set(i);
            }
            for (int i = 0; i < 1000; i++)
            {
                int k = random.Next % 52;
                int j = random.Next % 52;
                temp = cards[k];
                cards[k] = cards[j];
                cards[j] = temp;
            }
            Reset();
        }
        /// <summary>
        /// Validates and makes the specified move.
        /// </summary>
        /// <param name="move">The move to be made.</param>
        /// <param name="validate">Validation can be turned off for faster performance.</param>
        /// <returns>Returns true if the move is valid. Will always be true when validation is turned off.</returns>
        public bool MakeMove(Move move, bool validate = true)
        {
            if (validate && (move == null || !ValidateMove(move.from, move.to, move.cards))) { return false; }
            try
            {
                movesMade.AddLast(move.from, move.to, move.cards, 0);
                if (move.from != move.to)
                {
                    if (move.to == (int)PileType.Stock || move.to == (int)PileType.Waste)
                    {
                        piles[move.from].RemoveTop(piles[move.to], move.cards);
                        if (move.to == (int)PileType.Stock) { rounds++; }
                    }
                    else if (move.cards == 1)
                    {
                        piles[move.from].Remove(piles[move.to]);
                        if (move.to >= (int)PileType.Foundation1)
                        {
                            foundationCount++;
                            SetFoundationMin();
                        }
                        else if (move.from >= (int)PileType.Foundation1)
                        {
                            foundationCount--;
                            SetFoundationMin();
                        }
                    }
                    else
                    {
                        piles[move.from].Remove(piles[move.to], move.cards);
                    }
                }
                else
                {
                    piles[move.from].Flip();
                }
                UpdateMoves();
                return true;
            }
            catch
            {
                string moveVal = "NULL";
                if (move != null)
                {
                    moveVal = move.ToString();
                }
                throw new Exception("An error occured trying to apply move: " + moveVal);
            }
        }
        /// <summary>
        /// Validates and makes the specified move.
        /// </summary>
        /// <param name="fromPile">The pile containing the cards to be moved.</param>
        /// <param name="toPile">The pile where the cards are being moved to.</param>
        /// <param name="cardsMoved">The number of cards being moved.</param>
        /// <param name="validate">Validation can be turned off for faster performance.</param>
        /// <returns>Returns true if the move is valid. Will always be true when validation is turned off.</returns>
        public bool MakeMove(PileType fromPile, PileType toPile, int cardsMoved, bool validate = true)
        {
            if (validate && !ValidateMove((int)fromPile, (int)toPile, cardsMoved)) { return false; }
            try
            {
                int from = (int)fromPile;
                int to = (int)toPile;
                movesMade.AddLast(from, to, cardsMoved, 0);
                if (from != to)
                {
                    if (to == (int)PileType.Stock || to == (int)PileType.Waste)
                    {
                        piles[from].RemoveTop(piles[to], cardsMoved);
                        if (to == (int)PileType.Stock) { rounds++; }
                    }
                    else if (cardsMoved == 1)
                    {
                        piles[from].Remove(piles[to]);
                        if (to >= (int)PileType.Foundation1)
                        {
                            foundationCount++;
                            SetFoundationMin();
                        }
                        else if (from >= (int)PileType.Foundation1)
                        {
                            foundationCount--;
                            SetFoundationMin();
                        }
                    }
                    else
                    {
                        piles[from].Remove(piles[to], cardsMoved);
                    }
                }
                else
                {
                    piles[from].Flip();
                }
                UpdateMoves();
                return true;
            }
            catch
            {
                throw new Exception("An error occured trying to apply move: [" + fromPile + " to " + toPile + " moving " + cardsMoved + "]");
            }
        }
        /// <summary>
        /// Validates and makes the specified list of moves.
        /// </summary>
        /// <param name="moves">The moves to be made.</param>
        /// <param name="validate">Validation can be turned off for faster performance.</param>
        /// <returns>Returns true if the list of moves is valid. Will always be true when validation is turned off.</returns>
        public bool MakeMove(MoveList moves, bool validate = true)
        {
            try
            {
                Move move = moves.first;
                while (move != null)
                {
                    if (validate && !ValidateMove(move.from, move.to, move.cards)) { return false; }
                    movesMade.AddLast(move.from, move.to, move.cards, 0);
                    if (move.from != move.to)
                    {
                        if (move.to == (int)PileType.Stock || move.to == (int)PileType.Waste)
                        {
                            piles[move.from].RemoveTop(piles[move.to], move.cards);
                            if (move.to == (int)PileType.Stock) { rounds++; }
                        }
                        else if (move.cards == 1)
                        {
                            piles[move.from].Remove(piles[move.to]);
                            if (move.to >= (int)PileType.Foundation1)
                            {
                                foundationCount++;
                                SetFoundationMin();
                            }
                            else if (move.from >= (int)PileType.Foundation1)
                            {
                                foundationCount--;
                                SetFoundationMin();
                            }
                        }
                        else
                        {
                            piles[move.from].Remove(piles[move.to], move.cards);
                        }
                    }
                    else
                    {
                        piles[move.from].Flip();
                    }
                    move = move.next;
                }
                UpdateMoves();
                return true;
            }
            catch
            {
                string moveVal = "NULL";
                if (moves != null)
                {
                    moveVal = moves.ToString();
                }
                throw new Exception("An error occured trying to apply move(s): " + moveVal);
            }
        }
        private bool ValidateMove(int fromPile, int toPile, int cardsMoved)
        {
            if (cardsMoved < 0 || fromPile < 0 || fromPile > 12 || toPile < 0 || toPile > 12 || (fromPile != toPile && cardsMoved == 0))
            {
                return false;
            }
            Pile from = piles[fromPile];
            Pile to = piles[toPile];
            Card cTo, cFrom;
            switch (fromPile)
            {
                case (int)PileType.Stock:
                    if (toPile != (int)PileType.Waste || cardsMoved > drawCount || (cardsMoved < drawCount && from.size != cardsMoved))
                    {
                        return false;
                    }
                    return true;
                case (int)PileType.Waste:
                    if (from.size == 0 || toPile == (int)PileType.Waste || (toPile == (int)PileType.Stock && cardsMoved != from.size) || (toPile != (int)PileType.Stock && cardsMoved > 1))
                    {
                        return false;
                    }
                    if (toPile == (int)PileType.Stock)
                    {
                        return true;
                    }
                    cTo = to.TopCard;
                    cFrom = from.cards[from.size - 1];
                    if (toPile >= (int)PileType.Foundation1)
                    {
                        if (cFrom.rank - cTo.rank != 1 || (cFrom.suit != cTo.suit && cTo.rank >= (int)Rank.Ace))
                        {
                            return false;
                        }
                        return true;
                    }
                    if (!cTo.up || (cTo == Card.None && cFrom.rank != (int)Rank.King) || (cTo != Card.None && (cTo.rank - cFrom.rank != 1 || cTo.color == cFrom.color)))
                    {
                        return false;
                    }
                    return true;
                case (int)PileType.Foundation1:
                case (int)PileType.Foundation2:
                case (int)PileType.Foundation3:
                case (int)PileType.Foundation4:
                    if (from.size == 0 || toPile < (int)PileType.Tableau1 || toPile > (int)PileType.Tableau7 || cardsMoved > 1)
                    {
                        return false;
                    }
                    cTo = to.TopCard;
                    cFrom = from.cards[from.size - 1];
                    if (!cTo.up || (cTo == Card.None && cFrom.rank != (int)Rank.King) || (cTo != Card.None && (cTo.rank - cFrom.rank != 1 || cTo.color == cFrom.color)))
                    {
                        return false;
                    }
                    return true;
                case (int)PileType.Tableau1:
                case (int)PileType.Tableau2:
                case (int)PileType.Tableau3:
                case (int)PileType.Tableau4:
                case (int)PileType.Tableau5:
                case (int)PileType.Tableau6:
                case (int)PileType.Tableau7:
                    if (from.size == 0 || toPile == (int)PileType.Stock || toPile == (int)PileType.Waste)
                    {
                        return false;
                    }
                    cTo = to.TopCard;
                    cFrom = from.cards[from.size - 1];
                    if (toPile == fromPile && cardsMoved == 0 && !cFrom.up)
                    {
                        return true;
                    }
                    if (toPile != fromPile && (!cTo.up || !cFrom.up))
                    {
                        return false;
                    }
                    if (toPile >= (int)PileType.Foundation1)
                    {
                        if (cFrom.rank - cTo.rank != 1 || (cFrom.suit != cTo.suit && cTo.rank >= (int)Rank.Ace) || cardsMoved > 1)
                        {
                            return false;
                        }
                        return true;
                    }
                    Card cTop = from.cards[from.top];
                    if ((cTo == Card.None && cTop.rank != (int)Rank.King) || (cTo != Card.None && (cFrom.rank >= cTo.rank || cTop.rank + 1 < cTo.rank || ((cTo.color ^ cFrom.color) ^ (cTo.odd ^ cFrom.odd)) != 0)))
                    {
                        return false;
                    }
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Undos the last move(s) made in the solitaire game.
        /// </summary>
        /// <param name="movesToUndo">The number of moves to undo.</param>
        public void UndoMove(int movesToUndo = 1)
        {
            do
            {
                Move move = movesMade.RemoveLast();
                if (move == null) { return; }
                if (move.from != move.to)
                {
                    if (move.to == (int)PileType.Stock || move.to == (int)PileType.Waste)
                    {
                        piles[move.to].RemoveTop(piles[move.from], move.cards);
                        if (move.to == (int)PileType.Stock) { rounds--; }
                    }
                    else if (move.cards == 1)
                    {
                        piles[move.to].Remove(piles[move.from]);
                        if (move.to >= (int)PileType.Foundation1)
                        {
                            foundationCount--;
                            SetFoundationMin();
                        }
                        else if (move.from >= (int)PileType.Foundation1)
                        {
                            foundationCount++;
                            SetFoundationMin();
                        }
                    }
                    else
                    {
                        piles[move.to].Remove(piles[move.from], move.cards);
                    }
                }
                else
                {
                    piles[move.to].Flip();
                }
                movesToUndo--;
            } while (movesToUndo > 0);
            UpdateMoves();
        }
        private void SetFoundationMin()
        {
            int one = (int)piles[foundationOrder[1]].TopRank;
            int two = (int)piles[foundationOrder[3]].TopRank;
            redMin = one <= two ? one : two;
            one = (int)piles[foundationOrder[0]].TopRank;
            two = (int)piles[foundationOrder[2]].TopRank;
            blackMin = one <= two ? one : two;
            for (int i = 0; i < 4; i++)
            {
                int size = piles[9 + i].size;
                if (size > 0)
                {
                    foundationOrder[i] = (9 + piles[9 + i].cards[size - 1].suit);
                }
            }
        }
        private void UpdateMoves()
        {
            moves.Clear();//start from scratch

            //Check waste to draw
            //Check draw to waste
            int wasteSize = piles[(int)PileType.Waste].size;
            int stockSize = piles[(int)PileType.Stock].size;
            if (stockSize > 0)//Check if we have cards to draw
            {
                //moves.AddLast((int)PileType.Stock, (int)PileType.Waste, drawCount >= stockSize ? stockSize : drawCount, wasteSize == 0 ? 1 : 0);
                moves.AddLast((int)PileType.Stock, (int)PileType.Waste, drawCount >= stockSize ? stockSize : drawCount, moveValues(this, PileType.Stock, PileType.Waste, drawCount >= stockSize ? stockSize : drawCount));
            }
            else if (wasteSize > drawCount)//No use turning the waste pile over if it won't change anything
            {
                moves.AddLast((int)PileType.Waste, (int)PileType.Stock, wasteSize, moveValues(this, PileType.Stock, PileType.Waste, wasteSize));
            }

            //Check waste to goal
            //Check waste to base
            if (wasteSize > 0)//Need a card in the waste pile
            {
                Card cwaste = piles[(int)PileType.Waste].cards[wasteSize - 1];
                int wasteFoundation = foundationOrder[cwaste.suit]; //get the goal pile number of the waste card's suit
                if (cwaste.rank - (int)piles[wasteFoundation].TopRank == 1)//check if it can be moved to its goal pile
                {
                    //int min = cwaste.color == 0 ? redMin : blackMin; //get the opposite suits minimum rank
                    //if (cwaste.rank <= min) { moves.AddFirst((int)PileType.Waste, wasteFoundation, 1, 1); }
                    //else { moves.AddLast((int)PileType.Waste, wasteFoundation, 1, 0); }
                    moves.Add((int)PileType.Waste, wasteFoundation, 1, moveValues(this, PileType.Waste, (PileType)wasteFoundation, 1));
                }
                //check waste to base
                for (int i = (int)PileType.Tableau1; i <= (int)PileType.Tableau7; i++)
                {
                    Pile pile = piles[i];
                    int size = pile.size;
                    if (size != 0)//check non empty piles
                    {
                        Card card = pile.cards[size - 1]; //get the last card in the pile
                        //make sure it's facing up and the waste card can go ontop of it
                        if (!card.up || card.rank - cwaste.rank != 1 || card.color == cwaste.color) { continue; }
                        moves.Add((int)PileType.Waste, i, 1, moveValues(this, PileType.Waste, (PileType)i, 1));
                        continue;
                    }
                    if (cwaste.rank != (int)Rank.King) { continue; }//we are on an empty pile so we need a king to be able to move
                    moves.Add((int)PileType.Waste, i, 1, moveValues(this, PileType.Waste, (PileType)i, 1));
                    break;
                }
            }

            //Check flip of base pile
            //Check base to goal
            //Check base to base
            for (int i = (int)PileType.Tableau1; i <= (int)PileType.Tableau7; i++)
            {
                Pile pile1 = piles[i];
                int pile1Size = pile1.size;
                if (pile1Size == 0) { continue; } //only check starting piles that have cards

                Card card1 = pile1.cards[pile1Size - 1];
                if (!card1.up)//if the last card is facing down add move to flip it
                {
                    //moves.AddFirst(i, i, 0, 1);
                    moves.Add(i, i, 0, moveValues(this, (PileType)i, (PileType)i, 0));
                    continue;
                }

                int cardFoundation = foundationOrder[card1.suit]; //get the last cards goal pile number
                int foundationSize = piles[cardFoundation].size;
                int foundationRank = foundationSize > 0 ? piles[cardFoundation].cards[foundationSize - 1].rank : -1;
                if (card1.rank - foundationRank == 1)//check if we can move it to the goal
                {
                    //int min = card1.color == 0 ? redMin : blackMin;
                    //if (card1.rank <= min) { moves.AddFirst(i, cardFoundation, 1, 1); }
                    //else { moves.Add(i, cardFoundation, 1, 0); }
                    moves.Add(i, cardFoundation, 1, moveValues(this, (PileType)i, (PileType)cardFoundation, 1));
                }

                Card card2 = pile1.cards[pile1.top];
                int pile1Length = (card2.rank - card1.rank + 1);
                bool kingMoved = false;
                for (int j = (int)PileType.Tableau1; j <= (int)PileType.Tableau7; j++)
                {
                    if (i == j) { continue; } //dont check the same pile

                    Pile pile2 = piles[j];
                    int pile2Size = pile2.size;
                    if (pile2Size == 0)//check if this pile is empty
                    {
                        if (card2.rank != (int)Rank.King || pile1Size == pile1Length || kingMoved) { continue; } //we need a king and we should only move a king when it is covering a card
                        //moves.Add(i, j, pile1Length, 0);
                        moves.Add(i, j, pile1Length, moveValues(this, (PileType)i, (PileType)j, pile1Length));
                        kingMoved = true;
                        continue;
                    }

                    Card card3 = pile2.cards[pile2Size - 1];
                    if (!card3.up) { continue; } //we need the last card to be facing up

                    //check if we can move the card stack (card1 to card2) onto card3
                    if (card1.rank >= card3.rank || card2.rank + 1 < card3.rank || ((card3.color ^ card1.color) ^ (card3.odd ^ card1.odd)) != 0)
                    {
                        continue;
                    }
                    int pile1Moved = (card3.rank - card1.rank);
                    //if (pile1Moved == pile1Length && pile1Length != pile1Size) { moves.Add(i, j, pile1Moved, 0); }
                    //else { moves.AddLast(i, j, pile1Moved, -1); }
                    moves.Add(i, j, pile1Moved, moveValues(this, (PileType)i, (PileType)j, pile1Moved));
                }
            }

            //Check goal to base
            for (int i = (int)PileType.Foundation1; i <= (int)PileType.Foundation4; i++)
            {
                //Need a card to check
                Pile pile = piles[i];
                int size = pile.size;
                if (size == 0) { continue; }

                Card card1 = pile.cards[size - 1];
                for (int j = (int)PileType.Tableau1; j <= (int)PileType.Tableau7; j++)
                {
                    int pile2Size = piles[j].size;
                    Card card2 = pile2Size > 0 ? piles[j].cards[pile2Size - 1] : Card.None;

                    //Check if we can move to base pile
                    if (!card2.up || (card2.rank < 0 && card1.rank != (int)Rank.King) || (card2.rank >= 0 && (card2.rank - card1.rank != 1 || card1.color == card2.color)))
                    {
                        continue;
                    }
                    //moves.AddLast(i, j, 1, -1);
                    moves.Add(i, j, 1, moveValues(this, (PileType)i, (PileType)j, 1));
                    if (card1.rank == (int)Rank.King) { break; }
                }
            }
        }
        /// <summary>
        /// A string representation of the current state of this solitaire game.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 13; i++)
            {
                sb.Append(((PileType)i).ToString()).Append(": ");
                sb.AppendLine(piles[i].ToString());
            }
            sb.Append("Moves Available: ").AppendLine(moves.ToString());
            sb.Append("Moves Made: ").Append(TotalMovesMade);
            return sb.ToString();
        }
        /// <summary>
        /// <para>Card Set is a string representation of the initial deal of this particular solitaire game.</para>
        /// <para>The format is R1R1S1...R52R52S52 where RxRxSx is the rank and suit of that card in numeric digits.</para>
        /// <para>Rank goes from 01 to 13(Ace to King) and suit goes from 1 to 4 (Clubs 1,Dimonds 2,Hearts 3,Spades 4).</para>
        /// <para>The position of the cards is as follows:</para>
        /// <para>Stock pile: card positions 29-52 where 29 would be the top card on the stock pile.</para>
        /// <para>Tableau piles:</para>
        /// <para>#1 #2 #3 #4 #5 #6 #7</para>
        /// <para>01 02 03 04 05 06 07</para>
        /// <para>__ 08 09 10 11 12 13</para>
        /// <para>__ __ 14 15 16 17 18</para>
        /// <para>__ __ __ 19 20 21 22</para>
        /// <para>__ __ __ __ 23 24 25</para>
        /// <para>__ __ __ __ __ 26 27</para>
        /// <para>__ __ __ __ __ __ 28</para>
        /// <para>So 012113... would represent Ace of Dimonds(AD) and the Jack of Hearts(JH)</para>
        /// </summary>
        public string CardSet
        {
            get
            {
                StringBuilder sb = new StringBuilder(3 * 52);
                int suit, rank;
                for (int i = 0; i < 52; i++)
                {
                    suit = cards[i].suit;
                    if (suit >= 2)
                    {
                        suit = (suit == 2) ? 3 : 2;
                    }
                    suit++;
                    rank = cards[i].rank + 1;
                    sb.Append(rank < 10 ? "0" : "").Append(rank).Append(suit);
                }
                return sb.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 156)
                {
                    throw new FormatException("Card Set is in an invalid format.");
                }
                int suit, rank;
                for (int i = 0; i < 52; i++)
                {
                    suit = (value[i * 3 + 2] ^ 0x30) - 1;
                    if (suit < 0 || suit > 3) { throw new FormatException("Card Set is in an invalid format."); }
                    if (suit >= 2)
                    {
                        suit = (suit == 2) ? 3 : 2;
                    }
                    rank = (value[i * 3] ^ 0x30) * 10 + (value[i * 3 + 1] ^ 0x30) - 1;
                    if (rank < 0 || rank > 12) { throw new FormatException("Card Set is in an invalid format."); }
                    cards[i].rank = rank;
                    cards[i].suit = suit;
                    cards[i].odd = rank & 1;
                    cards[i].color = suit & 1;
                }
                Reset();
            }
        }
        /// <summary>
        /// Returns true when the specified object is a solitaire object and its card set is equal and its piles contain the same cards.
        /// </summary>
        /// <param name="obj">The object to be checked for equality.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Solitaire)) { return false; }
            Solitaire tmp = (Solitaire)obj;
            for (int i = 0; i < 52; i++)
            {
                if (cards[i].value != tmp.cards[i].value) { return false; }
            }
            for (int i = 0; i < 13; i++)
            {
                if (piles[i].size != tmp.piles[i].size) { return false; }
                Pile pileA = piles[i];
                Pile pileB = tmp.piles[i];
                for (int j = pileA.size - 1; j >= 0; j--)
                {
                    if (pileA.cards[j].value != pileB.cards[j].value || pileA.cards[j].up != pileB.cards[j].up) { return false; }
                }
            }
            return true;
        }
        /// <summary>
        /// Returns an integer representation of this solitaire game in its current state.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int ret1 = 0;
            for (int i = (int)PileType.Tableau1; i <= (int)PileType.Tableau4; i++)
            {
                ret1 <<= 7;
                Card c = piles[i].TopCard;
                ret1 |= ((c.up ? 1 : 0) << 6) | c.value;
            }
            int ret2 = 0;
            for (int i = (int)PileType.Tableau5; i <= (int)PileType.Tableau7; i++)
            {
                ret2 <<= 7;
                Card c = piles[i].TopCard;
                ret2 |= ((c.up ? 1 : 0) << 6) | c.value;
            }
            int ret3 = 0;
            for (int i = (int)PileType.Foundation1; i <= (int)PileType.Foundation4; i++)
            {
                ret3 <<= 7;
                Card c = piles[i].TopCard;
                ret3 |= ((c.up ? 1 : 0) << 6) | c.value;
            }
            return ret1 ^ ret2 ^ ret3;
        }
        /// <summary>
        /// Returns true when the specified solitaire objects' card set is equal and its piles contain the same cards.
        /// </summary>
        /// <param name="solitaire">The solitaire game to be checked for equality.</param>
        /// <returns></returns>
        public bool Equals(Solitaire solitaire)
        {
            for (int i = 0; i < 52; i++)
            {
                if (cards[i].value != solitaire.cards[i].value) { return false; }
            }
            for (int i = 0; i < 13; i++)
            {
                if (piles[i].size != solitaire.piles[i].size) { return false; }
                Pile pileA = piles[i];
                Pile pileB = solitaire.piles[i];
                for (int j = pileA.size - 1; j >= 0; j--)
                {
                    if (pileA.cards[j].value != pileB.cards[j].value || pileA.cards[j].up != pileB.cards[j].up) { return false; }
                }
            }
            return true;
        }
    }
}