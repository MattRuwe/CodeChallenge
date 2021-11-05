Imports System.Numerics
Imports System.Security.Cryptography
Imports OmahaMTG.Challenge.ExecutionCommon

Public Class PrimeNumbersExecutor
    Inherits ChallengeExecutorBase(Of IPrimeNumberChallenge)

    Protected Overrides ReadOnly Property MaxAuthorNotesLength As Integer
        Get
            Return 0
        End Get
    End Property

    Protected Overrides Sub RunChallengeOverride()
        Dim sw As New Stopwatch

        'Dim lowerBound As BigInteger = New BigInteger(New Byte() {1, 132, 228, 218, 28, 71, 46, 139, 189, 21, 166, 174, 223, 121, 32, 156, 3, 160, 138, 68, 226, 80, 238, 206, 162, 32, 33, 221, 114, 212, 56, 179, 192, 77, 214, 62, 232, 134, 25, 180, 6, 149, 116, 254, 139, 159, 52, 133, 198, 114, 216, 155, 241, 86, 218, 115, 25, 182, 130, 1, 233, 213, 183, 200, 193})
        Dim lowerBound As BigInteger = BigInteger.Parse("534912749") '193749321923484198742123491234581923847927009124387676093240093485080293485203948520348957523")

        Dim primesFound As New List(Of BigInteger)
        Dim currentCandidate As BigInteger = lowerBound

        Dim iterations As Long = 0
        Dim iterationErrors As Long = 0
        Dim verifiedPrimesFound As Long = 0
        Dim falsePrimesFound As Long = 0
        Dim maxPrime As BigInteger = lowerBound

        sw.Start()
        Do While sw.Elapsed.TotalSeconds < 60
            Try
                currentCandidate = Challenge.GetNextPrime(lowerBound + 1)

                Dim count = (From pf In primesFound Where pf = currentCandidate).LongCount

                If currentCandidate >= lowerBound AndAlso IsPrime(currentCandidate) AndAlso count = 0 Then
                    primesFound.Add(currentCandidate)
                    verifiedPrimesFound += 1
                    lowerBound = currentCandidate
                    If maxPrime < currentCandidate Then
                        maxPrime = currentCandidate
                    End If
                Else
                    falsePrimesFound += 1
                End If

            Catch ex As Exception
                iterationErrors += 1
                Console.WriteLine(ex.ToString)
            End Try

            iterations += 1
        Loop
        sw.Stop()


        Dim result As New ChallengeResult With {
            .ResultMessage = String.Format("Found {0} distinct prime numbers in {1} iterations.  There were {2} false primes found amongst the results.  Last prime found was {3} places away from the starting position." & If(iterationErrors > 0, String.Format("  There were also {0} error(s) that occurred.", iterationErrors), String.Empty), verifiedPrimesFound, iterations, falsePrimesFound, (maxPrime - lowerBound).ToString),
            .Score = verifiedPrimesFound,
            .Successful = True,
            .DurationTicks = sw.ElapsedTicks}

        MyBase.ResultsAvailable(result)


    End Sub

    'Public Function IsPrime(candidate As BigInteger) As Boolean
    '    ' Test whether the parameter is a prime number.
    '    If (candidate And 1) = 0 Then
    '        If candidate = 2 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    End If
    '    ' Note:
    '    ' ... This version was changed to test the square.
    '    ' ... Original version tested against the square root.
    '    ' ... Also we exclude 1 at the very end.
    '    Dim i As BigInteger = 3
    '    While (i * i) <= candidate
    '        If (candidate Mod i) = 0 Then
    '            Return False
    '        End If
    '        i += 2
    '    End While
    '    Return candidate <> 1
    'End Function

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
