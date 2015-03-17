Imports System.Net
Imports System.Web.Http
Imports System.Web.SessionState


'<KeyAuthorize>
Public Class ConfirmController
    Inherits ApiController

    'Public key As String()

    'Dim sess As New SessionIDManager

    ' GET api/test
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/test/5
    Public Function GetValue(ByVal id As Integer) As userObj
        ''For testing purposes
        Dim user1 As New userObj
        user1.username = "glor"
        user1.pw = "7713989Gl"
        Return user1
    End Function

    ' POST api/test
    Public Function PostValue(<FromBody()> ByVal user As userObj) As String
        ''Error handling: JQuery receives the code and checks if its "
        ''Instead of boolean, send back exception object
        ''JQuery can catch the error object, display or do what it has to do


        'Try
        '    Dim user1 As New userObj
        '    user1 = login
        '    Dim allow As  Boolean = login.validate(login)
        '    Return allow
        'Catch ex As Exception
        '    Return False
        'End Try

        Try
            Dim ekey = user.login(user) ''returning the encryption key?
            Return ekey
            'If ekey = "" Then
            '    Return False
            'Else
            '    ''Dim sess As HttpSessionState()
            '    ''sess("ekey") = ekey
            '    ''HttpContext.Current("ekey") = ekey
            '    ''HttpContext.Session("ekey") = ekey
            '    'http://stackoverflow.com/questions/17947975/the-session-object-is-null-in-asp-net-mvc-4-webapplication-once-deployed-to-iis
            '    ''remove sessionstatemodule
            '    ''http://stackoverflow.com/questions/10629882/asp-net-mvc-session-is-null
            '    ''https://nickstips.wordpress.com/2011/07/01/asp-net-mvc-ensure-session-variable-is-always-populated/
            '    'If ((HttpContext.Current.Session) Is Nothing) Then

            '    '    Return False ''if the current session is still nothing, return false
            '    'End If

            '    'HttpContext.Current.Session.Add("userkey", ekey)
            '    ''http://stackoverflow.com/questions/560084/session-variables-in-asp-net-mvc



            '    ''USING TEMP DATA (DATA IS DISCARDED AFTER NEXT REQUEST COMPLETES)
            '    ''Dim tempData As TempDataDictionary
            '    ''tempData.Add("userkey", ekey)


            '    ''key = ekey
            '    ''TempDataDictionary.Add()

            '    ''Return True
            '    Return True
            'End If
            ''Return True
        Catch ex As Exception
            Return ""
        End Try

    End Function

    ' PUT api/test/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/test/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub

    'Protected Overrides Sub Initialize(requestContext As System.Web.Routing.RequestContext)
    '    MyBase.Initialize(ControllerContext)
    'End Sub
End Class
