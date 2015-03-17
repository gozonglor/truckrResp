Imports System.Net
Imports System.Web.Http

<KeyAuthorize>
Public Class LocationController
    Inherits ApiController

    ' GET api/location
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/location/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/location
    Public Sub PostValue(<FromBody()> ByVal value As String)
        ''Post the city
        ''check a list of cities to find the match
        ''return matching zipcode
    End Sub

    ' PUT api/location/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/location/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
