''' <summary>
''' The base interface for all challenge implementations.  
''' </summary>
''' <remarks>This interface is not intended to be directly implemented except when creating a new challenge.</remarks>
Public Interface IChallenge
    ' ''' <summary>
    ' ''' The name that you would like to be associated with your results on the web site
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'ReadOnly Property AuthorName() As String

    ' ''' <summary>
    ' ''' Your OmahaMTG username
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks>This value is verified during the execution of your entry</remarks>
    'ReadOnly Property OmahaMtgUsername() As String

    ''' <summary>
    ''' Notes that will be read when your challenge is being executed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Depending on the challenge, you may be permitted to return results through this property.  The challenge executor will decide at runtime 
    ''' if and how many characters may be returned.  Most challenges will not allow you to return any information due to possibility of cheating.  However,
    ''' if you have problems with your challenge, the code challenge administrator can view what notes you have made, and relay those if appropriate.  This will
    ''' only be considered in extreme cases.</remarks>
    ReadOnly Property AuthorNotes() As String
End Interface
