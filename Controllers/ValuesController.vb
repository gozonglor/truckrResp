Imports System.Net
Imports System.Web.Http
Imports System.Data.SqlClient


'<KeyAuthorize>
Public Class ValuesController
    Inherits ApiController

    Dim dbObj As New database
    Dim ekeyG As String

    ' GET api/values
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/values/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/values
    Public Function PostValue(<FromBody()> ByVal value As String) As String
        ''passing in the user name into the body
        ''check to see if the 
        ''Dim chosen As New clientObj
        ''chosen = value
        ''HttpContext.Current.Items[chosen] 

        ''Given an id, look up the clientObj in the database using the id

        ''open the database 
        ''throw in the query
        ''check to see if everything is matching
        Dim ekey As String = ""

        Try
            ekey = dbObj.queryGetValue2("select guid from users where uname=@uname", {New SqlParameter("@uname", value)})
            Dim ekeyArray As Byte() = ASCIIEncoding.ASCII.GetBytes(ekey)
            ekey = System.Convert.ToBase64String(ekeyArray)
            ekeyG = ekey
        Catch ex As Exception
            ekey = "EMPTY"
        End Try
        Return ekey
    End Function

    ' PUT api/values/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/values/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
