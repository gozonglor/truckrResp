Public Class buildingForm

    Sub New()

    End Sub
    Sub New(naidChoice As String, donationDescription As String, org As String)
        Me.naidChoice = naidChoice
        Me.donationDescription = donationDescription
        Me.org = org
    End Sub


    Private _donorSig As String
    Public Property donorSig() As String
        Get
            Return _donorSig
        End Get
        Set(value As String)
            _donorSig = value
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


End Class
