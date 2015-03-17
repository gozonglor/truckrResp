Imports System.Net
Imports System.Web.Http

'<KeyAuthorize>
Public Class ClientController
    Inherits ApiController

    ' GET api/client
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/client/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/client
    Public Function PostValue(<FromBody()> ByVal value As clientObj) As List(Of clientObj)
        Dim tempList As New List(Of clientObj)
        Try
            Dim value2 As New clientObj
            value2.firstName = "Gozong"
            tempList.Add(value)
            tempList.Add(value2)
            Return tempList
        Catch ex As Exception
            Return tempList ''return empty list, so js side can check if list is empty or not..
        End Try
    End Function

    ' PUT api/client/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/client/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
