Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim username = "Bob"
            Dim password = "123"

            If (username = TextBox1.Text & password = TextBox2.Text) Then

            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Protected Sub TextBox1_TextChanged1(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class