Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Object
Imports System.Web.Security.Membership
Imports PdfFileWriter

Public Class userObj
    Dim dbObj As New database

    'Class contructor: ID, firstName, lastName, phone, org, email, address, city, zip, state, notes
    Sub New()


    End Sub
    Sub New(username As String, pw As String)
        Me.username = username
        Me.pw = pw
    End Sub

    Private _username As String
    Public Property username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property


    Private _pw As String
    Public Property pw() As String
        Get
            Return _pw
        End Get
        Set(ByVal value As String)
            _pw = value
        End Set
    End Property

    Public Function validate(ByVal user1 As userObj) As Boolean
        Dim valid As New Boolean
        valid = True

        Dim nameA As String = user1.username
        Dim pwA As String = user1.pw

        Try
            Dim username = "Bob"
            Dim password = "123"

            If (username = nameA & password = pwA) Then
                valid = True
            End If

        Catch ex As Exception
            valid = False
        End Try


        Return valid

    End Function

    Public Function login(ByVal user As userObj) As String ''pass in clientObj, went from protected sub to public function?
        If ValidateUser(user.username, user.pw) Then ''if it's a valid user...
            ''originally upper line was: If ValidateUser(user.username, GetHashedPass(user.pw)) Then
            ''now switched to If ValidateUser(user.username,user.pw) Then
            Return dbObj.queryGetValue2("select guid from users where uname=@uname", {New SqlParameter("@uname", user.username)}) ''grab the associated GUID
            ''Why is the GUID constantly being returned as ""?
        Else
            Return ""
        End If
    End Function

    Private Function ValidateUser(ByVal strUsername As String, ByVal strPassword As String) As Boolean
        'Dim dbObj As New database
        'Return true if the username and password is valid, false if it isn't
        Dim dbpass As String
        Dim uname As String = dbObj.CleanSQLValue(strUsername) ''originally txtUsername.text, but changed to strUsername (passed in above)
        Dim params As String(,) = {{"@uname", uname}}
        dbpass = dbObj.queryGetValue("select pwd from users where uname = @uname", params) ''Grab the password associated with the username

        Dim result As Boolean = CBool(strPassword = dbpass) ''Compare Booleans --- are they the same?

        'if user name and pass are correct, check to make sure the user is active
        If result Then
            dbpass = dbObj.queryGetValue("select active from users where uname = @uname", params) ''Check the activity of the user -- are they active or not?
        End If

        Return CBool(dbpass = "Y") ''Return whether or not they are active
    End Function

    Private Function AssignRoles(ByVal strUsername As String) As String
        'Return a | separated list of roles this user is a member of
        If strUsername <> "Casey" Then ''originally txtUsername.Text <> "Casey" ... etc
            Return "bigboss" '"bigboss|wimpyuser"
        Else
            Return "user" 'String.Empty
        End If
    End Function

    Function GetHashedPass(ByVal aPassword As String) As String
        Return FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword, "sha1") ''original api used
        ''Return CalculateSha1(aPassword, Encoding.Default)
        'MembershipPasswordFormat.Hashed()
        'Dim s As HashAlgorithm
        ' ''s = s.
        ' ''s.Initialize()
        's.Equals(HashAlgorithm.Create(Membership.HashAlgorithmType))
        's.ComputeHash(



    End Function

End Class
