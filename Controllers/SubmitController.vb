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
        Dim base64img As String = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASoAAAA3CAYAAAC7BQfeAAAJd0lEQVR4Xu2df2wURRTHX39Da2uiLRHlt8aAFPijASJIYsUYBFSgaEsUEIKgUjQGBRNRAhGjVPEHJaFgQAoqIIgk0CoBIZGiacEILUKC0iJgjC2JtELoT5w31znW6+11j9u529t+NyFX7vbeznxm5rtv3rzZi7kuDsIBAiAAAg4mEAOhcnDroGggAAKSAIQKHQEEQMDxBCBUjm8iFBAEQABChT4AAiDgeAIQKsc3EQoIAiAAoUIfAAEQcDwBCJXjmwgFBAEQgFChD4AACDieAITK8U2EAoIACECo0AdAAAQcTwBCZWiiyuo6emLpLmq42kSZ/dLp1pQkSktJpCH9MuRZ6r3U5ERaULifYmJi6OD7uZYb+fKVRsqcs5GuNbfK7/DmJWHC+8rvJcTF0uxxQyj3wYE0pH+6ZduRONGsPqos3RLjqWr9s5JjNB3cD478epHKTl6k7ypqqLXtumwnY5uptlr1QjblibbCoZcAhKqd79o9x+nNzw6390bx0t4xA+FnoanbOd9yC72x8Qdat/eEUCaPfd5myWKnXj0DwfOecVA4ccCzSA2du4muNjZ3qI8CwnW5pXsi1WyZa5lRJE/sNW0tXWvy3ESUMHVsE2P7eP7m9jn/xbxIFt311+7yQsUDbsGaA1RaXi0be+6EobToqRH0x98NVH+1kfjzKnGH5aOypo7qxf/5vZPnLslBWL35OUudhO/Ok5Z+I8/9viDX1Fviu/nWQ6dow7dV1NzS6hUyJw14rj/XpUrwSE6KpxPrOnpNfM7dM9YLPY6h2iDE3BJMTSdlTF0jLbNXO2XMvTR68F3yX58eqX6v+OXB08KzPsC3F3HDytdUKpiVN46uvCmZxWPGeyVCkJqIp3OF+WNp/IgBtvcM4xTptSeH06LcEZav4cQB3/fpIrpyrUVMhW+n3csnm07t0nMKZT2jZRB7yhtj2Uvmm0r2q1ujqo6WO57DTuyyQuWdhokGGXXfnVS8eLyWWIqv93Hu8+CnCE4b8Mrz+G3THFNmXO97Zn4qu3vtDuvT40iOj2CEir2pJWIqz/VM6ZZAN9OukaxrtF27ywkVT+lmriyR0xY+gvVwgmlgYxxncN/A3kcgu0oYnDLgOxvQSpwrq2ujahBb4cx1W7LxsJien5ZNxoseK2Y9oOUmF0xfc/u5XUqo1F2Qp3q90lOlF6VzZS1fxC+2iQ5tFsex2rmc5lEFEioeyMNEkP2KCLKHIs5W2dh5HtdLLWSwXeNiR/ekBCpZkUOPLfla1o1DBStmjaFp2Vjxs7MNzGx1CaHyDZiH4y7IoviSCNJzh969bHJIgmjlTh+OzqKuYVYe7zRXxG6Su/kPsoeznMFei1f9GttTR/4vVIY0EvEB123P2zkhtWmwZevq57teqDjgyVO987UNYbsL8jUniXws9tw+mT82pLuuE2M9/oTKGIuLNk8qkAhwW05/t4Qu1NXL05THxTl1weTQdXWhCbX+rhaqldvKqeCrCsmIB0/x4gmmS82hgjR+X62K5YlpAa8khnJw0L9I5Hg5KWCrpn6FC8bKZEe3ipSx/3BmW5shx43706EP8kJpWnw3CAKuFCoOmPO0i9MP+NAZMPfH2sqqmJU2spp7ZcWWneeoKRInvMbGSj+D2kT29h23JdOqedk0clDPqAwuGzPS9x31ZKRHov/Y2VZuseU6oSopPyuT8FRu1GYRMOekvXAena2KWSlLKLlXVuyHek7h7l9o+eYymWTve6itQYo7v6qtSByz07mAEUy9jMJUVnVR9hnvISqWlBhHpe9MdUx5g6mb2851jVDxwC7YXkFFe4/LNho3vL+cdkVin1mowW/foLTTcnR4SsSceWCndE+giSMHiOTPDBEHrJdZ/D+d+lNMkwIPld4ZqWIankbqVe2jHCySSO1qM/ZIOTO+sqZW7ChoErsN6mWskvly6oRxhY9LyyvBozM92eiBMtLdJgLRUB/XCJWKC/Gdm7fAPD9xWMT4h+JR8V1eLYE7KSjN02lOtfhw51Fqbm3z3gx4id5siwkLhdqCdFlsR2IRYzsX6hpM28a419HfSZ19zt+xcg67gvHxsZRjYatMxDoSLuwl4BqhUuLAKzGRnlrcrEflDd6KQeSE5X0WmdKKatoqUi1UvI9FICE+jna89XhIU2oWLPbAWJh5/6TaRym9IOOOYJ/BakWE1NSThZ69s97Cc+sjPLg08bfqG+EOB0BzQiPgGqG6WXEIDZ//bwdbFqMXxRbDHfz3rQWLBYsTx/tU3IZjS7wPkhMcMch19BrYDEQAQqWhf2TkrJFB5t+LzffCqcv6elGRSiRk74m3hSwrPuKd2nEZeR9kXvYgIVL9bYsdaUAOky4nAKHS0MAcL7va2CJzjFab5FGxF8UpFOHYcxioijwFK9he7vWe1NTulSlZUqDM4k8asMEkCJgScI9QBeHF6O4PxieF+stM901EXZ3/cNjjajyt44f4qdiT8p7miUUIHY+60c0c9t1NwDVC5Vn1a3bMEyXVXr80EdvZ8voE+uffRum17Co7491PFo5YlHzIX80luUR/XnhP7MH9KB6zq9IHVOyJV0rhPbl7sEdz7VwjVDwgB87eQC1i6TzU/XV2NCh7VQuLDtHPZ/4yPFr4xmNs1TXMVrE6W93q7HO2H8g2r9wtnT5KBsftyluygxtsgIA/Aq4RKq6c59Gw+ylePEr2oxcfishD97kMC9celAFpFgo+bjwDPdxCJba4xMXQ/QN7Uqb4oYjeGWlyiqmSKzEkQCBaCLhKqBj6jX1o1ylReA3h+pUQFigOSnPmswpIczIhr5bxcj68lmgZEiinEwn4Fapjx445sayWy7TvRC19XHpWbir1N/0x+9UX4wV8vxdoqmX8rEdaEj0zphc9MtTzE1s4QAAEgiOQlZXV4QuuFCpVS6NgmYmQ1RhRZ0LF082XHx0AgQquT+JsEOhAwLJQgR0IgAAIOImA62JUToKLsoAACNhDAEJlD0dYAQEQ0EgAQqURLkyDAAjYQwBCZQ9HWAEBENBIAEKlES5MgwAI2EMAQmUPR1gBARDQSABCpREuTIMACNhDAEJlD0dYAQEQ0EgAQqURLkyDAAjYQwBCZQ9HWAEBENBIAEKlES5MgwAI2EMAQmUPR1gBARDQSABCpREuTIMACNhDAEJlD0dYAQEQ0EgAQqURLkyDAAjYQwBCZQ9HWAEBENBIAEKlES5MgwAI2EMAQmUPR1gBARDQSOA/MAX+mAKfNhoAAAAASUVORK5CYII="
        Dim testFormObj As New tooFormObj(1, "yesNaid", 1234, "glor", "18 dell laptops, 3 desktops", base64img, "", "1/1/11", "glor@macalester.edu")
        Return testFormObj
    End Function

    ' POST api/submit
    Public Function PostValue(<FromBody()> ByVal value As tooFormObj) As Boolean
        Try
            'adding tooFormObj to db
            Dim sender As New sendemail

            If System.Configuration.ConfigurationManager.AppSettings("testmode") = "on" Then
                'Test Mode - send a receipt they included an email.
                sender.sendhtml("glor@pcsforpeople.com", "PCs for People - Transfer of Ownership form", "<div id='submitted_too_form'>" + value.formatted + " </div>", value.pdf64)

            Else
                'site is live sent the receipt
                sender.sendhtml(value.donorEmail, "PCs for People - Transfer of Ownership form", "<div id='submitted_too_form'>" + value.formatted + " </div>", value.pdf64)
            End If

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
