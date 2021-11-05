Imports OmahaMTG.Challenge.ChallengeCommon

''' <summary>
''' This interface defines how an entry should solve this challenge.
''' </summary>
''' <remarks>
''' The implementation of this interface is used to solve the Rubik's cube challenge.  For details on how to use this interface, please refer to the
''' <see cref="M:OmahaMTG.Challenge.Challenges.IRubiksCubeChallenge.SolveCube(OmahaMTG.Challenge.Challenges.RubiksCube)" /> method.  You will find detailed
''' examples there.
''' </remarks>
Public Interface IRubiksCubeChallenge
    Inherits IChallenge

    ''' <summary>
    ''' The is the primary method used as an entry point to solving a Rubik's cube
    ''' </summary>
    ''' <param name="cube">The scrambled cube that needs to be solved.</param>
    ''' <returns>A list of <see cref="T:OmahaMTG.Challenge.Challenges.RubiksMove" /> values.  These moves represent the turns needed to solve the given Rubik's cube.</returns>
    ''' <remarks>The challenge executor will give this method a predefined (i.e. always the same) scrambled 
    ''' <see cref="T:OmahaMTG.Challenge.Challenges.RubiksCube" />.  The goal of an implementation is to 
    ''' determine the moves necessary to solve the cube.  It's important to note that you should attempt to solve the cube in 
    ''' as few moves as are necessary because part of the scoring for your entry will be based on the number of moves it takes
    ''' to solve the cube.  In addition, your cube will be tasked with solving multiple different cube representations.  Some of these cubes have been scrambled 
    ''' with more than 400 random moves and others have been scrambled with far fewer.  In fact, your code will be tasked with solving at least one cube that has had
    ''' only 3 scramble moves applied to it.  It's important for your implementation to recognize when this is the case and come up with the appropriate solution.<br /><br />
    ''' 
    ''' <b>Optimization</b><br/>
    ''' The RubiksCube object that you are given may not contain the most efficient way for making many moves.  It is recommended that any part of the challenge that you feel 
    ''' could be made more optimal should be considered and implemented as a part of your challenge entry.<br/><br/>
    ''' <b>Scoring</b><br />
    ''' Scoring for your entry is determine by using the following formula:  100,000 - (M * 1500) - N <br />
    ''' M = The number of moves it took your implementation to solve the cube<br />
    ''' N = The number of milliseconds your code took to execute<br />
    ''' Both of these factors are important to consider while solving your cube.  You need to solve the cube in as few moves as possible, but generally speaking, the fewer moves
    ''' you find, the longer it takes.  You need to find the balancing point between time and moves.
    ''' <example>
    ''' The following illustrates how to implement a challenge entry that attempts to solve the rubiks cube by making random moves.  Note:  This is a very poor implementation and is only 
    ''' intended to show how to implement the IRubiksCubeChallenge interface.  In fact, if you attempt to submit this code you will exceed the memory bound before your entry completes.
    ''' <code lang="cs">
    '''using OmahaMTG.Challenge.Challenges;
    '''using System.Collections.Generic;
    '''using System;
    '''
    '''public class RubiksCubeImpl : IRubiksCubeChallenge
    '''{
    '''
    '''    public string AuthorNotes
    '''    {
    '''        get { return "Best solver"; }
    '''    }
    '''
    '''    public IEnumerable&lt;RubiksMove&gt; SolveCube(RubiksCube cube)
    '''    {
    '''        List&lt;RubiksMove&gt; returnValue = new List&lt;RubiksMove&gt;();
    '''
    '''        Random random = new Random();
    '''
    '''        while (!cube.IsSolved)
    '''        {
    '''            RubiksMove move = (RubiksMove)random.Next(1, 13);
    '''            returnValue.Add(move);
    '''            cube.MakeMove(move);
    '''        }
    '''
    '''        return returnValue;
    '''    }
    '''}
    ''' </code>
    ''' <code lang="vbnet">
    '''Imports OmahaMTG.Challenge.Challenges
    '''
    '''Public Class RubiksCubeImpl
    '''    Implements IRubiksCubeChallenge
    '''
    '''    Public ReadOnly Property AuthorNotes As String Implements ChallengeCommon.IChallenge.AuthorNotes
    '''        Get
    '''            Return "Best solver"
    '''        End Get
    '''    End Property
    '''
    '''    Public Function SolveCube(ByVal cube As RubiksCube) As IEnumerable(Of RubiksMove) Implements IRubiksCubeChallenge.SolveCube
    '''        Dim returnValue As New List(Of RubiksMove)
    '''
    '''        Dim random As New Random
    '''
    '''        While Not cube.IsSolved
    '''            Dim move As RubiksMove = random.Next(1, 13)
    '''            returnValue.Add(move)
    '''            cube.MakeMove(move)
    '''        End While
    '''
    '''        Return returnValue
    '''    End Function
    '''End Class
    ''' </code>
    ''' </example>
    ''' </remarks>
    Function SolveCube(ByVal cube As RubiksCube) As IEnumerable(Of RubiksMove)
End Interface
