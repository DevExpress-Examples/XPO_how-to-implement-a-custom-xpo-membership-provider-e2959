Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic

Public Class XpoUser
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
	<Size(255), Indexed("UserName", Unique := True)> _
	Public Property ApplicationName() As String
		Get
			Return _ApplicationName
		End Get
		Set(ByVal value As String)
			SetPropertyValue("ApplicationName", _ApplicationName, value)
		End Set
	End Property

	Private _UserName As String
	<Size(255), Indexed("ApplicationName", Unique := True)> _
	Public Property UserName() As String
		Get
			Return _UserName
		End Get
		Set(ByVal value As String)
			SetPropertyValue("UserName", _UserName, value)
		End Set
	End Property

	Private _Email As String
	<Size(128)> _
	Public Property Email() As String
		Get
			Return _Email
		End Get
		Set(ByVal value As String)
			SetPropertyValue("Email", _Email, value)
		End Set
	End Property

	Private _Comment As String
	<Size(SizeAttribute.Unlimited)> _
	Public Property Comment() As String
		Get
			Return _Comment
		End Get
		Set(ByVal value As String)
			SetPropertyValue("Comment", _Comment, value)
		End Set
	End Property

	Private _Password As String
	<Size(128)> _
	Public Property Password() As String
		Get
			Return _Password
		End Get
		Set(ByVal value As String)
			SetPropertyValue("Password", _Password, value)
		End Set
	End Property

	Private _PasswordQuestion As String
	<Size(255)> _
	Public Property PasswordQuestion() As String
		Get
			Return _PasswordQuestion
		End Get
		Set(ByVal value As String)
			SetPropertyValue("PasswordQuestion", _PasswordQuestion, value)
		End Set
	End Property

	Private _PasswordAnswer As String
	<Size(255)> _
	Public Property PasswordAnswer() As String
		Get
			Return _PasswordAnswer
		End Get
		Set(ByVal value As String)
			SetPropertyValue("PasswordAnswer", _PasswordAnswer, value)
		End Set
	End Property

	Private _IsApproved As Boolean
	Public Property IsApproved() As Boolean
		Get
			Return _IsApproved
		End Get
		Set(ByVal value As Boolean)
			SetPropertyValue("IsApproved", _IsApproved, value)
		End Set
	End Property

	Private _LastActivityDate As DateTime
	Public Property LastActivityDate() As DateTime
		Get
			Return _LastActivityDate
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("LastActivityDate", _LastActivityDate, value)
		End Set
	End Property

	Private _LastLoginDate As DateTime
	Public Property LastLoginDate() As DateTime
		Get
			Return _LastLoginDate
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("LastLoginDate", _LastLoginDate, value)
		End Set
	End Property

	Private _LastPasswordChangedDate As DateTime
	Public Property LastPasswordChangedDate() As DateTime
		Get
			Return _LastPasswordChangedDate
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("LastPasswordChangedDate", _LastPasswordChangedDate, value)
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

	Public ReadOnly Property IsOnline() As Boolean
		Get
			Dim span As New TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0)
			Dim time As DateTime = DateTime.UtcNow.Subtract(span)
			Return (Me.LastActivityDate.ToUniversalTime() > time)
		End Get
	End Property

	Private _IsLockedOut As Boolean
	Public Property IsLockedOut() As Boolean
		Get
			Return _IsLockedOut
		End Get
		Set(ByVal value As Boolean)
			SetPropertyValue("IsLockedOut", _IsLockedOut, value)
		End Set
	End Property

	Private _LastLockedOutDate As DateTime
	Public Property LastLockedOutDate() As DateTime
		Get
			Return _LastLockedOutDate
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("LastLockedOutDate", _LastLockedOutDate, value)
		End Set
	End Property

	Private _FailedPasswordAttemptCount As Integer
	Public Property FailedPasswordAttemptCount() As Integer
		Get
			Return _FailedPasswordAttemptCount
		End Get
		Set(ByVal value As Integer)
			SetPropertyValue("FailedPasswordAttemptCount", _FailedPasswordAttemptCount, value)
		End Set
	End Property

	Private _FailedPasswordAttemptWindowStart As DateTime
	Public Property FailedPasswordAttemptWindowStart() As DateTime
		Get
			Return _FailedPasswordAttemptWindowStart
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("FailedPasswordAttemptWindowStart", _FailedPasswordAttemptWindowStart, value)
		End Set
	End Property

	Private _FailedPasswordAnswerAttemptCount As Integer
	Public Property FailedPasswordAnswerAttemptCount() As Integer
		Get
			Return _FailedPasswordAnswerAttemptCount
		End Get
		Set(ByVal value As Integer)
			SetPropertyValue("FailedPasswordAnswerAttemptCount", _FailedPasswordAnswerAttemptCount, value)
		End Set
	End Property

	Private _FailedPasswordAnswerAttemptWindowStart As DateTime
	Public Property FailedPasswordAnswerAttemptWindowStart() As DateTime
		Get
			Return _FailedPasswordAnswerAttemptWindowStart
		End Get
		Set(ByVal value As DateTime)
			SetPropertyValue("FailedPasswordAnswerAttemptWindowStart", _FailedPasswordAnswerAttemptWindowStart, value)
		End Set
	End Property

	<Association("XpoRoles-XpoUsers")> _
	Public ReadOnly Property Roles() As XPCollection(Of XpoRole)
		Get
			Return GetCollection(Of XpoRole)("Roles")
		End Get
	End Property
End Class

