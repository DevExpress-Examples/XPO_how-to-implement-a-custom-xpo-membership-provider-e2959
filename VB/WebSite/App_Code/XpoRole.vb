Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic

Public Class XpoRole
	Inherits XPObject

	Public Sub New()
		MyBase.New()
	End Sub

	Public Sub New(ByVal session As Session)
		MyBase.New(session)
	End Sub

	Public Overrides Sub AfterConstruction()
		MyBase.AfterConstruction()
	End Sub

	Private _ApplicationName As String
	<Size(255), Indexed("RoleName", Unique := True)> _
	Public Property ApplicationName() As String
		Get
			Return _ApplicationName
		End Get
		Set(ByVal value As String)
			SetPropertyValue("ApplicationName", _ApplicationName, value)
		End Set
	End Property

	Private _RoleName As String
	<Size(255), Indexed("ApplicationName", Unique := True)> _
	Public Property RoleName() As String
		Get
			Return _RoleName
		End Get
		Set(ByVal value As String)
			SetPropertyValue("RoleName", _RoleName, value)
		End Set
	End Property

	Private _Description As String
	<Size(255)> _
	Public Property Description() As String
		Get
			Return _Description
		End Get
		Set(ByVal value As String)
			SetPropertyValue("Description", _Description, value)
		End Set
	End Property

	Private _CreationDate As DateTime
	Public Property CreationDate() As DateTime
		Get
			Return _CreationDate
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("CreationDate", _CreationDate, value)
		End Set
	End Property

	<Association("XpoRoles-XpoUsers")> _
	Public ReadOnly Property Users() As XPCollection(Of XpoUser)
		Get
			Return GetCollection(Of XpoUser)("Users")
		End Get
	End Property
End Class

