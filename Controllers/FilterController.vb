Imports System.Net
Imports System.Web.Http


Public Class FilterController
    Inherits ApiController


    'FilterController is a controller that manages what the user inputs into the clients.html form and returns related clients.

    ' GET api/filter
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/filter/5
    Public Function GetValue(ByVal id As Integer) As clientObj
        Dim relationship1 As New List(Of String)
        relationship1.Add("Computer Donor")
        Dim client1 As New clientObj(relationship1, "Active", 1, "Gabriel", "Garcia Marquez", "000-000-0000", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", 1, "N/A")
        Return client1
    End Function

    ' POST api/filter
    Public Function PostValue(<FromBody()> ByVal value As clientObj) As List(Of clientObj)
        'Creates the client object and makes it a donor before checking for it in the list of clients
        ''Dim clientA As New clientObj(0, "", "", "", "", "", "", "", "", 0, "")
        ''clientA = value
        ''value.makeDonor(value) ''make the client a donor 

        Dim emptyList As New List(Of clientObj)
        Dim relationship1 As New List(Of String)

        emptyList.Add(New clientObj(relationship1, "NULL", 0, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", 0, "NULL"))
        Try
            Return value.filterClient(value)
            'Return value.checkForClient(value)
        Catch ex As Exception
            Return value.filterClient(value)
        End Try
    End Function

    ' PUT api/filter/5
    Public Function PutValue(<FromBody()> ByVal value As clientObj)
        ''JQuery Validation: Make sure all fields are filled
        value.id = 1
        Return value
    End Function

    ' DELETE api/filter/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class