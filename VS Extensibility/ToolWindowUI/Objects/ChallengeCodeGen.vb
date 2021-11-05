Imports System.CodeDom.Compiler
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports Microsoft.CSharp
Imports OmahaMTG.Challenge.ChallengeCommon
Imports System.CodeDom
Imports System.Threading

<Serializable()>
Public Class ChallengeCodeGen

    Public Function Generate(rootPath As String) As String
        Dim ads As New AppDomainSetup() With
        {
            .PrivateBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location),
            .ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location)
        }

        Dim loadingDomain As AppDomain = AppDomain.CreateDomain("loadingDOmain", Nothing, ads)
        loadingDomain.SetData("rootPath", rootPath)

        loadingDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf GenerateCodeSafe))

        Dim returnValue As String = loadingDomain.GetData("implementationCode")

        AppDomain.Unload(loadingDomain)

        Return returnValue
    End Function

    Private Sub GenerateCodeSafe()
        Dim rootPath As String = AppDomain.CurrentDomain.GetData("rootPath")
        Dim challengeName As String

        Dim challengeType As Type = FindChallengeType(rootPath)

        challengeName = challengeType.Name.Substring(1, challengeType.Name.Length - 1)
        Dim currentUserName As String = Thread.CurrentPrincipal.Identity.Name.Replace(" ", "")

        Dim ns As New CodeNamespace(String.Format("OmahaMTGCodeChallenge.{0}", challengeName))

        Dim classImpl As New CodeTypeDeclaration(challengeName)
        classImpl.BaseTypes.Add(challengeType)
        ns.Types.Add(classImpl)
        Dim csProvider As New CSharpCodeProvider()

        'Add the methods
        AddMethods(challengeType, classImpl)

        'Add the properties
        AddProperties(challengeType, classImpl)


        Dim options As New CodeGeneratorOptions()
        options.IndentString = Space(3)
        options.BracingStyle = "C"
        options.BlankLinesBetweenMembers = True

        Dim sb As New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        csProvider.GenerateCodeFromNamespace(ns, sw, options)

        Dim implementationCode = sb.ToString()

        AppDomain.CurrentDomain.SetData("implementationCode", implementationCode)

    End Sub

    Private Sub AddMethods(challengeType As Type, classImpl As CodeTypeDeclaration)
        For Each mi As MethodInfo In challengeType.GetMethods(BindingFlags.Public Or BindingFlags.Instance)
            If Not mi.IsSpecialName Then
                Dim codeMethod As New CodeMemberMethod()
                codeMethod.Attributes = MemberAttributes.Public Or MemberAttributes.Final
                codeMethod.Name = mi.Name
                If mi.ReturnType IsNot Nothing Then
                    codeMethod.ReturnType = New CodeTypeReference(mi.ReturnType)
                End If
                For Each param As ParameterInfo In mi.GetParameters()
                    codeMethod.Parameters.Add(New CodeParameterDeclarationExpression(param.ParameterType, param.Name))
                Next

                codeMethod.Statements.Add(New CodeThrowExceptionStatement(New CodeObjectCreateExpression(GetType(NotImplementedException))))
                classImpl.Members.Add(codeMethod)
            End If
        Next

        For Each interfaceType As Type In challengeType.GetInterfaces
            AddMethods(interfaceType, classImpl)
        Next
    End Sub

    Private Sub AddProperties(challengeType As Type, classImpl As CodeTypeDeclaration)
        For Each pi As PropertyInfo In challengeType.GetProperties()
            Dim codeProp As New CodeMemberProperty()
            codeProp.Attributes = MemberAttributes.Public Or MemberAttributes.Final
            codeProp.Name = pi.Name
            If pi.CanRead Then
                codeProp.HasGet = True
                codeProp.GetStatements.Add(New CodeThrowExceptionStatement(New CodeObjectCreateExpression(GetType(NotImplementedException))))
            End If
            If pi.CanWrite Then
                codeProp.HasSet = True
                codeProp.SetStatements.Add(New CodeThrowExceptionStatement(New CodeObjectCreateExpression(GetType(NotImplementedException))))
            End If
            codeProp.Type = New CodeTypeReference(pi.PropertyType)
            classImpl.Members.Add(codeProp)
        Next

        For Each interfaceType As Type In challengeType.GetInterfaces
            AddProperties(interfaceType, classImpl)
        Next
    End Sub

    Private Function FindChallengeType(rootPath As String) As Type
        Dim returnValue As Type = Nothing
        Dim assemblyFiles() As String = Directory.GetFiles(rootPath, "*.dll")
        For Each assemblyFile In assemblyFiles
            Dim assem As Assembly = Assembly.LoadFrom(assemblyFile)

            For Each assemType As Type In assem.GetTypes
                For Each interfaceType As Type In assemType.GetInterfaces
                    If interfaceType.FullName = GetType(IChallenge).FullName Then
                        returnValue = assemType
                        Exit For
                    End If
                Next
                If returnValue IsNot Nothing Then
                    Exit For
                End If
            Next
            If returnValue IsNot Nothing Then
                Exit For
            End If
        Next
        Return returnValue
    End Function

End Class
