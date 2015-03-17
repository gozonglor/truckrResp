Imports System.Drawing
Imports System.IO
Imports System.Drawing.Imaging

Public Class imgCreator

    Private _img As Byte()
    Public Property imageByte() As Byte()
        Get
            Return _img
        End Get
        Set(ByVal value As Byte())
            _img = value
        End Set
    End Property
    Private _name As String
    Public Property name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property


    Sub New()

    End Sub

    Sub New(name As String, imageAsByte As Byte())
        Me.imageByte = imageAsByte
        Me.name = name
    End Sub

    Sub New(name As String, imageBase64DataURL As String)
        Me.imageByte = Convert.FromBase64String(imageBase64DataURL.Substring(imageBase64DataURL.IndexOf(",") + 1))
        Me.name = name
    End Sub

    Sub New(name As String, imageIn As System.Drawing.Image)
        Try
            convertImagetoByte(imageIn)
            Me.name = name
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function getImagefromBase64B(ByVal uploadedImage As String) As Image
        'get a temp image from bytes, instead of loading from disk
        'data:image/gif;base64,
        'this image is a single pixel (black)
        Dim bytes As Byte() = Convert.FromBase64String(uploadedImage)

        Dim image__1 As Image
        Using ms As New MemoryStream(bytes)
            ms.Write(bytes, 0, bytes.Length)
            image__1 = Image.FromStream(ms)
        End Using

        Return image__1
    End Function

    Sub New(name As String, theUploadedImage As HttpPostedFile)
        Try

            If name = "" Then Throw New Exception("Please choose a file.")

            Me.name = name

            Const upload_max_size = 1024 * 3 ' max size of the upload (KB) note: this doesn't override any server upload limits
            Dim originalimg As System.Drawing.Image ' used to hold the original image
            Dim msg As String = "" ' display results
            Dim upload_ok As Boolean  ' did the upload work ?
            Randomize() ' used to help the cache-busting on the preview images
            upload_ok = False

            'file name is upload_dir & upload_original & intloop.ToString & fileExt
            Dim intloop As Integer = 0

            If theUploadedImage.ContentLength <= 0 Then
                Throw New Exception("The uploaded file appears to be empty.")
            End If

            If Not IsValidImage(theUploadedImage) Then
                Throw New Exception("This is not a valid image. Ex. PNG, JPG, and GIF")
            End If

            If theUploadedImage.ContentLength > upload_max_size * 1024 Then
                Throw New Exception("Sorry, the image must be less than " & upload_max_size.ToString & "Kb")
            End If

            originalimg = System.Drawing.Image.FromStream(theUploadedImage.InputStream)
            convertImagetoByte(originalimg)

        Catch ex As Exception
            Throw New Exception("Sorry, that was not an image we could process. Error: " + ex.Message)
        End Try
    End Sub

    Sub resize(width As Integer, height As Integer)

        Dim originalimg As Image = Me.convertByteToImage
        Dim newWidth, newHeight As Integer ' new width/height for the thumbnail
        Dim l2 As Integer


        ' work out the width/height for the thumbnail. Preserve aspect ratio and honour max width/height
        ' Note: if the original is smaller than the thumbnail size it will be scaled up
        If (originalimg.Width / width) > (originalimg.Width / height) Then
            l2 = originalimg.Width
            newWidth = width
            newHeight = originalimg.Height * (width / l2)
            If newHeight > height Then
                newWidth = newWidth * (height / newHeight)
                newHeight = height
            End If
        Else
            l2 = originalimg.Height
            newHeight = height
            newWidth = originalimg.Width * (height / l2)
            If newWidth > width Then
                newHeight = newHeight * (width / newWidth)
                newWidth = width
            End If
        End If

        Dim thumb As New Bitmap(newWidth, newHeight)

        'Create a graphics object           
        Dim gr_dest As Graphics = Graphics.FromImage(thumb)

        ' just in case it's a transparent GIF force the bg to white 
        Dim sb = New SolidBrush(System.Drawing.Color.White)
        gr_dest.FillRectangle(sb, 0, 0, thumb.Width, thumb.Height)

        'Re-draw the image to the specified height and width
        gr_dest.DrawImage(originalimg, 0, 0, thumb.Width, thumb.Height)

        'convert the image to byte for the database
        Using ms As New MemoryStream()
            thumb.Save(ms, ImageFormat.Jpeg)
            Me.imageByte = ms.ToArray()
        End Using

        ' Housekeeping for the generated thumbnail
        If Not thumb Is Nothing Then
            thumb.Dispose()
            thumb = Nothing
        End If
        ' House Keeping !
        If Not originalimg Is Nothing Then
            originalimg.Dispose()
            originalimg = Nothing
        End If
    End Sub

    Function IsValidImage(httpimage As HttpPostedFile) As Boolean
        Try
            Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(httpimage.InputStream)
            Return True
        Catch generatedExceptionName As OutOfMemoryException
            ' Image.FromFile throws an OutOfMemoryException  
            ' if the file does not have a valid image format or 
            ' GDI+ does not support the pixel format of the file. 
            Return False
        End Try
    End Function

    Public Function getDataURL() As String
        Return "data:image/jpg;base64," + Convert.ToBase64String(Me.imageByte)
    End Function

    Public Function convertByteToImage() As Image
        Try
            Using ms As New MemoryStream(Me.imageByte)
                Dim returnImage As Image = Image.FromStream(ms)
                Return returnImage
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function saveImageToFile(path As String) As Boolean
        Try
            Using ms As New MemoryStream(Me.imageByte)
                Dim returnImage As Image = Image.FromStream(ms)
                returnImage.Save(path, ImageFormat.Jpeg)
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function



    Private Sub convertImagetoByte(imageIn As Image)
        Using ms As New MemoryStream()
            imageIn.Save(ms, ImageFormat.Jpeg)
            Me.imageByte = ms.ToArray()
        End Using
    End Sub

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
