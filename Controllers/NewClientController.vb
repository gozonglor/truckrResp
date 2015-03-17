Imports System.Net
Imports System.Web.Http

Public Class NewClientController
    Inherits ApiController

    ' GET api/newclient
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/newclient/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/newclient
    Public Function PostValue(<FromBody()> ByVal value As clientObj) As Boolean
        Return value.save()

    End Function

    ' PUT api/newclient/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/newclient/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
