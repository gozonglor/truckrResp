Imports System.Net.Mail
Imports System.IO
Imports PdfFileWriter

Public Class sendemail
    Public sent As String

    Public Function sendPic(ByVal toaddress As String, ByVal subject As String, ByVal body As String, Optional ByVal file As String = "", Optional ByVal bcc As String = "") As String
        Dim sent = ""
        Dim retryCount As Integer = 1
        Dim imageCreater As New imgCreator
        While (sent <> "ok" OrElse retryCount <= 10)
            Try
                Dim SmtpServer As New SmtpClient()
                Dim mailme As New MailMessage()

                SmtpServer.Credentials = New Net.NetworkCredential("support@pcsforpeople.com", "pcsforpeople")
                SmtpServer.Port = 587
                SmtpServer.Host = "smtp.gmail.com"
                SmtpServer.EnableSsl = True
                mailme = New MailMessage()
                mailme.From = New MailAddress("support@pcsforpeople.com", "PCs for People - Internet")
                mailme.To.Add(toaddress)
                If bcc <> "" Then
                    mailme.Bcc.Add(bcc)
                End If
                mailme.Subject = subject
                mailme.IsBodyHtml = True

                Dim contentID As String = "testing!"
                If file <> "" Then
                    'Dim attachment As New Net.Mail.Attachment()
                    'mailme.Attachments.Add(attachment)
                    'attachment.ContentId = "testing!"

                    ''=======try sending as an attachment
                    'Try
                    '    Dim imgStream As New MemoryStream()
                    '    Dim image As System.Drawing.Bitmap = imageCreater.getImagefromBase64B(file)
                    '    Dim filename As String = "sample image"

                    '    image.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png)
                    '    imgStream.Position = 0
                    '    mailme.Attachments.Add(New Attachment(imgStream, filename, System.Net.Mime.MediaTypeNames.Image.Jpeg))


                    'Catch ex As Exception

                    'End Try

                    ''======testing sending a regular image (ignore base 64)
                    Dim oAttachment As Attachment = mailme.Attachments("C:\Users\Intern\Documents\Visual Studio 2012\Projects\truckrAPI\truckrAPI\Images")
                    Dim CID As String = "test001@host"
                    oAttachment.ContentId = CID
                    mailme.Body = "Embedding the image: <img src=""cid:" + CID + """> embedded picture."



                End If

                'mailme.Body = "<html><body>this is a <img src=""cid:" _
                '     + contentID + """> embedded picture.</body></html>"

                SmtpServer.Send(mailme)
                sent = "ok"
                Return sent

            Catch ex As Exception
                If retryCount = 10 Then
                    senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: sendemail.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The User: unknown" + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
                    sent = ex.Message
                    Return sent
                Else
                    retryCount = retryCount + 1
                End If
            End Try
        End While

        Return sent
    End Function



    Public Function send(ByVal toaddress As String, ByVal subject As String, ByVal body As String, Optional ByVal file As String = "", Optional ByVal bcc As String = "") As String
        Try
            Dim SmtpServer As New SmtpClient()
            Dim mailme As New MailMessage()
            SmtpServer.Credentials = New Net.NetworkCredential("support@pcsforpeople.com", "pcsforpeople")
            SmtpServer.Port = 587
            SmtpServer.Host = "smtp.gmail.com"
            SmtpServer.EnableSsl = True
            mailme = New MailMessage()
            mailme.From = New MailAddress("support@pcsforpeople.com", "PCs for People - Internet")
            mailme.To.Add(toaddress)
            If bcc <> "" Then
                mailme.Bcc.Add(bcc)
            End If
            mailme.Subject = subject
            mailme.IsBodyHtml = True
            mailme.Body = body

            If file <> "" Then
                Dim attachment As New Net.Mail.Attachment(file)
                mailme.Attachments.Add(attachment)
            End If
            SmtpServer.Send(mailme)
            sent = "ok"
            Return sent
        Catch ex As Exception
            senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: sendemail.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The User: unknown" + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            sent = ex.Message
            Return sent
        End Try
    End Function

    Public Function sendhtml(ByVal toaddress As String, ByVal subject As String, ByVal body As String, Optional ByVal file As String = "", Optional ByVal bcc As String = "") As String
        sent = ""
        Dim retryCount As Integer = 1

        While (sent <> "ok" OrElse retryCount <= 10)
            Try
                Dim SmtpServer As New SmtpClient()
                Dim mailme As New MailMessage()
                SmtpServer.Credentials = New Net.NetworkCredential("support@pcsforpeople.com", "pcsforpeople")
                SmtpServer.Port = 587
                SmtpServer.Host = "smtp.gmail.com"
                SmtpServer.EnableSsl = True
                mailme = New MailMessage()
                mailme.From = New MailAddress("support@pcsforpeople.com", "PCs for People - Internet")
                mailme.To.Add(toaddress)
                If bcc <> "" Then
                    mailme.Bcc.Add(bcc)
                End If
                mailme.Subject = subject
                mailme.IsBodyHtml = True
                mailme.Body = body

                Dim byteArray() As Byte
                ''byteArray = Convert.FromBase64String(file)
                byteArray = Convert.FromBase64String(file.Substring(file.IndexOf(",") + 1))

                If file Is Nothing Then
                    mailme.Body = mailme.Body + "<br><br><b>PDF attachment not found.</b>"
                End If

                If file <> "" Then
                    Dim attachment As New Net.Mail.Attachment(New MemoryStream(byteArray), "transferform", "image/png") ''was pdf
                    mailme.Attachments.Add(attachment)
                End If
                SmtpServer.Send(mailme)
                sent = "ok"
                Return sent

            Catch ex As Exception
                If retryCount = 10 Then
                    senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: sendemail.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The User: unknown" + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
                    sent = ex.Message
                    Return sent
                Else
                    retryCount = retryCount + 1
                End If
            End Try
        End While

        Return sent
    End Function



    Private Function FileExists(ByVal FileFullPath As String) _
     As Boolean
        If Trim(FileFullPath) = "" Then Return False

        Dim f As New IO.FileInfo(FileFullPath)
        Return f.Exists

    End Function

    Public Function sendhtml(toaddress As String, subject As String, body As String, mainHeading As String, generateTemplate As Boolean, Optional file As String = "", Optional bcc As String = "") As String
        sent = ""
        Dim retryCount As Integer = 1

        If generateTemplate Then
            If mainHeading = "" Then mainHeading = "PCs for People"
            body = generateBodyTemplate(body, mainHeading)
        End If


        While (sent <> "ok" OrElse retryCount <= 10)
            Try
                Dim SmtpServer As New SmtpClient()
                Dim mailme As New MailMessage()
                SmtpServer.Credentials = New Net.NetworkCredential("support@pcsforpeople.com", "pcsforpeople")
                SmtpServer.Port = 587
                SmtpServer.Host = "smtp.gmail.com"
                SmtpServer.EnableSsl = True
                mailme = New MailMessage()
                mailme.From = New MailAddress("support@pcsforpeople.com", "PCs for People - Internet")
                mailme.To.Add(toaddress)
                If bcc <> "" Then
                    mailme.Bcc.Add(bcc)
                End If
                mailme.Subject = subject
                mailme.IsBodyHtml = True
                mailme.Body = body

                If file <> "" Then
                    Dim attachment As New Net.Mail.Attachment(file)
                    mailme.Attachments.Add(attachment)
                End If
                SmtpServer.Send(mailme)
                sent = "ok"
                Return sent

            Catch ex As Exception
                If retryCount = 10 Then
                    senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: sendemail.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The User: unknown" + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
                    sent = ex.Message
                    Return sent
                Else
                    retryCount = retryCount + 1
                End If
            End Try
        End While

        Return sent
    End Function

    Public Sub senderror(ByVal toaddress As String, ByVal subject As String, ByVal body As String, Optional ByVal file As String = "", Optional ByVal bcc As String = "")
        Try

            Dim SmtpServer As New SmtpClient()
            Dim mailme As New MailMessage()
            SmtpServer.Credentials = New Net.NetworkCredential("support@pcsforpeople.com", "pcsforpeople")
            SmtpServer.Port = 587
            SmtpServer.Host = "smtp.gmail.com"
            SmtpServer.EnableSsl = True
            mailme = New MailMessage()
            mailme.From = New MailAddress("support@pcsforpeople.com", "PCs for People - Internet")
            mailme.To.Add(toaddress)
            If bcc <> "" Then
                mailme.Bcc.Add(bcc)
            End If
            mailme.Subject = subject
            mailme.IsBodyHtml = True
            mailme.Body = body

            If file <> "" Then
                Dim attachment As New Net.Mail.Attachment(file)
                mailme.Attachments.Add(attachment)
            End If
            SmtpServer.Send(mailme)
            sent = "ok"
        Catch ex As Exception
            sent = "An error: " + ex.Message
        End Try
    End Sub

    Public Function usersend(ByVal user As String, ByVal pass As String, ByVal mailName As String, ByVal fromaddy As String, ByVal toaddress As String, ByVal subject As String, ByVal body As String, Optional ByVal file As String = "", Optional ByVal bcc As String = "", Optional ByVal cc As String = "") As String
        Try
            Dim SmtpServer As New SmtpClient()
            Dim mailme As New MailMessage()
            SmtpServer.Credentials = New Net.NetworkCredential(user, pass)
            SmtpServer.Port = 587
            SmtpServer.Host = "smtp.gmail.com"
            SmtpServer.EnableSsl = True
            mailme = New MailMessage()
            mailme.From = New MailAddress(fromaddy, mailName)
            If toaddress <> "" Then
                mailme.To.Add(toaddress)
            End If
            If bcc <> "" Then
                mailme.Bcc.Add(bcc)
            End If
            If cc <> "" Then
                mailme.CC.Add(cc)
            End If
            mailme.Subject = subject
            mailme.IsBodyHtml = True
            mailme.Body = body

            If file <> "" Then
                Dim attachment As New Net.Mail.Attachment(file)
                mailme.Attachments.Add(attachment)
            End If
            SmtpServer.Send(mailme)
            sent = "ok"
            Return sent
        Catch ex As Exception
            senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: sendemail.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The User: unknown" + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            sent = ex.Message
            Return sent
        End Try
    End Function

    Function generateBodyTemplate(ByVal body As String, mainHeading As String) As String
        Return "<div style=""border: 1px solid black; padding: 5px; width: 550px;text-align:left;background-color:#FFF;""><span style=""text-align:center;""><h2>" + mainHeading + "</h2><hr /></span>" + body + "<div style=""text-align:center;margin-top: 10px;""></div>" + "<div style=""text-align:center;margin-top: 10px;font-size:8pt""><img src=""http://www.pcsforpeople.com/images/logo.jpg"" /><br>1481 Marshall Ave St. Paul, MN 55104 <br /> 651-354-2552 <br /> www.pcsforpeople.org</div></div>"
    End Function

    Function generateBodyTemplateRenwal(ByVal body As String, mainHeading As String) As String
        Return "<div style=""border: 1px solid black; padding: 5px; width: 550px;text-align:left;background-color:#FFF;""><span style=""text-align:center;""><h2>" + mainHeading + "</h2><hr /></span>" + body + "<div style=""text-align:center;margin-top: 10px;""><a style='border-width: 10px 20px; border-style: solid; border-color: rgb(52, 142, 218); margin: 0px 10px 0px 0px; padding: 0px; border-radius: 25px; text-align: center; color: rgb(255, 255, 255); line-height: 2; font-family: ""Helvetica Neue"", ""Helvetica"", Helvetica, Arial, sans-serif; font-size: 100%; font-weight: bold; text-decoration: none; display: inline-block; cursor: pointer; background-color: rgb(52, 142, 218);' href=""https://www.internetrenewal.com"">Renew Online</a></div>" + "<div style=""text-align:center;margin-top: 10px;font-size:8pt""><img src=""http://www.pcsforpeople.com/images/logo.jpg"" /><br>1481 Marshall Ave St. Paul, MN 55104 <br /> 651-354-2552 <br /> www.pcsforpeople.org</div></div>"
    End Function

    Function sendTOOForm(ByVal formInformation As tooFormObj, mainHeading As String) As String



        'Dim finalResponse As String = ""
        'Dim firstParagraph As String
        'firstParagraph = ""

        'If (formInformation.org.Equals("")) Then
        '    firstParagraph = formInformation.donorID + " is transferring ownership of electronic equipment to PCs for People. PCs for People will redistribute the refurbished computers to low income families and people with disabilities. This document fully transfers legal ownership from to PCs for People, and along with that ownership all liability associated with disposal and use. PCs for People has a zero landfill policy and any non-functional parts will be locally recycled. No goods or services were exchanged in consideration of this contribution and PCs for People understands that no warranty is provided and all equipment is accepted as is."
        'Else
        '    firstParagraph = formInformation.org + " is transferring ownership of electronic equipment to PCs for People. PCs for People will redistribute the refurbished computers to low income families and people with disabilities. This document fully transfers legal ownership from to PCs for People, and along with that ownership all liability associated with disposal and use. PCs for People has a zero landfill policy and any non-functional parts will be locally recycled. No goods or services were exchanged in consideration of this contribution and PCs for People understands that no warranty is provided and all equipment is accepted as is."
        'End If


        'Dim formattedDescription As String
        'formattedDescription = ""
        'formattedDescription = "<div id='donation' style='font-size:10pt; padding-left: 25px; padding-right:25px; padding-bottom: 5px; background-color: #EEEEEE;'><b><u>Donation Description:</b></u> " + formInformation.donationDescription + "</div>"

        'Dim naidChoice = formInformation.naidChoice
        'Dim formattedNaid As String
        'formattedNaid = ""
        'formattedNaid = "<center><h4>Data Sanitization</h4></center><div id='naid_decision' style='background-color:#EEEEEE;padding-left: 25px; padding-right:25px; padding-bottom: 5px;'>" + naidChoice + "</div>"

        'Dim finalParagraph As String
        'finalParagraph = ""
        'finalParagraph = "By signing your name below you agree that you are a duly appointed and authorized representative of your company:"

        'Dim finalForm As String
        'finalForm = ""
        'finalForm = "" + firstParagraph + formattedDescription + formattedNaid + finalParagraph
        'finalResponse = finalForm




        Return "<div style=""border: 1px solid black; padding: 5px; width: 550px;text-align: left; background-color:#EEEEEE;""><center><h1>" + mainHeading + "</h1></center>" + "<b>Lot Number: </b>" + formInformation.lotNum + "<br><b>Donor ID: </b>" + formInformation.donorID + "<b>Donation Descripttion: </b>" + formInformation.donationDescription + "</div>"

    End Function
End Class
