Imports System.Numerics

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.Challenges
Imports System.Security.Cryptography


'''<summary>
'''This is a test class for PrimeNumbersExecutorTest and is intended
'''to contain all PrimeNumbersExecutorTest Unit Tests
'''</summary>
<TestClass()> _
Public Class PrimeNumbersExecutorTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    <TestMethod()> _
    Public Sub RunChallenge()
        Dim target As PrimeNumbersExecutor = New PrimeNumbersExecutor()

        target.RunChallenge(New PrimeNumbersImplementation)
    End Sub

    Private Function GetRandomBigInt(sizeInBytes As Integer) As BigInteger
        Dim rng As New RNGCryptoServiceProvider

        Dim randomBytes(sizeInBytes) As Byte

        rng.GetBytes(randomBytes)

        For i As Integer = 0 To randomBytes.Count - 1
            Debug.Write(randomBytes(i) & ", ")
        Next

        Dim returnValue As New BigInteger(randomBytes)

        Return returnValue
    End Function

End Class
