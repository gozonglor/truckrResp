Imports System.Net
Imports System.Web.Http
Imports PdfFileWriter
Imports System.Drawing
Imports System.Data.SqlClient


Public Class SubmitController
    Inherits ApiController

    Dim dbObj As New database


    ' GET api/submit
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
    End Function

    ' GET api/submit/5
    Public Function GetValue(ByVal id As Integer) As tooFormObj
        Dim base64img As String = "data:image/png;base64,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"
        Dim testFormObj As New tooFormObj(1, "yesNaid", 1234, "glor", "18 dell laptops, 3 desktops", base64img, "", "1/1/11")
        Return testFormObj
    End Function

    ' POST api/submit
    Public Function PostValue(<FromBody()> ByVal value As tooFormObj) As Boolean
        Try
            'adding tooFormObj to db
            Dim sender As New sendemail
            'sendemail.sendTOOForm(value, "Transfer of ownership")
            'sender.sendTOOForm(value, "Transfer of Ownership")
            'Dim image As New imgCreator("Donor signature", value.donorSig)

            ''1. create emtpy document
            'Dim doc As New PdfDocument(PaperType.Letter, False, UnitOfMeasure.Inch, "TransferForm")

            ''2. Create resources, define font resources
            'Dim fontName As String = "Arial"
            'Dim ArialNormal As New PdfFont(doc, fontName, FontStyle.Regular, True)

            ''3. Add new page
            'Dim pg As New PdfPage(doc)

            ''4. Add contents to page
            'Dim content As New PdfContents(pg)

            ''5. add graphics and text content to the contents object
            'content.SaveGraphicsState()
            'content.DrawText(ArialNormal, 40.0, 4.25, 9.25, TextJustify.Center, 0.02, Color.Black, Color.Black, "Transfer of Ownership Form")
            'content.SaveGraphicsState()
            'Dim box As New TextBox(4, 4)
            'box.AddText(ArialNormal, 9.0, "Hello testing testing testing testing")
            'content.DrawText(0.0, 12.0, 12.0, 5.0, 5.0, 7.0, TextBoxJustify.FitToWidth, box)
            'content.RestoreGraphicsState()

            'content.SaveGraphicsState()
            'Dim img As PdfImage
            'Dim create As New imgCreator
            'img = New PdfImage(doc, create.Base64ToImage(value.donorSig))
            'content.DrawImage(img, 20.0, 20.0, 300, 300)
            'content.RestoreGraphicsState()

            ''6. create pdf file
            'doc.CreateFile()

            ''Dim savePdf As New PdfPrintDocument(doc, 300.0)
            ''savePdf.CropRect = New RectangleF(0.95F, 0.95F, 6.6F, 9.1F)


            ''sender.sendhtml("glor@pcsforpeople.com", "Email works!", "<div id='submitted_too_form'>" + value.formatted + " </div>")
            ''sender.sendPic("glor@pcsforpeople.com", "Pic - Email works!", value.donorSig, "data:application/pdf;base64,JVBERi0xLjMKMyAwIG9iago8PC9UeXBlIC9QYWdlCi9QYXJlbnQgMSAwIFIKL1Jlc291cmNlcyAyIDAgUgovQ29udGVudHMgNCAwIFI+PgplbmRvYmoKNCAwIG9iago8PC9MZW5ndGggNDAwPj4Kc3RyZWFtCjE0LjQwIHcKMCBHCnEKcSBCVCAzNi4wMCAxNTQ4LjAwIFRkCjAgLTI2LjI1IFRkCi9GMSAxOC43NSBUZiAoVHJhbnNmZXIgb2YgT3duZXJzaGlwKSBUagpFVCBRCnEgQlQgMzYuMDAgMTUyMS43NSBUZAowIC0yNi4yNSBUZAovRjIgMTIuMDAgVGYgKEN1c3RvbWVyIFNpZ25hdHVyZTogKSBUagovRjEgMTIuMDAgVGYgKFNpZ24pIFRqCkVUIFEKcSBCVCAzNi4wMCAxNDk1LjUwIFRkCjAgLTI2LjI1IFRkCi9GMSAxNS4wMCBUZiAoZCkgVGoKRVQgUQpxIEJUIDM2LjAwIDE0NTguMDAgVGQKMCAtMjYuMjUgVGQKL0YyIDEyLjAwIFRmIChUaXRsZTopIFRqCkVUIFEKcSBCVCAzNi4wMCAxNDIwLjUwIFRkCjAgLTI2LjI1IFRkCi9GMiAxMi4wMCBUZiAoUENzIGZvciBQZW9wbGUgRW1wbG95ZWU6KSBUagpFVCBRClEKZW5kc3RyZWFtCmVuZG9iagoxIDAgb2JqCjw8L1R5cGUgL1BhZ2VzCi9LaWRzIFszIDAgUiBdCi9Db3VudCAxCi9NZWRpYUJveCBbMCAwIDEyMjQuMDAgMTU4NC4wMF0KPj4KZW5kb2JqCjUgMCBvYmoKPDwvQmFzZUZvbnQvSGVsdmV0aWNhL1R5cGUvRm9udAovRW5jb2RpbmcvV2luQW5zaUVuY29kaW5nCi9TdWJ0eXBlL1R5cGUxPj4KZW5kb2JqCjYgMCBvYmoKPDwvQmFzZUZvbnQvSGVsdmV0aWNhLUJvbGQvVHlwZS9Gb250Ci9FbmNvZGluZy9XaW5BbnNpRW5jb2RpbmcKL1N1YnR5cGUvVHlwZTE+PgplbmRvYmoKNyAwIG9iago8PC9CYXNlRm9udC9IZWx2ZXRpY2EtT2JsaXF1ZS9UeXBlL0ZvbnQKL0VuY29kaW5nL1dpbkFuc2lFbmNvZGluZwovU3VidHlwZS9UeXBlMT4+CmVuZG9iago4IDAgb2JqCjw8L0Jhc2VGb250L0hlbHZldGljYS1Cb2xkT2JsaXF1ZS9UeXBlL0ZvbnQKL0VuY29kaW5nL1dpbkFuc2lFbmNvZGluZwovU3VidHlwZS9UeXBlMT4+CmVuZG9iago5IDAgb2JqCjw8L0Jhc2VGb250L0NvdXJpZXIvVHlwZS9Gb250Ci9FbmNvZGluZy9XaW5BbnNpRW5jb2RpbmcKL1N1YnR5cGUvVHlwZTE+PgplbmRvYmoKMTAgMCBvYmoKPDwvQmFzZUZvbnQvQ291cmllci1Cb2xkL1R5cGUvRm9udAovRW5jb2RpbmcvV2luQW5zaUVuY29kaW5nCi9TdWJ0eXBlL1R5cGUxPj4KZW5kb2JqCjExIDAgb2JqCjw8L0Jhc2VGb250L0NvdXJpZXItT2JsaXF1ZS9UeXBlL0ZvbnQKL0VuY29kaW5nL1dpbkFuc2lFbmNvZGluZwovU3VidHlwZS9UeXBlMT4+CmVuZG9iagoxMiAwIG9iago8PC9CYXNlRm9udC9Db3VyaWVyLUJvbGRPYmxpcXVlL1R5cGUvRm9udAovRW5jb2RpbmcvV2luQW5zaUVuY29kaW5nCi9TdWJ0eXBlL1R5cGUxPj4KZW5kb2JqCjEzIDAgb2JqCjw8L0Jhc2VGb250L1RpbWVzLVJvbWFuL1R5cGUvRm9udAovRW5jb2RpbmcvV2luQW5zaUVuY29kaW5nCi9TdWJ0eXBlL1R5cGUxPj4KZW5kb2JqCjE0IDAgb2JqCjw8L0Jhc2VGb250L1RpbWVzLUJvbGQvVHlwZS9Gb250Ci9FbmNvZGluZy9XaW5BbnNpRW5jb2RpbmcKL1N1YnR5cGUvVHlwZTE+PgplbmRvYmoKMTUgMCBvYmoKPDwvQmFzZUZvbnQvVGltZXMtSXRhbGljL1R5cGUvRm9udAovRW5jb2RpbmcvV2luQW5zaUVuY29kaW5nCi9TdWJ0eXBlL1R5cGUxPj4KZW5kb2JqCjE2IDAgb2JqCjw8L0Jhc2VGb250L1RpbWVzLUJvbGRJdGFsaWMvVHlwZS9Gb250Ci9FbmNvZGluZy9XaW5BbnNpRW5jb2RpbmcKL1N1YnR5cGUvVHlwZTE+PgplbmRvYmoKMiAwIG9iago8PAovUHJvY1NldCBbL1BERiAvVGV4dCAvSW1hZ2VCIC9JbWFnZUMgL0ltYWdlSV0KL0ZvbnQgPDwKL0YxIDUgMCBSCi9GMiA2IDAgUgovRjMgNyAwIFIKL0Y0IDggMCBSCi9GNSA5IDAgUgovRjYgMTAgMCBSCi9GNyAxMSAwIFIKL0Y4IDEyIDAgUgovRjkgMTMgMCBSCi9GMTAgMTQgMCBSCi9GMTEgMTUgMCBSCi9GMTIgMTYgMCBSCj4+Ci9YT2JqZWN0IDw8Cj4+Cj4+CmVuZG9iagoxNyAwIG9iago8PAovUHJvZHVjZXIgKGpzUERGIDAuOS4wcmMyKQovQ3JlYXRpb25EYXRlIChEOjIwMTUwMjAzMTU0NzUxKQo+PgplbmRvYmoKMTggMCBvYmoKPDwKL1R5cGUgL0NhdGFsb2cKL1BhZ2VzIDEgMCBSCi9PcGVuQWN0aW9uIFszIDAgUiAvRml0SCBudWxsXQovUGFnZUxheW91dCAvT25lQ29sdW1uCj4+CmVuZG9iagp4cmVmCjAgMTkKMDAwMDAwMDAwMCA2NTUzNSBmIAowMDAwMDAwNTM2IDAwMDAwIG4gCjAwMDAwMDE3NjcgMDAwMDAgbiAKMDAwMDAwMDAwOSAwMDAwMCBuIAowMDAwMDAwMDg3IDAwMDAwIG4gCjAwMDAwMDA2MjUgMDAwMDAgbiAKMDAwMDAwMDcxNSAwMDAwMCBuIAowMDAwMDAwODEwIDAwMDAwIG4gCjAwMDAwMDA5MDggMDAwMDAgbiAKMDAwMDAwMTAxMCAwMDAwMCBuIAowMDAwMDAxMDk4IDAwMDAwIG4gCjAwMDAwMDExOTIgMDAwMDAgbiAKMDAwMDAwMTI4OSAwMDAwMCBuIAowMDAwMDAxMzkwIDAwMDAwIG4gCjAwMDAwMDE0ODMgMDAwMDAgbiAKMDAwMDAwMTU3NSAwMDAwMCBuIAowMDAwMDAxNjY5IDAwMDAwIG4gCjAwMDAwMDE5OTEgMDAwMDAgbiAKMDAwMDAwMjA3MyAwMDAwMCBuIAp0cmFpbGVyCjw8Ci9TaXplIDE5Ci9Sb290IDE4IDAgUgovSW5mbyAxNyAwIFIKPj4Kc3RhcnR4cmVmCjIxNzcKJSVFT0Y=")

            sender.sendhtml("glor@macalester.edu", "PCs for People - Transfer of Ownership form", "<div id='submitted_too_form'>" + value.formatted + " </div>", value.pdf64)
            Dim encoding As New System.Text.ASCIIEncoding()
            Dim holder As New Byte()
            Dim h As String = value.pdf64
            ''Dim a As Char() = System.Convert.FromBase64String(value.pdf64)


            Dim binary = Convert.FromBase64String(value.pdf64)
            Dim choice = ""

            If (value.naidChoice = "yesNaid") Then
                choice = 0
            Else
                choice = 1
            End If

            value.naidChoice = choice
            Dim numberID As Integer = Convert.ToInt32(value.donorID.ToString)
            value.donorID = numberID

            Return dbObj.insert2("insert into transferOfOwnership values(@lotNumber, @contactID, @uname, @donationDesc, @transferPDF, @pickupDate, @naid)", {New SqlParameter("@lotNumber", value.lotNum), New SqlParameter("@contactID", value.donorID),
                                                                                                                                                                                               New SqlParameter("@uname", value.username), New SqlParameter("@donationDesc", value.donationDescription),
                                                                                                                                                                                               New SqlParameter("@transferPDF", binary), New SqlParameter("@pickupDate", value.dateFilled),
                                                                                                                                                                                               New SqlParameter("@naid", value.naidChoice)})


            ''Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ' PUT api/submit/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/submit/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
