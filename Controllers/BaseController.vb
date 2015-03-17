Imports System.Net
Imports System.Web.Http

Public Class BaseController
    Inherits ApiController

    ' GET api/base
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/base/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/base
    Public Function PostValue(<FromBody()> ByVal value As String) As String
        Return value
    End Function

    ' PUT api/base/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/base/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
