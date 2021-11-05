Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.Manager
Imports System.IO



'''<summary>
'''This is a test class for ChallengeManagerTest and is intended
'''to contain all ChallengeManagerTest Unit Tests
'''</summary>
<TestClass()> _
Public Class ChallengeManagerTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
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

    <TestMethod()>
    Public Sub TestMethod()
        Dim currentTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.GetSystemTimeZones().Where(Function(tz) tz.BaseUtcOffset.Hours = -7).Select(Function(tz) tz.StandardName).FirstOrDefault()))
    End Sub
End Class
