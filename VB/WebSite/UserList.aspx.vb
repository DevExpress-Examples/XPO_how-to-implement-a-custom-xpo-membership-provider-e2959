Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Xpo

Partial Public Class Default2
    Inherits System.Web.UI.Page

    Private session As Session = XpoHelper.GetNewSession()

    Private ReadOnly RestrictedFields() As String = { "Email", "Password", "PasswordQuestion", "PasswordAnswer" }

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        xpoUsers.Session = session
    End Sub
    Protected Sub gvUsers_CustomColumnDisplayText(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs)
        If RestrictedFields.Contains(e.Column.FieldName) Then
            e.DisplayText = "********"
        End If
    End Sub
    Protected Sub gvUsers_HtmlDataCellPrepared(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewTableDataCellEventArgs)
        If RestrictedFields.Contains(e.DataColumn.FieldName) Then
            e.Cell.ForeColor = System.Drawing.Color.Red
        End If
    End Sub
End Class