Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Principal
Imports System.Data.SqlClient

Public Class KeyAuthorizeAttribute
    Inherits System.Web.Http.AuthorizeAttribute
    'Protected Overrides Function AuthorizeCore(httpContext As HttpContextBase) As Boolean
    '    'Dim key As String = httpContext.Request("X-Key")
    '    Return False 'ApiValidatorService.IsValid(key)
    'End Function

    Private Const BasicAuthResponseHeaderValue = "Basic"

    Protected Overrides Function IsAuthorized(actionContext As Http.Controllers.HttpActionContext) As Boolean

        If actionContext Is Nothing Then
            '            Throw New HttpException(New beaconresponse(99, "", "Authentication Data Not Porvided.").generateResponseMesage(Request, New Uri(Url.Link("DefaultApi2", New With {.id = Nothing})), HttpStatusCode.NotFound))
            Throw New ArgumentNullException("actionContext")
        End If

        If AuthorizeRequest(actionContext.ControllerContext.Request) Then
            Return True
        Else
            Return False
        End If
        'MyBase.IsAuthorized(actionContext)
    End Function

    Public Overrides Sub OnAuthorization(actionContext As Http.Controllers.HttpActionContext)
        If actionContext Is Nothing Then
            Throw New ArgumentNullException("actionContext")
        End If

        If AuthorizeRequest(actionContext.ControllerContext.Request) Then
            Return
        End If

        Me.HandleUnauthorizedRequest(actionContext)
    End Sub

    Private Function AuthorizeRequest(request As HttpRequestMessage) As Boolean
        Dim authValue As AuthenticationHeaderValue = request.Headers.Authorization
        If authValue Is Nothing OrElse [String].IsNullOrWhiteSpace(authValue.Parameter) OrElse [String].IsNullOrWhiteSpace(authValue.Scheme) OrElse authValue.Scheme <> BasicAuthResponseHeaderValue Then
            Return False
        End If

        'Dim parsedHeader As String() = ParseAuthorizationHeader(authValue.Parameter)
        'If parsedHeader Is Nothing Then
        '    Return False
        'End If

        'Match Guid From DB
        Dim dbobj As New database
        Dim test = dbobj.queryGetValue2("select uname from users where guid = @guid", {New SqlParameter("@guid", authValue.Parameter)})

        If test = "" OrElse test Is Nothing OrElse test Is DBNull.Value Then
            Return False
        Else
            Return True
        End If

        'Return 'String.Equals("cce14732-6ebb-46d4-a8b1-317e67549402", authValue.Parameter) 'validateAuthorization(authValue.Parameter)
        'Dim principal As IPrincipal = Nothing
        'If TryCreatePrincipal(parsedHeader(0), parsedHeader(1), principal) Then
        '    HttpContext.Current.User = principal
        '    Return CheckRoles(principal) AndAlso CheckUsers(principal)
        'Else
        '    Return False
        'End If
    End Function

    Private Function ParseAuthorizationHeader(authHeader As String) As String()
        Dim credentials As String() = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(":")
        If credentials.Length <> 2 OrElse String.IsNullOrEmpty(credentials(0)) OrElse String.IsNullOrEmpty(credentials(1)) Then
            Return Nothing
        End If
        Return credentials
    End Function

    'Private Function AuthorizationDisabled(actionContext As Http.Controllers.HttpActionContext) As Boolean
    '    If Not actionContext.ActionDescriptor.GetCustomAttributes(
    'End Function

End Class