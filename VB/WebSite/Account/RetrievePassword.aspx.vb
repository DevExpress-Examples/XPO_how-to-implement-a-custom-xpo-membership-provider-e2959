Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class Account_RetrievePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub PasswordRecovery_UserLookupError(ByVal sender As Object, ByVal e As EventArgs)
        GenerateErrorRowScript("userNameErrorKey", "userNameErrorRow")
    End Sub
    Protected Sub PasswordRecovery_AnswerLookupError(ByVal sender As Object, ByVal e As EventArgs)
        GenerateErrorRowScript("answerErrorRowKey", "answerErrorRow")
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
    Protected Sub PasswordRecovery_SendMailError(ByVal sender As Object, ByVal e As SendMailErrorEventArgs)
        ' we do not send any emails
        e.Handled = True
    End Sub
End Class