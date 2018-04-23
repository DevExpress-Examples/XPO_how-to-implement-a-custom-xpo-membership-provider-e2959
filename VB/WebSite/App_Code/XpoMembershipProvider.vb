Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.Configuration
Imports System.Configuration.Provider
Imports System.Configuration
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Security.Cryptography

''' <summary>
''' Summary description for XpoMembershipProvider
''' </summary>
Public NotInheritable Class XpoMembershipProvider
	Inherits MembershipProvider
	Private Enum FailureType
		Password = 1
		PasswordAnswer = 2
	End Enum

	'
	' Global connection String.
	'

	Private connectionString As String

	'
	' Used when determining encryption key values.
	'

	Private machineKey As MachineKeySection

	'
	' If false, exceptions are thrown to the caller. If true,
	' exceptions are written to the event log.
	'

	Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
		'
		' Initialize values from web.config.
		'

		If config Is Nothing Then
			Throw New ArgumentNullException("config")
		End If

		If name Is Nothing OrElse name.Length = 0 Then
			name = "XpoMembershipProvider"
		End If

		If String.IsNullOrEmpty(config("description")) Then
			config.Remove("description")
			config.Add("description", "XPO Membership provider")
		End If

		' Initialize the abstract base class.
		MyBase.Initialize(name, config)

		pApplicationName = GetConfigValue(config("applicationName"), System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
		pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config("maxInvalidPasswordAttempts"), "5"))
		pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config("passwordAttemptWindow"), "10"))
		pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config("minRequiredNonAlphanumericCharacters"), "1"))
		pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config("minRequiredPasswordLength"), "7"))
		pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config("passwordStrengthRegularExpression"), ""))
		pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config("enablePasswordReset"), "true"))
		pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config("enablePasswordRetrieval"), "true"))
		pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config("requiresQuestionAndAnswer"), "false"))
		pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config("requiresUniqueEmail"), "true"))

		Dim temp_format As String = config("passwordFormat")
		If temp_format Is Nothing Then
			temp_format = "Hashed"
		End If

		Select Case temp_format
			Case "Hashed"
				pPasswordFormat = MembershipPasswordFormat.Hashed
			Case "Encrypted"
				pPasswordFormat = MembershipPasswordFormat.Encrypted
			Case "Clear"
				pPasswordFormat = MembershipPasswordFormat.Clear
			Case Else
				Throw New ProviderException("Password format not supported.")
		End Select

		'
		' Initialize XPO Connection string.
		'

		Dim ConnectionStringSettings As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(config("connectionStringName"))

		If ConnectionStringSettings Is Nothing OrElse ConnectionStringSettings.ConnectionString.Trim() = "" Then
			Throw New ProviderException("Connection String cannot be blank.")
		End If

		connectionString = ConnectionStringSettings.ConnectionString


		' Get encryption and decryption key information from the configuration.
		Dim cfg As Configuration = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
		machineKey = CType(cfg.GetSection("system.web/machineKey"), MachineKeySection)

		If machineKey.ValidationKey.Contains("AutoGenerate") Then
			If PasswordFormat <> MembershipPasswordFormat.Clear Then
				Throw New ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.")
			End If
		End If

	End Sub

	'
	' A helper function to retrieve config values from the configuration file.
	'

	Private Function GetConfigValue(ByVal configValue As String, ByVal defaultValue As String) As String
		If String.IsNullOrEmpty(configValue) Then
			Return defaultValue
		End If

		Return configValue
	End Function

	'
	' System.Web.Security.MembershipProvider properties.
	'

	Private pApplicationName As String
	Private pEnablePasswordReset As Boolean
	Private pEnablePasswordRetrieval As Boolean
	Private pRequiresQuestionAndAnswer As Boolean
	Private pRequiresUniqueEmail As Boolean
	Private pMaxInvalidPasswordAttempts As Integer
	Private pPasswordAttemptWindow As Integer
	Private pPasswordFormat As MembershipPasswordFormat

	Public Overrides Property ApplicationName() As String
		Get
			Return pApplicationName
		End Get
		Set(ByVal value As String)
			pApplicationName = value
		End Set
	End Property

	Public Overrides ReadOnly Property EnablePasswordReset() As Boolean
		Get
			Return pEnablePasswordReset
		End Get
	End Property

	Public Overrides ReadOnly Property EnablePasswordRetrieval() As Boolean
		Get
			Return pEnablePasswordRetrieval
		End Get
	End Property

	Public Overrides ReadOnly Property RequiresQuestionAndAnswer() As Boolean
		Get
			Return pRequiresQuestionAndAnswer
		End Get
	End Property

	Public Overrides ReadOnly Property RequiresUniqueEmail() As Boolean
		Get
			Return pRequiresUniqueEmail
		End Get
	End Property

	Public Overrides ReadOnly Property MaxInvalidPasswordAttempts() As Integer
		Get
			Return pMaxInvalidPasswordAttempts
		End Get
	End Property

	Public Overrides ReadOnly Property PasswordAttemptWindow() As Integer
		Get
			Return pPasswordAttemptWindow
		End Get
	End Property

	Public Overrides ReadOnly Property PasswordFormat() As MembershipPasswordFormat
		Get
			Return pPasswordFormat
		End Get
	End Property

	Private pMinRequiredNonAlphanumericCharacters As Integer

	Public Overrides ReadOnly Property MinRequiredNonAlphanumericCharacters() As Integer
		Get
			Return pMinRequiredNonAlphanumericCharacters
		End Get
	End Property

	Private pMinRequiredPasswordLength As Integer

	Public Overrides ReadOnly Property MinRequiredPasswordLength() As Integer
		Get
			Return pMinRequiredPasswordLength
		End Get
	End Property

	Private pPasswordStrengthRegularExpression As String

	Public Overrides ReadOnly Property PasswordStrengthRegularExpression() As String
		Get
			Return pPasswordStrengthRegularExpression
		End Get
	End Property


	Public Overrides Function ChangePassword(ByVal username As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
		Dim args As New ValidatePasswordEventArgs(username, newPassword, False)

		OnValidatingPassword(args)

		If args.Cancel Then
			If args.FailureInformation IsNot Nothing Then
				Throw args.FailureInformation
			Else
				Throw New Exception("Change password canceled due to new password validation failure.")
			End If
		End If

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)))
			If user IsNot Nothing Then
				user.Password = EncodePassword(newPassword)
				user.LastPasswordChangedDate = DateTime.Now
			Else
				Return False
			End If
			user.Save()
		End Using

		Return True
	End Function

	Public Overrides Function ChangePasswordQuestionAndAnswer(ByVal username As String, ByVal password As String, ByVal newPasswordQuestion As String, ByVal newPasswordAnswer As String) As Boolean
		Throw New NotImplementedException()
	End Function

	Public Overrides Function CreateUser(ByVal username As String, ByVal password As String, ByVal email As String, ByVal passwordQuestion As String, ByVal passwordAnswer As String, ByVal isApproved As Boolean, ByVal providerUserKey As Object, <System.Runtime.InteropServices.Out()> ByRef status As MembershipCreateStatus) As MembershipUser
		Dim args As New ValidatePasswordEventArgs(username, password, True)

		OnValidatingPassword(args)

		If args.Cancel Then
			status = MembershipCreateStatus.InvalidPassword
			Return Nothing
		End If

		If RequiresQuestionAndAnswer AndAlso String.IsNullOrEmpty(passwordAnswer) Then
			status = MembershipCreateStatus.InvalidAnswer
			Return Nothing
		End If

		If RequiresUniqueEmail Then
			If (Not IsEmail(email)) Then
				status = MembershipCreateStatus.InvalidEmail
				Return Nothing
			End If
			If (Not String.IsNullOrEmpty(GetUserNameByEmail(email))) Then
				status = MembershipCreateStatus.DuplicateEmail
				Return Nothing
			End If
		End If

		Dim mUser As MembershipUser = GetUser(username, False)

		If mUser IsNot Nothing Then
			status = MembershipCreateStatus.DuplicateUserName
			Return Nothing
		End If

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As New XpoUser(session) With {.ApplicationName = ApplicationName, .UserName = username, .Password = EncodePassword(password), .Email = email, .PasswordQuestion = passwordQuestion, .PasswordAnswer = EncodePassword(passwordAnswer), .IsApproved = isApproved, .CreationDate = DateTime.Now, .FailedPasswordAnswerAttemptCount = 0, .FailedPasswordAnswerAttemptWindowStart = DateTime.MinValue, .IsLockedOut = False, .LastActivityDate = DateTime.Now, .LastLockedOutDate = DateTime.MinValue, .FailedPasswordAttemptCount = 0, .FailedPasswordAttemptWindowStart = DateTime.MinValue}

			user.Save()
			status = MembershipCreateStatus.Success
		End Using

		Return GetUser(username, False)
	End Function

	' Used by Web Site Administration Tool 

	Public Overrides Function DeleteUser(ByVal username As String, ByVal deleteAllRelatedData As Boolean) As Boolean
		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)))

			If user Is Nothing Then
				Return False
			End If

			user.Delete()
			user.Save()
		End Using
		Return True
	End Function

	' Used by Web Site Administration Tool 

	Public Overrides Function FindUsersByEmail(ByVal emailToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, <System.Runtime.InteropServices.Out()> ByRef totalRecords As Integer) As MembershipUserCollection
		Dim mclUsers As New MembershipUserCollection()

		Using session As Session = XpoHelper.GetNewSession()
			Dim xpcUsers As XPCollection(Of XpoUser) = New XPCollection(Of XpoUser)(session, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("Email", String.Format("%{0}%", emailToMatch), BinaryOperatorType.Like)), New SortProperty("UserName", DevExpress.Xpo.DB.SortingDirection.Ascending))

			totalRecords = Convert.ToInt32(session.Evaluate(Of XpoUser)(CriteriaOperator.Parse("Count()"), New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("Email", String.Format("%{0}%", emailToMatch), BinaryOperatorType.Like))))

			xpcUsers.SkipReturnedObjects = pageIndex * pageSize
			xpcUsers.TopReturnedObjects = pageSize

			For Each xpoUser As XpoUser In xpcUsers
				Dim mUser As MembershipUser = GetUserFromXpoUser(xpoUser)
				mclUsers.Add(mUser)
			Next xpoUser
		End Using

		Return mclUsers
	End Function

	' Used by Web Site Administration Tool 

	Public Overrides Function FindUsersByName(ByVal usernameToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, <System.Runtime.InteropServices.Out()> ByRef totalRecords As Integer) As MembershipUserCollection
		Dim mclUsers As New MembershipUserCollection()

		Using session As Session = XpoHelper.GetNewSession()
			Dim xpcUsers As XPCollection(Of XpoUser) = New XPCollection(Of XpoUser)(session, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", String.Format("%{0}%", usernameToMatch), BinaryOperatorType.Like)), New SortProperty("UserName", DevExpress.Xpo.DB.SortingDirection.Ascending))

			totalRecords = Convert.ToInt32(session.Evaluate(Of XpoUser)(CriteriaOperator.Parse("Count()"), New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", String.Format("%{0}%", usernameToMatch), BinaryOperatorType.Like))))

			xpcUsers.SkipReturnedObjects = pageIndex * pageSize
			xpcUsers.TopReturnedObjects = pageSize

			For Each xpoUser As XpoUser In xpcUsers
				Dim mUser As MembershipUser = GetUserFromXpoUser(xpoUser)
				mclUsers.Add(mUser)
			Next xpoUser
		End Using

		Return mclUsers
	End Function

	' Used by Web Site Administration Tool 

	Public Overrides Function GetAllUsers(ByVal pageIndex As Integer, ByVal pageSize As Integer, <System.Runtime.InteropServices.Out()> ByRef totalRecords As Int32) As MembershipUserCollection
		Dim mclUsers As New MembershipUserCollection()

		Using session As Session = XpoHelper.GetNewSession()
			Dim xpcUsers As XPCollection(Of XpoUser) = New XPCollection(Of XpoUser)(session, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New SortProperty("UserName", DevExpress.Xpo.DB.SortingDirection.Ascending))

			totalRecords = Convert.ToInt32(session.Evaluate(Of XpoUser)(CriteriaOperator.Parse("Count()"), Nothing))

			xpcUsers.SkipReturnedObjects = pageIndex * pageSize
			xpcUsers.TopReturnedObjects = pageSize

			For Each xpoUser As XpoUser In xpcUsers
				Dim mUser As MembershipUser = GetUserFromXpoUser(xpoUser)
				mclUsers.Add(mUser)
			Next xpoUser
		End Using

		Return mclUsers
	End Function

	Public Overrides Function GetNumberOfUsersOnline() As Integer
		Using session As Session = XpoHelper.GetNewSession()
			Dim xpcUsers As XPCollection(Of XpoUser) = New XPCollection(Of XpoUser)(session, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("IsOnline", True, BinaryOperatorType.Equal)))

			Return xpcUsers.Count
		End Using
	End Function

	Public Overrides Function GetPassword(ByVal username As String, ByVal answer As String) As String
		If (Not EnablePasswordRetrieval) Then
			Throw New ProviderException("Password Retrieval Not Enabled.")
		End If

		If PasswordFormat = MembershipPasswordFormat.Hashed Then
			Throw New ProviderException("Cannot retrieve Hashed passwords.")
		End If

		Dim password As String
		Dim passwordAnswer As String

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)))

			If user Is Nothing Then
				Throw New MembershipPasswordException("The specified user is not found.")
			End If
			If user.IsLockedOut Then
				Throw New MembershipPasswordException("The specified user is locked out.")
			End If

			password = user.Password
			passwordAnswer = user.PasswordAnswer
		End Using

		If RequiresQuestionAndAnswer AndAlso (Not CheckPassword(answer, passwordAnswer)) Then
			UpdateFailureCount(username, FailureType.PasswordAnswer)

			Throw New MembershipPasswordException("Incorrect password answer.")
		End If

		If PasswordFormat = MembershipPasswordFormat.Encrypted Then
			password = DecodePassword(password)
		End If

		Return password
	End Function

	Public Overrides Overloads Function GetUser(ByVal username As String, ByVal userIsOnline As Boolean) As MembershipUser
		Dim mUser As MembershipUser

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)))

			If user Is Nothing Then
				Return Nothing
			End If

			mUser = GetUserFromXpoUser(user)

			If userIsOnline Then
				user.LastActivityDate = DateTime.Now
			End If

			user.Save()
		End Using

		Return mUser
	End Function

	Public Overrides Overloads Function GetUser(ByVal providerUserKey As Object, ByVal userIsOnline As Boolean) As MembershipUser
		Dim mUser As MembershipUser

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("Oid", providerUserKey, BinaryOperatorType.Equal)))

			If user Is Nothing Then
				Return Nothing
			End If

			mUser = GetUserFromXpoUser(user)

			If userIsOnline Then
				user.LastActivityDate = DateTime.Now
			End If

			user.Save()
		End Using

		Return mUser
	End Function

	Public Overrides Function GetUserNameByEmail(ByVal email As String) As String
		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("Email", email, BinaryOperatorType.Equal)))

			If user Is Nothing Then
				Return String.Empty
			End If

			Return user.UserName
		End Using
	End Function

	Public Overrides Function ResetPassword(ByVal username As String, ByVal answer As String) As String
		Throw New NotImplementedException("ResetPassword")
	End Function

	Public Overrides Function UnlockUser(ByVal userName As String) As Boolean
		Throw New NotImplementedException("UnlockUser")
	End Function

	' Used by Web Site Administration Tool 

	Public Overrides Sub UpdateUser(ByVal mUser As MembershipUser)
		Using session As Session = XpoHelper.GetNewSession()
			Dim xpoUser As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", mUser.UserName, BinaryOperatorType.Equal)))

			If xpoUser Is Nothing Then
				Throw New ProviderException("The specified user is not found.")
			End If

			xpoUser.Email = mUser.Email
			xpoUser.Comment = mUser.Comment
			xpoUser.IsApproved = mUser.IsApproved
			xpoUser.LastLoginDate = mUser.LastLoginDate
			xpoUser.LastActivityDate = mUser.LastActivityDate

			xpoUser.Save()
		End Using
	End Sub

	Public Overrides Function ValidateUser(ByVal username As String, ByVal password As String) As Boolean
		Dim isValid As Boolean = False

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)))

			If user Is Nothing Then
				Return False
			End If

			' http://msdn.microsoft.com/en-us/library/system.web.security.membershipuser.lastactivitydate.aspx

			If CheckPassword(password, user.Password) Then
				If ((Not user.IsLockedOut)) AndAlso (user.IsApproved) Then
					isValid = True
					user.LastLoginDate = DateTime.Now
					user.LastActivityDate = DateTime.Now

					user.Save()
				End If
			End If
		End Using

		Return isValid
	End Function

	Private Sub UpdateFailureCount(ByVal username As String, ByVal failureType As FailureType)
		Dim windowStart As DateTime
		Dim windowEnd As DateTime
		Dim failureCount As Integer

		Using session As Session = XpoHelper.GetNewSession()
			Dim user As XpoUser = session.FindObject(Of XpoUser)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)))

			Select Case failureType
				Case FailureType.Password
					failureCount = user.FailedPasswordAttemptCount
					windowStart = user.FailedPasswordAttemptWindowStart
					windowEnd = windowStart.AddMinutes(PasswordAttemptWindow)

					user.FailedPasswordAttemptWindowStart = DateTime.Now

					If DateTime.Now > windowEnd Then
						user.FailedPasswordAttemptCount = 1
					Else
						user.FailedPasswordAttemptCount += 1
					End If

					If user.FailedPasswordAttemptCount >= MaxInvalidPasswordAttempts Then
						If (Not user.IsLockedOut) Then
							user.LastLockedOutDate = DateTime.Now
							user.IsLockedOut = True
						End If
					End If

				Case FailureType.PasswordAnswer
					failureCount = user.FailedPasswordAnswerAttemptCount
					windowStart = user.FailedPasswordAnswerAttemptWindowStart
					windowEnd = windowStart.AddMinutes(PasswordAttemptWindow)

					user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now

					If DateTime.Now > windowEnd Then
						user.FailedPasswordAnswerAttemptCount = 1
					Else
						user.FailedPasswordAnswerAttemptCount += 1
					End If

					If user.FailedPasswordAnswerAttemptCount >= MaxInvalidPasswordAttempts Then
						If (Not user.IsLockedOut) Then
							user.LastLockedOutDate = DateTime.Now
							user.IsLockedOut = True
						End If
					End If
			End Select
			user.Save()
		End Using

	End Sub

	Private Function CheckPassword(ByVal password As String, ByVal dbpassword As String) As Boolean
		Dim pass1 As String = password
		Dim pass2 As String = dbpassword

		Select Case PasswordFormat
			Case MembershipPasswordFormat.Clear
			Case MembershipPasswordFormat.Encrypted
				pass2 = DecodePassword(dbpassword)
			Case MembershipPasswordFormat.Hashed
				pass1 = EncodePassword(password)
			Case Else
		End Select

		Return pass1 = pass2
	End Function

	Private Function DecodePassword(ByVal encodedPassword As String) As String
		Dim password As String = encodedPassword

		If String.IsNullOrEmpty(password) Then
			Return password
		End If

		Select Case PasswordFormat
			Case MembershipPasswordFormat.Clear
			Case MembershipPasswordFormat.Encrypted
				password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)))
			Case MembershipPasswordFormat.Hashed
				Throw New ProviderException("Cannot decode a hashed password.")
			Case Else
				Throw New ProviderException("Unsupported password format.")
		End Select

		Return password
	End Function

	Private Function EncodePassword(ByVal password As String) As String
		Dim encodedPassword As String = password

		If String.IsNullOrEmpty(encodedPassword) Then
			Return encodedPassword
		End If

		Select Case PasswordFormat
			Case MembershipPasswordFormat.Clear
			Case MembershipPasswordFormat.Encrypted
				encodedPassword = Convert.ToBase64String(Me.EncryptPassword(Encoding.Unicode.GetBytes(password)))
			Case MembershipPasswordFormat.Hashed
				Dim hash As New HMACSHA512(HexToByte(machineKey.ValidationKey))
				encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)))
			Case Else
				Throw New ProviderException("Unsupported password format.")
		End Select

		Return encodedPassword
	End Function

	Private Shared Function IsEmail(ByVal inputEmail As String) As Boolean
		Dim strRegex As String = "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" & "\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" & ".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
		Dim re As New Regex(strRegex)
		If re.IsMatch(inputEmail) Then
			Return (True)
		Else
			Return (False)
		End If
	End Function

	Private Shared Function HexToByte(ByVal hexString As String) As Byte()
		Dim returnBytes(hexString.Length / 2 - 1) As Byte
		For i As Integer = 0 To returnBytes.Length - 1
			returnBytes(i) = Convert.ToByte(hexString.Substring(i * 2, 2), 16)
		Next i
		Return returnBytes
	End Function

	Private Function GetUserFromXpoUser(ByVal xUser As XpoUser) As MembershipUser
		Dim mUser As New MembershipUser(Me.Name, xUser.UserName, xUser.Oid, xUser.Email, xUser.PasswordQuestion, xUser.Comment, xUser.IsApproved, xUser.IsLockedOut, xUser.CreationDate, xUser.LastLoginDate, xUser.LastActivityDate, xUser.LastPasswordChangedDate, xUser.LastLockedOutDate)
		Return mUser
	End Function
End Class