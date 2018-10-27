Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class Account_ChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub ChangePassword_ChangePasswordError(ByVal sender As Object, ByVal e As EventArgs)
        GenerateErrorRowScript("changeErrorRowKey", "changePasswordErrorRow")
    End Sub

    Private Sub GenerateErrorRowScript(ByVal key As String, ByVal errorRowId As String)
        ' Define the type of the client scripts on the page.
        Dim cstype As Type = Me.GetType()

        ' Get a ClientScriptManager reference from the Page class.
        Dim cs As ClientScriptManager = Page.ClientScript

        ' Check to see if the startup script is already registered.
        If Not cs.IsStartupScriptRegistered(cstype, key) Then
            cs.RegisterStartupScript(cstype, key, String.Format("document.getElementById(""{0}"").style.display=""block"";", errorRowId), True)
        End If
    End Sub
    Protected Sub ChangePassword_ContinueButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim continueUrl As String = ChangePassword.ContinueDestinationPageUrl
        If String.IsNullOrEmpty(continueUrl) Then
            continueUrl = "~/"
        End If
        Response.Redirect(continueUrl)
    End Sub
End Class