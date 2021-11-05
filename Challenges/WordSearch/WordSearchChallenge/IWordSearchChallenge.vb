Imports OmahaMTG.Challenge.ChallengeCommon

''' <summary>
''' This interface defines how an entry should solve this challenge.
''' </summary>
''' <remarks>
''' The implementation of this interface is used to solve the word search challenge.
''' </remarks>
Public Interface IWordSearchChallenge
    Inherits IChallenge

    ''' <summary>
    ''' This method is used as the challenge entry point for solving the word search challenge
    ''' </summary>
    ''' <param name="puzzle">The puzzle that contains the board (a 2 dimensional array of characters) and the list of words to find</param>
    ''' <returns>An enumerable of FoundWord which contains the location of each of the words on the board</returns>
    ''' <remarks>Words can be forward, backward, and diagonal.  It should not be assumed that there is only once instance of a given word within a board.
    ''' If you find more than one instance, you will be awarded points for each instance found.  Your entry will be score using the following formula:<br /><br />
    ''' W * 500 + (30000 – M) – (U * 500) <br />
    ''' W = The number of successfully matched words <br />
    ''' M = The number of milliseconds your entry took to run (if 30,000 - M is less than 0, then 0 will be used instead) <br />
    ''' U = The number of unmatched words (i.e. words that were incorrectly returned) <br />
    ''' For example:  Your entry found 60 words in 15,000 millisecond, and incorrectly matched 10 words.  Your final score would be 60 * 500 + (30,000 - 15,000) - (10 * 500) = 40,000.</remarks>
    Function SolveWordSearch(puzzle As Puzzle) As IEnumerable(Of FoundWord)
End Interface
