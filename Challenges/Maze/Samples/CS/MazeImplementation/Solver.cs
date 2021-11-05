using System;
using System.Collections.Generic;
using System.Data;
using OmahaMTG.Challenge.Challenges;
using System.Linq;

namespace OmahaMTG.Sample.MazeImplementation
{
    public class Solver : IMazeChallenge
    {

        public string AuthorNotes
        {
            get { return string.Empty; }
        }


        private Direction _lastMove;
        public void StartNewMaze(System.Func<int, Maze> revealVicinity)
        {
            _lastMove = Direction.None;
        }

        public Direction MakeMove(Cell cell, Maze vicinity)
        {
            Direction returnValue = Direction.None;

            //Determine the opposite move from the last so we know not to go back the way we came
            Direction oppositeMove = GetOppositeMove(_lastMove);

            //Generate a randomly ordered list of moves to try from the current cell
            List<Direction> possibleMoves = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };
            possibleMoves = possibleMoves.OrderBy(val => Guid.NewGuid()).ToList();

            //For each of our randomly generated moves, see if we can go that direction
            foreach (Direction move in possibleMoves)
            {
                if (cell.IsValidMove(move) && move != oppositeMove)
                {
                    //We have a winner!
                    //We haven't been this way, and it is a valid move
                    returnValue = move;
                    break;
                }
            }

            //If we didn't find a move that works, we need to go back the direction that we came
            if (returnValue == Direction.None)
            {
                returnValue = oppositeMove;
            }

            //Record the last move so it can be used in the next iteration
            _lastMove = returnValue;

            return returnValue;
        }

        /// <summary>
        /// Get the direction opposite of the one passed in
        /// </summary>
        /// <param name="d">The direction to get the opposite of</param>
        /// <returns>The opposite direction of parameter d</returns>
        private Direction GetOppositeMove(Direction d)
        {
            switch (d)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    return Direction.None;
            }
        }
    }
}