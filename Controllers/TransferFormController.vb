Imports System.Net
Imports System.Web.Http


Public Class TransferFormController
    Inherits ApiController

    ' GET api/transferform
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/transferform/5
    Public Function GetValue(ByVal id As Integer) As tooFormObj

        Dim tooForm As New tooFormObj(12345, "yesNaid", 1, "Gozong", "2 dell laptops, 4 dell desktops", "1opu21", "Somewhere", "1/1/11")
        Return tooForm
    End Function

    ' POST api/transferform
    Public Function PostValue(<FromBody()> ByVal donorResponse As buildingForm) As String
        ''====[A]Dim errorList As New List(Of String)
        ''====[A]Original code that worked with the reading-in-text-from-desktop method
        'errorList.Add("Received an empty or incomplete form")
        'Try
        '    Return donorResponse.getNaid(donorResponse.naidChoice)
        'Catch ex As Exception
        '    Return errorList
        'End Try
        ''====End of original code. Function originally returned List(Of String)
        ''====List was then parsed out on the javascript side
        ''====[A] Check javascript side, which has also been commented out...
        Dim finalResponse As String = ""
        Dim form As String
        Try
            Dim repOrg As String = "True"
            If ((donorResponse.org.Equals("") Or (donorResponse.org.ToLower().Equals("unknown")))) Then ''if there is no organization tied to this particular donor...
                repOrg = "False" ''return false, grab the first and last name in js
            End If
            Dim firstParagraph As String
            firstParagraph = ""
            firstParagraph = repOrg + "%" + " is transferring ownership of electronic equipment to PCs for People. PCs for People will redistribute the refurbished computers to low income families and people with disabilities. This document fully transfers legal ownership from to PCs for People, and along with that ownership all liability associated with disposal and use. PCs for People has a zero landfill policy and any non-functional parts will be locally recycled. No goods or services were exchanged in consideration of this contribution and PCs for People understands that no warranty is provided and all equipment is accepted as is."
            ''^Above will have to be parsed/split by :, and repOrg will be checked on the js side

            Dim formattedDescription As String
            formattedDescription = ""
            formattedDescription = "<div id='donation' style='font-size:10pt; padding-left: 25px; padding-right:25px; padding-bottom: 10px; padding-top: 10px; background-color: #EEEEEE; padding-right: 25px; word-wrap: break-word;'><b><u>Donation Description:</b></u> " + donorResponse.donationDescription + "</div>"

            Dim naidChoice = donorResponse.getNaid(donorResponse.naidChoice)
            Dim formattedNaid As String
            formattedNaid = ""
            formattedNaid = "<center><h4>Data Sanitization</h4></center><div id='naid_decision' style='background-color:#EEEEEE;padding-left: 25px; padding-top: 10px; padding-right:25px; padding-bottom: 10px;'>" + naidChoice + "</div>"

            Dim finalParagraph As String
            finalParagraph = ""
            finalParagraph = "By signing your name below you agree that you are a duly appointed and authorized representative of your company:"

            Dim finalForm As String
            finalForm = ""
            finalForm = "" + firstParagraph + formattedDescription + formattedNaid + finalParagraph
            finalResponse = finalForm
        Catch ex As Exception
            form = "Received an empty or incomplete form"
            finalResponse = form
        End Try
        Return finalResponse
    End Function

    ' PUT api/transferform/5
    Public Function PutValue(ByVal val As tooFormObj) As tooFormObj
        val.username = "PCs for People"
        Return val
    End Function

    ' DELETE api/transferform/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
