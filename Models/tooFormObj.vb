Imports System.IO

Public Class tooFormObj

    Sub New()

    End Sub
    Sub New(lotNum As Integer, naidChoice As String, donorID As Integer, username As String, donationDescription As String, donorSig As String, org As String, dateFilled As String, Optional pdf64 As String = "", Optional formatted As String = "")
        Me.lotNum = lotNum
        Me.naidChoice = naidChoice
        Me.donorID = donorID
        Me.username = username
        Me.donationDescription = donationDescription
        Me.donorSig = donorSig
        Me.org = org ''will not need this
        Me.formatted = formatted ''will not need
        Me.pdf64 = pdf64 ''transfer pdf64
        Me.dateFilled = dateFilled
    End Sub

    'Private _donorSig As List(Of Dictionary(Of String, Integer))
    'Public Property donorSig() As List(Of Dictionary(Of String, Integer))
    '    Get
    '        Return _donorSig
    '    End Get
    '    Set(value As List(Of Dictionary(Of String, Integer)))
    '        _donorSig = value
    '    End Set
    'End Property

    Private _dateFilled As String
    Public Property dateFilled() As String
        Get
            Return _dateFilled
        End Get
        Set(value As String)
            _dateFilled = value
        End Set
    End Property


    Private _pdf64 As String
    Public Property pdf64() As String
        Get
            Return _pdf64
        End Get
        Set(value As String)
            _pdf64 = value
        End Set
    End Property

    Private _donorSig As String
    Public Property donorSig() As String
        Get
            Return _donorSig
        End Get
        Set(value As String)
            _donorSig = value
        End Set
    End Property

    Private _lotNum As Integer
    Public Property lotNum() As Integer
        Get
            Return _lotNum
        End Get
        Set(ByVal value As Integer)
            _lotNum = value
        End Set
    End Property

    Private _naidChoice As String
    Public Property naidChoice() As String
        Get
            Return _naidChoice
        End Get
        Set(ByVal value As String)
            _naidChoice = value
        End Set
    End Property

    Private _donorID As Integer
    Public Property donorID() As Integer
        Get
            Return _donorID
        End Get
        Set(ByVal value As Integer)
            _donorID = value
        End Set
    End Property

    Private _username As String
    Public Property username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Private _donationDescription As String
    Public Property donationDescription() As String
        Get
            Return _donationDescription
        End Get
        Set(ByVal value As String)
            _donationDescription = value
        End Set
    End Property

    Property org As String = "Empty"

    Property formatted As String = "Empty"

    Public Function getNaid(ByVal response As String) As String
        ''response will a string saying 'noNaid' or 'yesNaid'

        ''[A] ====== original string to hold js
        ''Dim donorChoice As New List(Of String)

        'Try
        'Using sr As New StreamReader("C:\Users\Intern\Desktop\verbage.txt")
        'Dim line As String
        'line = sr.ReadToEnd()
        'Dim paragraphs As String() = line.Split(New Char() {"~"})
        'Dim para As String
        'For Each para In paragraphs
        'donorChoice.Add(para)
        'Next

        'End Using
        'Catch ex As Exception
        'Dim error_string As String
        'error_string = "The file could not be read."
        'donorChoice.Add(error_string)
        'End Try

        ''=====[A]Below is recent code that works, pulling language from the desktop
        ''=====originally returned a list of strings that was parsed out on the javascript side
        'Try
        '    Using sr As New StreamReader("c:\users\intern\desktop\verbage.txt")
        '        Dim line As String
        '        line = sr.ReadToEnd()
        '        Dim paragraphs As String() = line.Split(New Char() {"~"})
        '        Dim para As String
        '        For Each para In paragraphs
        '            donorChoice.Add(para)
        '        Next

        '        If response = "yesnaid" Then
        '            donorChoice.Item(3) = ""
        '        Else
        '            If response = "nonaid" Then
        '                donorChoice.Item(2) = ""
        '            End If
        '        End If

        '    End Using
        'Catch ex As Exception
        '    Dim error_string As String
        '    error_string = "the file could not be read."
        '    donorChoice.Add(error_string)
        'End Try
        ''=====[A]End of code

        Dim donorChoice As String = ""
        Try
            If response = "yesNaid" Then
                donorChoice = "<b>Sanitize My Hard Drives Using NAID Certified Processes</b> - I require PCs for People, as a condition of NAID AAA Certification, to sanitize or physically destroy all computer hard drives being recycled. Additional steps will be taken to record the serial numbers of all computer hard drives that are physically destroyed and or/sanitized. Within ten business days, I will receive a certificate of data destruction and a data sanitization log detailing the per item status of each device recycled."
            Else
                If response = "noNaid" Then
                    donorChoice = "<b>Opt-Out of Hard Drive Sanitization Using NAID Certified Processes</b> - I choose to utilize PCs for People non-certified data sanitization. Each drive will receive a one-pass wipe and have Windows reinstalled, but serial numbers are not logged."
                End If
            End If
        Catch ex As Exception
            donorChoice = "Missing naid choice to evaluate."
        End Try

        Return donorChoice
    End Function

    Function Base64ToImage(ByVal base64string As String) As System.Drawing.Image
        'Setup image and get data stream together
        Dim img As System.Drawing.Image
        Dim MS As System.IO.MemoryStream = New System.IO.MemoryStream
        Dim b64 As String = base64string.Replace(" ", "+")
        Dim b() As Byte

        'Converts the base64 encoded msg to image data
        b = Convert.FromBase64String(b64)
        MS = New System.IO.MemoryStream(b)

        'creates image
        img = System.Drawing.Image.FromStream(MS)

        Return img
    End Function


End Class
