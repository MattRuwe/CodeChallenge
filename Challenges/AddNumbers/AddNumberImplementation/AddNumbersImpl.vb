Imports OmahaMTG.Challenge.Challenges
Imports System.IO
Imports System.Reflection
Imports System.Net

Public Class AddNumbersImpl
    Implements IAddNumberChallenge

    Public Function Add(ByVal x As Integer, ByVal y As Integer) As Integer Implements IAddNumberChallenge.Add
        'Dim file As New StreamReader("c:\test.txt")
        'Dim web As HttpWebRequest = HttpWebRequest.Create("http://www.yahoo.com")
        'Dim response As WebResponse = web.GetResponse()
        Return (x + y)
    End Function

    Public ReadOnly Property AuthorNotes As String Implements ChallengeCommon.IChallenge.AuthorNotes
        Get
            Return "No notes"
        End Get
    End Property
End Class
