Public Interface ITurnChallenge(Of TEnvironment As ITurnEnvironment, TMove As ITurnMove)
    Property AuthorNotes As String

    Function MakeMove(environment As TEnvironment) As TMove
End Interface
