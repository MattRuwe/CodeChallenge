Imports System.Collections.Generic

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.Challenges



'''<summary>
'''This is a test class for GuessNumberExecutorTest and is intended
'''to contain all GuessNumberExecutorTest Unit Tests
'''</summary>
<TestClass()> _
Public Class GuessNumberExecutorTest


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
            testContextInstance = Value
        End Set
    End Property

    '''<summary>
    '''A test for Sequence7
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence7Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor()
        Dim actual As List(Of Integer)
        actual = target.Sequence7
        Debug.WriteLine("Sequence7Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence1
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence1Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence1
        Debug.WriteLine("Sequence1Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence10
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence10Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence10
        Debug.WriteLine("Sequence10Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence11
    '''</summary>
    <TestMethod()> _
    Public Sub Sequence11Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence11
        Debug.WriteLine("Sequence11Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence12
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence12Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence12
        Debug.WriteLine("Sequence12Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence13
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence13Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence13
        Debug.WriteLine("Sequence13Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence14
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence14Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence14
        Debug.WriteLine("Sequence14Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence15
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence15Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence15
        Debug.WriteLine("Sequence15Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence17
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence17Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence17
        Debug.WriteLine("Sequence17Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence18
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence18Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence18
        Debug.WriteLine("Sequence18Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence19
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence19Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence19
        Debug.WriteLine("Sequence19Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence2
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence2Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence2
        Debug.WriteLine("Sequence2Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence20
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence20Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence20
        Debug.WriteLine("Sequence20Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence21
    '''</summary>
    <TestMethod()> _
    Public Sub Sequence21Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence21
        Debug.WriteLine("Sequence21Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence21
    '''</summary>
    <TestMethod()> _
    Public Sub Sequence22Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence21
        Debug.WriteLine("Sequence22Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence3
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence3Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence3
        Debug.WriteLine("Sequence3Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence4
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence4Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence4
        Debug.WriteLine("Sequence4Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence5
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence5Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence5
        Debug.WriteLine("Sequence5Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence6
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence6Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence6
        Debug.WriteLine("Sequence6Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence8
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence8Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence8
        Debug.WriteLine("Sequence8Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Sequence9
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Sequence9Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence9
        Debug.WriteLine("Sequence9Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    '''<summary>
    '''A test for Seuqnce16
    '''</summary>
    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Seuqnce16Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence16
        Debug.WriteLine("Sequence16Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Seuqnce23Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence23
        Debug.WriteLine("Sequence23Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Seuqnce24Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence24
        Debug.WriteLine("Sequence24Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub

    <TestMethod(), _
     DeploymentItem("OmahaMTG.Challenge.GuessTheNumberExecutor.dll")> _
    Public Sub Seuqnce25Test()
        Dim target As GuessNumberExecutor_Accessor = New GuessNumberExecutor_Accessor() ' TODO: Initialize to an appropriate value
        Dim actual As List(Of Integer)
        actual = target.Sequence25
        Debug.WriteLine("Sequence25Test" & actual.Select(Function(i) i.ToString()).Aggregate(Function(i, j) i & "," & j))
    End Sub
End Class
