Imports System.Data.SqlClient

Public Class clientObj

    Dim dbObj As New database

    'Class contructor: ID, firstName, lastName, phone, org, email, address, city, zip, state, notes
    Sub New()

    End Sub
    Sub New(contact_relationship As List(Of String), contact_status As String, id As Integer, firstName As String, lastName As String, phone As String, org As String,
            email As String, address As String, address2 As String, city As String, state As String, zip As String,
            notes As String, Optional username As String = "", Optional key As String = "", Optional midName As String = "")
        Me.contact_relationship = contact_relationship
        Me.contact_status = contact_status
        Me.id = id
        Me.firstName = firstName
        Me.lastName = lastName
        Me.phone = phone
        Me.org = org
        Me.email = email
        Me.address = address
        Me.address2 = address2
        Me.city = city
        Me.zip = zip
        Me.state = state
        Me.notes = notes
        Me.username = username
        Me.key = key
        Me.midName = midName
    End Sub

    Private _id As String
    Public Property id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _firstName As String
    Public Property firstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _midName As String
    Public Property midName() As String
        Get
            Return _midName
        End Get
        Set(ByVal value As String)
            _midName = value
        End Set
    End Property

    Private _lastName As String
    Public Property lastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _phone As String
    Public Property phone() As String
        Get
            Return _phone
        End Get
        Set(ByVal value As String)
            _phone = value
        End Set
    End Property

    Private _org As String
    Public Property org() As String
        Get
            Return _org
        End Get
        Set(ByVal value As String)
            _org = value
        End Set
    End Property

    Private _email As String
    Public Property email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Private _address As String
    Public Property address() As String
        Get
            Return _address
        End Get
        Set(ByVal value As String)
            _address = value
        End Set
    End Property

    Private _city As String
    Public Property city() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    'Private _zip As Double
    'Public Property zip() As Double
    '    Get
    '        Return _zip
    '    End Get
    '    Set(ByVal value As Double)
    '        _zip = value
    '    End Set
    'End Property


    Private _zip As String
    Public Property zip() As String
        Get
            Return _zip
        End Get
        Set(ByVal value As String)
            _zip = value
        End Set
    End Property

    Private _state As String
    Public Property state() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    Private _notes As String
    Public Property notes() As String
        Get
            Return _notes
        End Get
        Set(ByVal value As String)
            _notes = value
        End Set
    End Property

    Private _address2 As String
    Public Property address2() As String
        Get
            Return _address2
        End Get
        Set(ByVal value As String)
            _address2 = value
        End Set
    End Property

    Private _contact_relationship As List(Of String)
    Public Property contact_relationship() As List(Of String)
        Get
            Return _contact_relationship
        End Get
        Set(value As List(Of String))
            _contact_relationship = value
        End Set
    End Property

    Private _contact_status As String
    Public Property contact_status() As String
        Get
            Return _contact_status
        End Get
        Set(value As String)
            _contact_status = value
        End Set
    End Property

    Private _username As String
    Public Property username() As String
        Get
            Return _username
        End Get
        Set(value As String)
            _username = value
        End Set
    End Property

    Private _key As String
    Public Property key() As String
        Get
            Return _key
        End Get
        Set(value As String)
            _key = value
        End Set
    End Property


    Public Function makeDonor(ByVal client As clientObj) As clientObj
        Dim relationship1 As New List(Of String)
        relationship1.Add("Computer Donor")
        client.contact_relationship = relationship1
        Return client
    End Function

    Public Function addNewClient(ByVal client As clientObj) As Boolean

        Try
            'dbObj.insertmax2("insert into contact(originID, fname, mi, lname, phone, email, business, addy, addy2, city, state, zip, needs, notes, county, locationID, altName, stripeCustID",
            '                 {New SqlParameter("@fname", client.firstName
            '                                   "@lname", client.lastName
            '                                    )})

        Catch ex As Exception

        End Try

        Return True

    End Function

    Public Function filterClient(ByVal client As clientObj) As List(Of clientObj)
        Dim filter As String = ""
        Using dbObj.oDR

            dbObj.setReader("SELECT contactID, fname, mi, lname, phone, email, addy, business, city, state, zip from contact")

            If Not (client.firstName.Equals("")) Then
                filter += "fname LIKE '%" + client.firstName + "%'"
            End If

            If Not (client.midName.Equals("")) Then
                If filter = "" Then
                    filter += "mi LIKE '%" + client.midName + "%'"
                Else
                    filter += " and mi LIKE '%" + client.midName + "%'"
                End If
            End If

            If Not (client.lastName.Equals("")) Then
                If filter = "" Then
                    filter += "lname LIKE '%" + client.lastName + "%'"
                Else
                    filter += " and lname LIKE '%" + client.lastName + "%'"
                End If
            End If

            If Not (client.phone.Equals("")) Then
                If filter = "" Then
                    filter += "phone LIKE '%" + client.phone + "%'"
                Else
                    filter += " and phone LIKE '%" + client.phone + "%'"
                End If
            End If

            If Not (client.city.Equals("")) Then
                If filter = "" Then
                    filter += "city LIKE '%" + client.city + "%'"
                Else
                    filter += " and city LIKE '%" + client.city + "%'"
                End If
            End If

            'If Not (client.zip.Equals("")) Then
            If Not (client.zip.Equals("")) Then
                If filter = "" Then
                    filter += "zip LIKE '%" + client.zip.ToString() + "%'"
                Else
                    filter += " and zip LIKE '%" + client.zip.ToString() + "%'"
                End If
            End If

            If Not (client.email.Equals("")) Then
                If filter = "" Then
                    filter += "email LIKE '%" + client.email + "%'"
                Else
                    filter += " and email LIKE '%" + client.email + "%'"
                End If
            End If

            If Not (client.org.Equals("")) Then
                If filter = "" Then
                    filter += "business LIKE '%" + client.org + "%'"
                Else
                    filter += " and business LIKE '%" + client.org + "%'"
                End If
            End If



            Dim newtable As New DataTable
            newtable.Load(dbObj.getReader())

            Dim newdataview As DataView = New DataView(newtable, filter, "lname", DataViewRowState.CurrentRows)

            Dim filteredList As New List(Of clientObj)
            Dim relationship1 As New List(Of String)
            'convert all rows in dataview to client list
            filteredList.Insert(0, New clientObj(relationship1, "NULL", 0, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", 0, "NULL"))

            Dim count As New Integer
            count = 0

            For Each drv As DataRowView In newdataview
                Dim row As DataRow
                row = drv.Row
                Dim clientA As New clientObj(New List(Of String), "", row("contactID"), row("fname"), row("lname"), row("phone"), row("business"), row("email"), row("addy"), "", row("city"), row("state"), row("zip"), "")
                filteredList.Add(clientA)

            Next


            'For Each row As DataRow In newdataview

            '    Dim clientA As New clientObj(New List(Of String), "", row("clientID"), row("fname"), row("lname"), row("phone"), row("business"), row("email"), row("addy"), row("addy2"), row("city"), row("state"), row("zip"), "")
            '    filteredList.Add(clientA)
            'Next


            Return filteredList
        End Using

    End Function

    Public Function checkForClient(ByVal client As clientObj) As List(Of clientObj)
        ''For testing purposes
        Dim testList As New List(Of clientObj)


        Dim filteredList As New List(Of clientObj)
        Dim relationship1 As New List(Of String)

        relationship1.Add("Computer Donor")
        ''relationship1.Add("Grants/Funding")

        ''relationship2.Add("Grands/Funding")

        ''For Each storedClient In clientList / Call to data base will go here
        ''Generate a filter query
        ''Go through json object and check if the fields are !empty
        ''results will come back as a data table
        ''for each result in the data table, generate a client object and add it to the list
        ''the list is what we display in the ui


        'If storedClient.firstName.Equals(client.firstName) Then
        'filteredList.Add(storedClient)
        'End If
        'Next
        ''Change zip to string 
        ''can also create fake client
        Dim clientA As New clientObj(relationship1, "Active", 1, "Gabriel", "Garcia Marquez", "000-000-0000", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", 1, "N/A")
        'Dim clientB As New clientObj(relationship1, "Active", 2, "Bob", "Bob", "123-456-7890", "NASA", "bob@gmail.com", "N/A", "N/A", "N/A", "N/A", 102112, "Hello")
        'Dim clientC As New clientObj(relationship1, "Active", 3, "Albus", "Dumbledore", "990-111-0019", "Hogwarts", "N/A", "Somewhere in Scotland", "N/A", "N/A", "N/A", 81708, "Headmaster of Hogwarts School for Wizardry")
        'Dim clientD As New clientObj(relationship1, "Active", 4, "Funder1", "Funder's last name", "651-647-3828", "Successful Organization INC", "seifert@aol.com", "301 13th Street", "100 N. 5th Avenue West", "St. Paul", "MN", 56223, "N/A")
        'Dim clientE As New clientObj(relationship1, "Dormant", 5, "Kathy", "Kroetter", "651-659-9001", "American Engineering Testing", "kkoetter@clarkfield.org", "2356 University Avenue", "2550 University Avenue West", "St. Paul", "MN", 56601, "N/A")
        'Dim clientF As New clientObj(relationship1, "Active", 6, "Laura", "Juergens", "651-675-2000", "WuXi AppTec", "kjuerg@wuxiapp.com", "2540 Executive Drive", "N/A", "St. Paul", "MN", 55120, "N/A")
        'Dim clientG As New clientObj(relationship1, "Active", 7, "Joe", "Warner", "612-226-6976", "Warner Stellian", "jlwarner@wstellian.com", "550 Atwater Cir", "N/A", "St. Paul", "MN", 55479, "N/A")
        'Dim clientH As New clientObj(relationship1, "Active", 8, "Mike", "Nation", "612-436-7773", "Victims of Torture", "mnation@cvt.org", "2356 University Avenue W Stre 430", "N/A", "St. Paul", "MN", 55110, "N/A")
        'Dim clientI As New clientObj(relationship1, "Active", 9, "Dianne", "Buck", "507-372-7768", "Victims of Torture", "dbuck@cvt.org", "2356 University Avenue W Stre 430", "N/A", "St. Paul", "MN", 55110, "N/A")
        'Dim clientJ As New clientObj(relationship1, "Active", 10, "Steve", "Brigger", "952-921-4861", "Victims of Torture", "sbrigger@cvt.org", "2356 University Avenue W Stre 430", "N/A", "St. Paul", "MN", 55110, "N/A")


        filteredList.Add(clientA)
        'filteredList.Add(clientB)
        'filteredList.Add(clientC)
        'filteredList.Add(clientD)
        'filteredList.Add(clientE)
        'filteredList.Add(clientF)
        'filteredList.Add(clientG)
        'filteredList.Add(clientH)
        'filteredList.Add(clientI)
        'filteredList.Add(clientJ)
        'filteredList.Add(client)

        ''For testing purposes
        ''For Each clientObj In filteredList
        ''If clientObj.firstName.Equals(client.firstName) Then
        ''testList.Add(clientObj)
        ''End If
        ''Next

        ''If(client.firstName
        ''Add a second address field to client object

        ''To send email: Send response to JQUERY of success or failure
        ''Create another controller to send email and accept the client id, naid choices, lot 
        ''Sending sig, lot number, naid choice, client id, keep original text



        Return filteredList

    End Function


    Public Function save() As Boolean

        Dim locationID As String = ""
        Dim guid As String = ""

        Dim results As New DataTable
        'dbObj.queryGetValue2("select locationID, uname from users where guid=@guid", {New SqlParameter("@guid", Me.guid)})


        Using dbObj.oDR
            'Dim key As Byte() = ASCIIEncoding.ASCII.GetBytes(Me.ekey)
            'Dim ekeyW As String = Convert.ToBase64String(key)
            'Convert.toBase64()
            dbObj.setReader("select locationID, guid from users where uname=@uname", {New SqlParameter("@uname", Me.username)}) ''find a way to pass in a global guid, ekey. originally: Me.guid
            If dbObj.oDR Is Nothing Then
                Return False
            End If
            results.Load(dbObj.oDR)
        End Using

        locationID = results.Rows.Item(0)("locationID")
        guid = results.Rows.Item(0)("guid")

        Return dbObj.insert2("insert into contact values(@fname, @mi, @lname, @phone, @email, @business, @addy,	@addy2,	@city, @state, @zip, @notes, @locationID, @uname, @cstatusID, @oname)", {New SqlParameter("@fname", Me.firstName), New SqlParameter("@mi", ""),
                                                                                                                                                                                               New SqlParameter("@lname", Me.lastName), New SqlParameter("@phone", Me.phone),
                                                                                                                                                                                               New SqlParameter("@email", Me.email), New SqlParameter("@business", Me.org),
                                                                                                                                                                                               New SqlParameter("@addy", Me.address), New SqlParameter("@addy2", Me.address2),
                                                                                                                                                                                               New SqlParameter("@city", Me.city), New SqlParameter("@state", Me.state),
                                                                                                                                                                                               New SqlParameter("@zip", Me.zip), New SqlParameter("@notes", Me.notes),
                                                                                                                                                                                               New SqlParameter("@locationID", locationID), New SqlParameter("@uname", Me.username),
                                                                                                                                                                                               New SqlParameter("@cstatusID", "1"), New SqlParameter("@oname", Me.username)})




    End Function

End Class