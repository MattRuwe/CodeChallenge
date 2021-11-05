Imports OmahaMTG.Challenge.Challenges
Imports System.Numerics
Imports OmahaMTG.Challenge.ChallengeCommon
Imports System.Threading.Tasks

Public Class PrimeNumbersImplementation
    Implements IPrimeNumberChallenge

    Public ReadOnly Property AuthorNotes As String Implements IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Public Function GetNextPrime(startingValue As BigInteger) As BigInteger Implements IPrimeNumberChallenge.GetNextPrime

        Dim currentValue As BigInteger = startingValue
        Dim returnValue As BigInteger = currentValue
        If currentValue.IsEven Then
            currentValue += 1
        End If

        Parallel.For(1, CType(Integer.MaxValue, Long), New Action(Of Long, ParallelLoopState)(
                     Sub(i As Long, state As ParallelLoopState)
                         Dim localCurrentValue As BigInteger = currentValue + (i * 2)
                         If IsPrime(localCurrentValue) Then
                             returnValue = localCurrentValue
                             state.Break()
                         End If
                     End Sub))

        Return returnValue
    End Function

    Public Shared Function IsPrime(p As BigInteger) As Boolean
        If p < 2 Then
            Return False
        End If
        If p = 2 Then
            Return True
        End If
        If (p And 1) = 0 Then
            Return False
        End If

        Dim s As BigInteger = p - 1
        While (s And 1) = 0
            s >>= 1
        End While

        Dim x As BigInteger = 2 * BigInteger.Log(p) * BigInteger.Log(p)
        For b As BigInteger = 2 To x
            Dim temp As BigInteger = s
            Dim [mod] As BigInteger = BigInteger.ModPow(b, temp, p)
            While temp <> p - 1 AndAlso [mod] <> 1 AndAlso [mod] <> p - 1
                [mod] = ([mod] * [mod]) Mod p
                temp <<= 1
            End While
            If [mod] <> p - 1 AndAlso (temp And 1) = 0 Then
                Return False
            End If
        Next
        Return True
    End Function

End Class
