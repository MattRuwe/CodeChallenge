using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OmahaMTG.Challenge.SolitaireChallenge;
using OmahaMTG.Challenge.ChallengeCommon;
namespace OmahaMTG.Challenge.SolitaireImplementation
{
    public class SolitaireImplementation : OmahaMTG.Challenge.SolitaireChallenge.ISolitaireChallenge
    {
        public IEnumerable<Move> Solve(Solitaire solitaire)
        {
            Rand random = new Rand();
            //Set a custom function 'CalculateMoveValues' to be used to specify the values of moves in the list of moves available.
            solitaire.MoveValueCalc = CalculateMoveValues;
            //Make sure the solitaire game is in its initial deal state.
            solitaire.Reset();
            //Get a reference to the list of moves available. This list will always be sorted descending on the values of the moves.
            MoveList moves = solitaire.MovesAvailable;

            //Play the game.
            while (solitaire.TotalMovesMade < 300 && solitaire.RoundsPlayed < 10 && moves.Size > 0)
            {
                //Based on the custom function, do we have a move that has a value greater than 0 or do we just have a single move to play?
                if (moves[0].Value > 0 || moves.Size == 1)
                {
                    solitaire.MakeMove(moves[0]);
                }
                else
                {
                    //Determine how many moves have a value greater than or equal to 0.
                    int mvs = moves.Count(delegate(Move arg) { return arg.Value >= 0; });
                    //If no moves have a value greater than or equal to 0 then just use the size.
                    mvs = mvs > 0 ? mvs : moves.Size;
                    //Make a random move. This will automatically update the list of moves available.
                    solitaire.MakeMove(moves[random.Next % mvs]);
                }
            }
            if (solitaire.IsWon)
            {
                return solitaire.MovesMade;
            }
            return null;
        }

        private int CalculateMoveValues(Solitaire solitaire, PileType from, PileType to, int cardsMoved)
        {
            if (from == to) //Flipping a card over
            {
                return 2;
            }
            else if (from >= PileType.Foundation1) //Moving a card off its foundation pile
            {
                return -2;
            }
            else if (to >= PileType.Foundation1) //Moving a card to its foundation pile
            {
                return 1;
            }
            else if (solitaire.Pile(from).FaceUpCount > cardsMoved) //Not moving all face up cards
            {
                return -1;
            }
            return 0;
        }

        public string AuthorNotes
        {
            get { return ""; }
        }
    }
}
