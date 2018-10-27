Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Security

Partial Public Class NewAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub CreateUser_CreateUserError(ByVal sender As Object, ByVal e As CreateUserErrorEventArgs)
        ' Define the name and type of the client scripts on the page.
        Dim csErrorRowName As String = "errorRowScript"
        Dim cstype As Type = Me.GetType()

        ' Get a ClientScriptManager reference from the Page class.
        Dim cs As ClientScriptManager = Page.ClientScript

        ' Check to see if the startup script is already registered.
        If Not cs.IsStartupScriptRegistered(cstype, csErrorRowName) Then
            cs.RegisterStartupScript(cstype, csErrorRowName, "document.getElementById(""errorRow"").style.display=""block"";", True)
        End If
    End Sub

    Protected Sub CreateUser_ContinueButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        FormsAuthentication.SetAuthCookie(CreateUser.UserName, False)

        Dim continueUrl As String = CreateUser.ContinueDestinationPageUrl
        If String.IsNullOrEmpty(continueUrl) Then
            continueUrl = "~/"
        End If
        Response.Redirect(continueUrl)
    End Sub
End Class