Imports OmahaMTG.Challenge.ChallengeCommon

''' <summary>
''' Implement this interface to solve the Maze challenge
''' </summary>
''' <remarks>
''' The constructor for your entry is called only once to solve multiple mazes.  The maze executor will repeatedly call the 
''' <see cref="M:OmahaMTG.Challenge.Challenges.IMazeChallenge.MakeMove(OmahaMTG.Challenge.Challenges.Cell,OmahaMTG.Challenge.Challenges.Maze)" /> 
''' method until you have reached the ending position within the maze.  
''' Whenever a new maze starts, the StartMaze method will be called so you can reset any internal state you might be using.  
''' <br /><strong>Scoring:</strong> The formula used for scoring is: Math.Max(100000 - (100000 * (TotalMoves / (Math.Pow(maze_size, 2) * 2))) - elapsed_milliseconds - RevealPointsLost, 0) + (solved_maze ? 5000 : 0).  In 
''' essence, you start with 100,000 points and are given the enough points to traverse the entire maze twice (accounts for back tracking).  The percentage of the maximum number 
''' of moves that your entry uses will be taken away from your starting total of 100,000 points.  After these points are removed, you also lose 1 point for every milliseconds 
''' that elapses.  If you successfully solve the maze, 5,000 points are awarded (after the other calculations are finished).</remarks>
Public Interface IMazeChallenge
    Inherits IChallenge

    ''' <summary>
    ''' This method is called whenever the executor is starting a new maze.
    ''' </summary>
    ''' <param name="revealVicinity">Contains a reference to a func that will return a sub maze of the size you request.</param>
    ''' <remarks>
    ''' Please note that invoking the revealVicinity function will reduce your overall score using the following formula:  Math.Max(Math.Pow(10, size_requested / maze_size * 8), 500).<br />
    ''' Also, note that this effect is cumulative meaning that each time this method is called, it will subtract from your overall score.  You are able to solve the maze 
    ''' without ever calling this method, but your results will be random at best.  You should take advantage of this capability to refine your solution.
    '''</remarks>
    Sub StartNewMaze(revealVicinity As Func(Of Integer, Maze))

    ''' <summary>
    ''' This method is called when the executor is ready to get your entry's next move
    ''' </summary>
    ''' <param name="cell">The current cell</param>
    ''' <param name="vicinity">A 3x3 sub maze that shows the vicinity around the current cell.</param>
    ''' <returns>A direction to move next.  Note:  If the direction causes the movement through a wall, the entry will be stopped and receive a score of 0.</returns>
    ''' <remarks>This method is invoked repeatidly until either the end cell is found or the maximum number of moves (MazeSize^2 * 2) is reached.</remarks>
    Function MakeMove(cell As Cell, vicinity As Maze) As Direction
End Interface
