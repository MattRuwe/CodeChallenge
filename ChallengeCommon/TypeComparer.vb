Public Class TypeComparer
    Implements IComparer(Of Type)
    Implements IEqualityComparer(Of Type)

    Public Function Compare(ByVal x As Type, ByVal y As Type) As Integer Implements IComparer(Of Type).Compare
        Return String.Compare(x.FullName, y.FullName)
    End Function

    Public Overloads Function Equals(ByVal x As Type, ByVal y As Type) As Boolean Implements IEqualityComparer(Of Type).Equals
        Return x.FullName.Equals(y.FullName)
    End Function

    Public Overloads Function GetHashCode(ByVal obj As Type) As Integer Implements IEqualityComparer(Of Type).GetHashCode

    End Function
End Class
