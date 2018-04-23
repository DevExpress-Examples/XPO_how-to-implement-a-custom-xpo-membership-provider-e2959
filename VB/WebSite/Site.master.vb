Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Security
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxTreeView

Partial Public Class Site
	Inherits System.Web.UI.MasterPage
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		lbOnline.Text = Membership.GetNumberOfUsersOnline().ToString()
	End Sub

	Protected Sub lbLoginName_Load(ByVal sender As Object, ByVal e As EventArgs)
		Dim lbLoginName As ASPxLabel = TryCast(sender, ASPxLabel)
		Dim userName As String = Page.User.Identity.Name

		If (Not String.IsNullOrEmpty(userName)) Then
			lbLoginName.Text = userName
		End If
	End Sub

	Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As EventArgs)
		FormsAuthentication.SignOut()
		Response.Redirect(Page.Request.RawUrl)
	End Sub
	Protected Sub tvSiteMap_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim tvSiteMap As ASPxTreeView = TryCast(sender, ASPxTreeView)

		If (Not IsPostBack) Then
			tvSiteMap.ExpandAll()
		End If
	End Sub
End Class
