Imports OmahaMTG.Challenge.ChallengeCommon
Imports System.Numerics

Public Interface IPrimeNumberChallenge
    Inherits IChallenge

    Function GetNextPrime(startingValue As BigInteger) As BigInteger

End Interface
