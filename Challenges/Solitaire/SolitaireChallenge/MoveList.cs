using System;
using System.Collections.Generic;
using System.Text;
namespace OmahaMTG.Challenge.SolitaireChallenge
{
    /// <summary>
    /// Represents a double linked list of Moves that would be used in a game of Solitaire. This class is not thread safe.
    /// </summary>
    public sealed class MoveList : System.Collections.Generic.IEnumerable<Move>, System.Collections.IEnumerable
    {
        private Move extra;
        internal Move first, last;
        internal int size;

        /// <summary>
        /// Initialize a new instance of the MoveList class.
        /// </summary>
        public MoveList()
        {
            size = 0;
            first = null;
            last = null;
            extra = null;
        }
        /// <summary>
        /// Returns a clone of this MoveList object.
        /// </summary>
        /// <returns>A new MoveList.</returns>
        public MoveList Clone()
        {
            MoveList list = new MoveList();
            Move temp = first;
            while (temp != null)
            {
                list.AddLast(temp.from, temp.to, temp.cards, temp.val);
                temp = temp.next;
            }
            return list;
        }
        /// <summary>
        /// Returns the total number of moves in this list.
        /// </summary>
        public int Size
        {
            get { return size; }
        }
        /// <summary>
        /// The number of actual moves not including the "Turn", which is moving all cards in the waste pile back to the stock pile.
        /// </summary>
        public int TotalMoves
        {
            get
            {
                Move tmp = first;
                int ret = 0;
                while (tmp != null)
                {
                    if (tmp.from != (int)PileType.Waste || tmp.to != (int)PileType.Stock)
                    {
                        ret++;
                    }
                    tmp = tmp.next;
                }
                return ret;
            }
        }
        /// <summary>
        /// Counts the number of moves in this list that meet the specified criteria in the predicate function.
        /// </summary>
        public int Count(Predicate<Move> match)
        {
            Move tmp = first;
            int ret = 0;
            while (tmp != null)
            {
                if (match(tmp)) { ret++; }
                tmp = tmp.next;
            }
            return ret;
        }
        /// <summary>
        /// Clears the moves from this list.
        /// </summary>
        public void Clear()
        {
            if (first != null)
            {
                last.next = extra;
                if (extra != null) { extra.prev = last; }
                extra = first;
            }
            size = 0;
            first = null;
            last = null;
        }
        /// <summary>
        /// Removes any temporary saved moves so they may be garbage collected.
        /// </summary>
        public void Trim()
        {
            extra = null;
        }
        /// <summary>
        /// The first move in this list.
        /// </summary>
        public Move First
        {
            get { return first; }
        }
        /// <summary>
        /// The last move in this list.
        /// </summary>
        public Move Last
        {
            get { return last; }
        }
        /// <summary>
        /// Adds and returns a new move to the front of the list in O(1) time.
        /// </summary>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <returns>The new move.</returns>
        public Move AddFirst(PileType fromPile, PileType toPile, int cardsMoved)
        {
            return AddFirst((int)fromPile, (int)toPile, cardsMoved, 0);
        }
        /// <summary>
        /// Adds and returns a new move to the front of the list in O(1) time.
        /// </summary>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <param name="value">The value given to this move.</param>
        /// <returns>The new move.</returns>
        public Move AddFirst(PileType fromPile, PileType toPile, int cardsMoved, int value)
        {
            return AddFirst((int)fromPile, (int)toPile, cardsMoved, value);
        }
        /// <summary>
        /// Adds and returns a new move to the front of the list based on the specified move in O(1) time.
        /// </summary>
        /// <param name="move">The specified move.</param>
        /// <returns>The new move.</returns>
        public Move AddFirst(Move move)
        {
            if (move == null) { throw new ArgumentNullException("Move cannot be null."); }
            return AddFirst(move.from, move.to, move.cards, move.val);
        }
        /// <summary>
        /// Adds the specified collection of moves to the front of this list.
        /// </summary>
        /// <param name="moves">The collection of moves to be added.</param>
        public void AddFirst(IEnumerable<Move> moves)
        {
            if (moves == null) { throw new ArgumentNullException("Moves cannot be null."); }
            IEnumerator<Move> en = moves.GetEnumerator();
            if (en != null)
            {
                en.MoveNext();
                Move beg = null;
                Move end = null;
                if (en.Current != null)
                {
                    beg = AddAt(0, en.Current.from, en.Current.to, en.Current.cards, en.Current.val);
                    end = beg.next;
                    en.MoveNext();
                }
                Move ins;
                while (en.Current != null)
                {
                    if (extra != null)
                    {
                        extra.from = en.Current.from; extra.to = en.Current.to; extra.cards = en.Current.cards; extra.val = en.Current.val;
                        ins = extra;
                        extra = extra.next;
                    }
                    else
                    {
                        ins = new Move(en.Current.from, en.Current.to, en.Current.cards, en.Current.val);
                    }
                    beg.next = ins;
                    ins.prev = beg;
                    beg = ins;
                    en.MoveNext();
                }
                if (beg != null)
                {
                    beg.next = end;
                    if (end != null)
                    {
                        end.prev = beg;
                    }
                }
            }
        }
        internal Move AddFirst(int fromPile, int toPile, int cardsMoved, int value)
        {
            size++;
            Move temp;
            if (extra != null)
            {
                extra.from = fromPile; extra.to = toPile; extra.cards = cardsMoved; extra.val = value;
                temp = extra;
                extra = extra.next;
            }
            else
            {
                temp = new Move(fromPile, toPile, cardsMoved, value);
            }
            if (first != null)
            {
                temp.next = first;
                temp.prev = null;
                first.prev = temp;
                first = temp;
                return temp;
            }
            temp.prev = null;
            temp.next = null;
            first = temp;
            last = first;
            return first;
        }
        /// <summary>
        /// Adds and returns a new move to the end of the list in O(1) time.
        /// </summary>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <returns>The new move.</returns>
        public Move AddLast(PileType fromPile, PileType toPile, int cardsMoved)
        {
            return AddLast((int)fromPile, (int)toPile, cardsMoved, 0);
        }
        /// <summary>
        /// Adds and returns a new move to the end of the list in O(1) time.
        /// </summary>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <param name="value">The value given to this move.</param>
        /// <returns>The new move.</returns>
        public Move AddLast(PileType fromPile, PileType toPile, int cardsMoved, int value)
        {
            return AddLast((int)fromPile, (int)toPile, cardsMoved, value);
        }
        /// <summary>
        /// Adds and returns a new move to the end of the list based on the specified move in O(1) time.
        /// </summary>
        /// <param name="move">The specified move.</param>
        /// <returns>The new move.</returns>
        public Move AddLast(Move move)
        {
            if (move == null) { throw new ArgumentNullException("Move cannot be null."); }
            return AddLast(move.from, move.to, move.cards, move.val);
        }
        /// <summary>
        /// Adds the specified collection of moves to the end of this list.
        /// </summary>
        /// <param name="moves">The collection of moves to be added.</param>
        public void AddLast(IEnumerable<Move> moves)
        {
            if (moves == null) { throw new ArgumentNullException("Moves cannot be null."); }
            IEnumerator<Move> en = moves.GetEnumerator();
            if (en != null)
            {
                en.MoveNext();
                while (en.Current != null)
                {
                    AddLast(en.Current.from, en.Current.to, en.Current.cards, en.Current.val);
                    en.MoveNext();
                }
            }
        }
        internal Move AddLast(int fromPile, int toPile, int cardsMoved, int value)
        {
            size++;
            Move temp;
            if (extra != null)
            {
                extra.from = fromPile; extra.to = toPile; extra.cards = cardsMoved; extra.val = value;
                temp = extra;
                extra = extra.next;
            }
            else
            {
                temp = new Move(fromPile, toPile, cardsMoved, value);
            }
            if (last != null)
            {
                last.next = temp;
                temp.prev = last;
                temp.next = null;
                last = temp;
                return last;
            }
            first = temp;
            temp.prev = null;
            temp.next = null;
            last = first;
            return first;
        }
        /// <summary>
        /// Adds and returns a new move at the specified index to the list in O(index)/O(size-index) time.
        /// </summary>
        /// <param name="index">The index to add the new move at.</param>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <returns>The new move.</returns>
        public Move AddAt(int index, PileType fromPile, PileType toPile, int cardsMoved)
        {
            return AddAt(index, (int)fromPile, (int)toPile, cardsMoved, 0);
        }
        /// <summary>
        /// Adds and returns a new move at the specified index to the list in O(index)/O(size-index) time.
        /// </summary>
        /// <param name="index">The index to add the new move at.</param>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <param name="value">The value given to this move.</param>
        /// <returns>The new move.</returns>
        public Move AddAt(int index, PileType fromPile, PileType toPile, int cardsMoved, int value)
        {
            return AddAt(index, (int)fromPile, (int)toPile, cardsMoved, value);
        }
        /// <summary>
        /// Adds and returns a new move at the specified index to the list based on the specified move in O(index)/O(size-index) time.
        /// </summary>
        /// <param name="index">The index to add the new move at.</param>
        /// <param name="move">The specified move.</param>
        /// <returns>The new move.</returns>
        public Move AddAt(int index, Move move)
        {
            if (move == null) { throw new ArgumentNullException("Move cannot be null."); }
            return AddAt(index, move.from, move.to, move.cards, move.val);
        }
        /// <summary>
        /// Adds the specified collection of moves to the list starting at the specified index.
        /// </summary>
        /// <param name="moves">The collection of moves to be added.</param>
        /// <param name="index">The index to add the moves at.</param>
        public void AddAt(int index, IEnumerable<Move> moves)
        {
            if (moves == null) { throw new ArgumentNullException("Moves cannot be null."); }
            IEnumerator<Move> en = moves.GetEnumerator();
            if (en != null)
            {
                en.MoveNext();
                Move beg = null;
                Move end = null;
                if (en.Current != null)
                {
                    beg = AddAt(index, en.Current.from, en.Current.to, en.Current.cards, en.Current.val);
                    end = beg.next;
                    en.MoveNext();
                }
                Move ins;
                while (en.Current != null)
                {
                    if (extra != null)
                    {
                        extra.from = en.Current.from; extra.to = en.Current.to; extra.cards = en.Current.cards; extra.val = en.Current.val;
                        ins = extra;
                        extra = extra.next;
                    }
                    else
                    {
                        ins = new Move(en.Current.from, en.Current.to, en.Current.cards, en.Current.val);
                    }
                    beg.next = ins;
                    ins.prev = beg;
                    beg = ins;
                    en.MoveNext();
                }
                if (beg != null)
                {
                    beg.next = end;
                    if (end != null)
                    {
                        end.prev = beg;
                    }
                }
            }
        }
        internal Move AddAt(int index, int fromPile, int toPile, int cardsMoved, int value)
        {
            if (index >= 0 && index <= (size >> 1))
            {
                size++;
                Move temp = first;
                while (index > 0)
                {
                    index--;
                    temp = temp.next;
                }
                Move ins;
                if (extra != null)
                {
                    extra.from = fromPile; extra.to = toPile; extra.cards = cardsMoved; extra.val = value;
                    ins = extra;
                    extra = extra.next;
                }
                else
                {
                    ins = new Move(fromPile, toPile, cardsMoved, value);
                }
                if (temp == null)
                {
                    ins.next = null;
                    ins.prev = null;
                    first = ins;
                    last = ins;
                    return ins;
                }
                ins.prev = temp.prev;
                ins.next = temp;
                temp.prev = ins;
                if (ins.prev != null) { ins.prev.next = ins; } else { first = ins; }
                return ins;
            }
            else if (index > (size >> 1) && index <= size)
            {
                size++;
                Move temp = last;
                int max = size - 2;
                while (index < max)
                {
                    index++;
                    temp = temp.prev;
                }
                Move ins;
                if (extra != null)
                {
                    extra.from = fromPile; extra.to = toPile; extra.cards = cardsMoved; extra.val = value;
                    ins = extra;
                    extra = extra.next;
                }
                else
                {
                    ins = new Move(fromPile, toPile, cardsMoved, value);
                }
                if (index == size - 1)
                {
                    ins.next = null;
                    last.next = ins;
                    ins.prev = last;
                    last = ins;
                    return ins;
                }
                ins.prev = temp.prev;
                ins.next = temp;
                temp.prev = ins;
                ins.prev.next = ins;
                return ins;
            }
            return null;
        }
        /// <summary>
        /// Adds and returns a new move to the list sorting descending on the specified value in O(n) time.
        /// </summary>
        /// <param name="fromPile">The from pile.</param>
        /// <param name="toPile">The to pile.</param>
        /// <param name="cardsMoved">The amount of cards being moved.</param>
        /// <param name="value">The value given to this move.</param>
        /// <returns>The new move.</returns>
        public Move Add(PileType fromPile, PileType toPile, int cardsMoved, int value)
        {
            return Add((int)fromPile, (int)toPile, cardsMoved, value);
        }
        /// <summary>
        /// Adds and returns a new move to the list sorting descending on the specified moves' value in O(n) time.
        /// </summary>
        /// <param name="move">The specified move.</param>
        /// <returns>The new move.</returns>
        public Move Add(Move move)
        {
            return Add(move.from, move.to, move.cards, move.val);
        }
        internal Move Add(int fromPile, int toPile, int cardsMoved, int value)
        {
            size++;
            Move temp;
            if (extra != null)
            {
                extra.from = fromPile; extra.to = toPile; extra.cards = cardsMoved; extra.val = value;
                temp = extra;
                extra = extra.next;
            }
            else
            {
                temp = new Move(fromPile, toPile, cardsMoved, value);
            }
            if (first != null)
            {
                if (value <= last.val)
                {
                    last.next = temp;
                    temp.next = null;
                    temp.prev = last;
                    last = temp;
                    return last;
                }

                int v = temp.val;
                if (v >= first.val)
                {
                    temp.next = first;
                    first.prev = temp;
                    first = temp;
                }
                else
                {
                    Move cur = first;
                    while (cur.next != null && v < cur.next.val) { cur = cur.next; }
                    temp.next = cur.next;
                    cur.next.prev = temp;
                    cur.next = temp;
                    temp.prev = cur;
                }

                if (last.next != null) { last = last.next; }
                return temp;
            }
            temp.next = null;
            temp.prev = null;
            first = temp;
            last = first;
            return first;
        }
        /// <summary>
        /// Removes and returns the first move in the list in O(1) time.
        /// </summary>
        /// <returns></returns>
        public Move RemoveFirst()
        {
            if (size > 0)
            {
                size--;
                Move ret = first.next;
                first.next = extra;
                extra = first;
                first = ret;
                if (first != null) { first.prev = null; }
                if (size == 0) { last = null; }
                return extra;
            }
            return null;
        }
        /// <summary>
        /// Removes and returns the last move in the list in O(1) time.
        /// </summary>
        /// <returns></returns>
        public Move RemoveLast()
        {
            if (size > 0)
            {
                size--;
                Move ret = last.prev;
                last.next = extra;
                extra = last;
                last = ret;
                if (last != null) { last.next = null; }
                if (size == 0) { first = null; }
                return extra;
            }
            return null;
        }
        /// <summary>
        /// Removes and returns the move at the specified index in O(index)/O(size-index) time.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Move RemoveAt(int index)
        {
            if (index >= 0 && index < (size >> 1))
            {
                size--;
                Move temp = first;
                while (index > 0)
                {
                    index--;
                    temp = temp.next;
                }
                if (temp.prev != null) { temp.prev.next = temp.next; } else { first = first.next; }
                if (temp.next != null) { temp.next.prev = temp.prev; } else { last = last.prev; }
                temp.prev = null;
                temp.next = extra;
                extra = temp;
                return extra;
            }
            else if (index >= (size >> 1) && index < size)
            {
                size--;
                Move temp = last;
                int max = size;
                while (index < max)
                {
                    index++;
                    temp = temp.prev;
                }
                if (temp.prev != null) { temp.prev.next = temp.next; } else { first = first.next; }
                if (temp.next != null) { temp.next.prev = temp.prev; } else { last = last.prev; }
                temp.prev = null;
                temp.next = extra;
                extra = temp;
                return extra;
            }
            return null;
        }
        /// <summary>
        /// Sorts this list based on the contained moves' values in either descending or ascending order in O(n) time.
        /// </summary>
        /// <param name="descending"></param>
        public void Sort(bool descending)
        {
            if (size < 2) { return; }
            if (size < 32)
            {
                Move mLowTmp;
                if (descending) { InsertDesc(0, size - 1, first, out mLowTmp); return; }
                InsertAsc(0, size - 1, first, out mLowTmp); return;
            }
            MoveList[] lists = new MoveList[256];
            int desc = descending ? 255 : 0;
            for (int i = 0; i < 32; i += 8)
            {
                Move temp = first;
                while (temp != null)
                {
                    int bucket = ((temp.val >> i) & 255) - desc;
                    if (bucket < 0) { bucket = -bucket; }
                    if (lists[i] == null)
                    {
                        lists[i] = new MoveList();
                    }
                    lists[i].AddExisting(temp);
                    temp = temp.next;
                }
                first = null;
                last = null;
                size = 0;
                for (int j = 0; j < 256; j++)
                {
                    if (lists[j] != null)
                    {
                        temp = lists[j].first;
                        while (temp != null)
                        {
                            AddExisting(temp);
                            temp = temp.next;
                        }
                        lists[j].first = null;
                        lists[j].last = null;
                    }
                }
            }
        }
        private Move AddExisting(Move move)
        {
            size++;
            if (last != null)
            {
                last.next = move;
                move.prev = last;
                move.next = null;
                last = move;
                return last;
            }
            first = move;
            move.prev = null;
            move.next = null;
            last = first;
            return first;
        }
        private Move InsertDesc(int low, int high, Move mLow, out Move outLow)
        {
            outLow = mLow;
            Move temp, mLowNxt = mLow.next;
            for (int i = low + 1; i <= high; i++)
            {
                mLow = mLowNxt;
                temp = mLow;
                mLowNxt = mLow.next;
                while (temp != outLow && temp.prev.val < mLow.val)
                {
                    temp = temp.prev;
                }
                if (temp == mLow) { continue; }
                if (temp == outLow) { outLow = mLow; }

                mLow.prev.next = mLow.next;
                if (mLow.next != null) { mLow.next.prev = mLow.prev; } else { last = last.prev; }
                mLow.prev = temp.prev;
                mLow.next = temp;
                temp.prev = mLow;
                if (mLow.prev != null) { mLow.prev.next = mLow; } else { first = mLow; }
            }
            return mLowNxt != null ? mLowNxt.prev : last;
        }
        private Move InsertAsc(int low, int high, Move mLow, out Move outLow)
        {
            outLow = mLow;
            Move temp, mLowNxt = mLow.next;
            for (int i = low + 1; i <= high; i++)
            {
                mLow = mLowNxt;
                temp = mLow;
                mLowNxt = mLow.next;
                while (temp != outLow && temp.prev.val > mLow.val)
                {
                    temp = temp.prev;
                }
                if (temp == mLow) { continue; }
                if (temp == outLow) { outLow = mLow; }

                mLow.prev.next = mLow.next;
                if (mLow.next != null) { mLow.next.prev = mLow.prev; } else { last = last.prev; }
                mLow.prev = temp.prev;
                mLow.next = temp;
                temp.prev = mLow;
                if (mLow.prev != null) { mLow.prev.next = mLow; } else { first = mLow; }
            }
            return mLowNxt != null ? mLowNxt.prev : last;
        }
        /// <summary>
        /// Sorts this list based on the contained moves in O(n*log(n)) time.
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(Comparison<Move> comparer)
        {
            if (size < 2) { return; }
            Move mLowTmp;
            Move mSplit = Sort(comparer, 0, (size - 1) >> 1, first, out mLowTmp).next;
            Move mSplitTmp;
            Sort(comparer, ((size - 1) >> 1) + 1, size - 1, mSplit, out mSplitTmp);
            Join(comparer, 0, size - 1, mLowTmp, mSplitTmp, out mLowTmp);
        }
        private Move Sort(Comparison<Move> comparer, int low, int high, Move mLow, out Move outLow)
        {
            if (high - low < 1) { outLow = mLow; return mLow; }
            Move mLowTmp;
            Move mSplit = Sort(comparer, low, (low + high) >> 1, mLow, out mLowTmp).next;
            Move mSplitTmp;
            Sort(comparer, ((low + high) >> 1) + 1, high, mSplit, out mSplitTmp);
            Move mJoin = Join(comparer, low, high, mLowTmp, mSplitTmp, out mLowTmp);
            outLow = mLowTmp;
            return mJoin;
        }
        private Move Join(Comparison<Move> comparer, int low, int high, Move mLow, Move mSplit, out Move outLow)
        {
            int lowStart = low;
            int split = ((low + high) >> 1) + 1;
            int splitI = split;
            outLow = mLow;
            do
            {
                while (low < split && comparer(mSplit,mLow) >= 0)
                {
                    mLow = mLow.next;
                    low++;
                }
                Move mSplitTmp = mSplit.next;
                if (mLow != mSplit)
                {
                    mSplit.prev.next = mSplit.next;
                    if (mSplit.next != null) { mSplit.next.prev = mSplit.prev; } else { last = last.prev; }
                    mSplit.prev = mLow.prev;
                    mSplit.next = mLow;
                    mLow.prev = mSplit;
                    if (mSplit.prev != null) { mSplit.prev.next = mSplit; } else { first = mSplit; }

                    if (low == lowStart && split == splitI) { outLow = mSplit; }
                }
                splitI++;
                mSplit = mSplitTmp;
            } while (low < split && splitI <= high);
            if (splitI > high && mSplit != null)
            {
                mSplit = mSplit.prev;
                return mSplit;
            }
            while (splitI < high)
            {
                mSplit = mSplit.next;
                splitI++;
            }
            return mSplit;
        }
        /// <summary>
        /// Returns the move at the specified index in O(index)/O(size-index) time.
        /// </summary>
        /// <param name="index">The specified index.</param>
        /// <returns></returns>
        public Move this[int index]
        {
            get
            {
                if (index >= 0 && index < (size >> 1))
                {
                    Move temp = first;
                    while (index > 0)
                    {
                        index--;
                        temp = temp.next;
                    }
                    return temp;
                }
                else if (index >= (size >> 1) && index < size)
                {
                    Move temp = last;
                    int max = size - 1;
                    while (index < max)
                    {
                        index++;
                        temp = temp.prev;
                    }
                    return temp;
                }
                return null;
            }
        }
        /// <summary>
        /// Returns the move at the specified index in O(index)/O(size-index) time.
        /// </summary>
        /// <param name="index">The specified index.</param>
        /// <returns></returns>
        public Move MoveAt(int index)
        {
            if (index >= 0 && index < (size >> 1))
            {
                Move temp = first;
                while (index > 0)
                {
                    index--;
                    temp = temp.next;
                }
                return temp;
            }
            else if (index >= (size >> 1) && index < size)
            {
                Move temp = last;
                int max = size - 1;
                while (index < max)
                {
                    index++;
                    temp = temp.prev;
                }
                return temp;
            }
            return null;
        }
        /// <summary>
        /// Returns a string containing all moves in this list.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (first != null)
            {
                StringBuilder sb = new StringBuilder();
                Move tmp = first;
                while (tmp != null)
                {
                    sb.Append(tmp.ToString());
                    tmp = tmp.next;
                }
                return sb.ToString();
            }
            return "";
        }
        /// <summary>
        /// Returns an enumerator for this list of moves. You may have unexpected behavior if the list is modified during enumeration.
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerator<Move> GetEnumerator()
        {
            Move t = first;
            while (t != null)
            {
                yield return t;
                t = t.next;
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Move t = first;
            while (t != null)
            {
                yield return t;
                t = t.next;
            }
        }
    }
}