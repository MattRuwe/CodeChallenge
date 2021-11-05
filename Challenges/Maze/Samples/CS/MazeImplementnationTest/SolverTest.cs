using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmahaMTG.Challenge.Challenges;
using OmahaMTG.Sample.MazeImplementation;

[TestClass()]
public class SolverTest
{
    [TestMethod()]
    public void MakeMoveTest()
    {
        int moves = 0;
        Maze maze = new Maze(10);
        maze.GenerateMaze(100);
        Solver solver = new Solver();
        Direction playerMove = default(Direction);
        bool madeAnIllegalMove = false;
        long maxMoves = (long)(Math.Pow(maze.GetSize(), 2) * 2);

        solver.StartNewMaze((int size) => { return maze.GetSubMaze(maze.CurrentIndex, size); });

        while (!maze.CurrentCell.IsEnd && moves <= maxMoves)
        {
            playerMove = solver.MakeMove(maze.CurrentCell, maze.GetSubMaze(maze.CurrentIndex, 3));

            try
            {
                maze.MoveCurrentCell(playerMove);
                moves += 1;
            }
            catch (IllegalMoveException)
            {
                madeAnIllegalMove = true;
                break; // TODO: might not be correct. Was : Exit While
            }
        }

        Assert.IsTrue(maze.CurrentCell.IsEnd);
        Assert.IsFalse(madeAnIllegalMove);
    }

}
