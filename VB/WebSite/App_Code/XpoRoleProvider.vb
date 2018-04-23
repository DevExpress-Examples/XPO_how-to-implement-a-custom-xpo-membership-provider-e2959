Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Configuration
Imports System.Configuration.Provider
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

''' <summary>
''' Summary description for XpoRoleProvider
''' </summary>
Public NotInheritable Class XpoRoleProvider
	Inherits RoleProvider

	'
	' Global connection String.
	'

	Private connectionString As String

	Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
		'
		' Initialize values from web.config.
		'

		If config Is Nothing Then
			Throw New ArgumentNullException("config")
		End If

		If name Is Nothing OrElse name.Length = 0 Then
			name = "XpoRoleProvider"
		End If

		If String.IsNullOrEmpty(config("description")) Then
			config.Remove("description")
			config.Add("description", "XPO Role provider")
		End If

		MyBase.Initialize(name, config)

		pApplicationName = GetConfigValue(config("applicationName"), System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)

		'
		' Initialize XPO Connection string.
		'

		Dim ConnectionStringSettings As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(config("connectionStringName"))

		If ConnectionStringSettings Is Nothing OrElse ConnectionStringSettings.ConnectionString.Trim() = "" Then
			Throw New ProviderException("Connection String cannot be blank.")
		End If

		connectionString = ConnectionStringSettings.ConnectionString
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
	' System.Web.Security.RoleProvider properties.
	'

	Private pApplicationName As String

	Public Overrides Property ApplicationName() As String
		Get
			Return pApplicationName
		End Get
		Set(ByVal value As String)
			pApplicationName = value
		End Set
	End Property

	Public Overrides Sub AddUsersToRoles(ByVal usernames() As String, ByVal roleNames() As String)
		For Each rolename As String In roleNames
			If (Not RoleExists(rolename)) Then
				Throw New ProviderException("Role name not found.")
			End If
		Next rolename

		For Each username As String In usernames
			If username.Contains(",") Then
				Throw New ArgumentException("User names cannot contain commas.")
			End If

			For Each rolename As String In roleNames
				If IsUserInRole(username, rolename) Then
					Throw New ProviderException("User is already in role.")
				End If
			Next rolename
		Next username

		Using uow As UnitOfWork = XpoHelper.GetNewUnitOfWork()
			Dim xpcUsers As New XPCollection(Of XpoUser)(uow, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New InOperator("UserName", usernames)))
			Dim xpcRoles As New XPCollection(Of XpoRole)(uow, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New InOperator("RoleName", roleNames)))

			For Each user As XpoUser In xpcUsers
				user.Roles.AddRange(xpcRoles)
			Next user

			uow.CommitChanges()
		End Using
	End Sub

	Public Overrides Sub CreateRole(ByVal roleName As String)
		CreateRole(roleName, String.Empty)
	End Sub

	Public Overloads Sub CreateRole(ByVal roleName As String, ByVal description As String)
		If String.IsNullOrEmpty(roleName) Then
			Throw New ProviderException("Role name cannot be empty or null.")
		End If
		If roleName.Contains(",") Then
			Throw New ArgumentException("Role names cannot contain commas.")
		End If
		If RoleExists(roleName) Then
			Throw New ProviderException("Role name already exists.")
		End If
		If roleName.Length > 256 Then
			Throw New ProviderException("Role name cannot exceed 256 characters.")
		End If

		Using session As Session = XpoHelper.GetNewSession()
			Dim role As New XpoRole(session) With {.RoleName = roleName, .ApplicationName = Me.ApplicationName, .Description = description}
			role.Save()
		End Using
	End Sub

	Public Overrides Function DeleteRole(ByVal roleName As String, ByVal throwOnPopulatedRole As Boolean) As Boolean
		If (Not RoleExists(roleName)) Then
			Throw New ProviderException("Role does not exist.")
		End If

		If throwOnPopulatedRole AndAlso GetUsersInRole(roleName).Length > 0 Then
			Throw New ProviderException("Cannot delete a populated role.")
		End If

		Using session As Session = XpoHelper.GetNewSession()
			Dim role As XpoRole = session.FindObject(Of XpoRole)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal)))
			role.Delete()
			role.Save()
		End Using

		Return Not RoleExists(roleName)
	End Function

	Public Overrides Function FindUsersInRole(ByVal roleName As String, ByVal userNameToMatch As String) As String()
		Dim users() As String

		If (Not RoleExists(roleName)) Then
			Throw New ProviderException("Role does not exist.")
		End If

        Using session As Session = XpoHelper.GetNewSession()
            Dim collection As CriteriaOperatorCollection = New CriteriaOperatorCollection()
            collection.Add(OperandProperty.Parse("UserName"))
            Dim xpvUsers As New XPView(session, GetType(XpoUser), collection, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("UserName", String.Format("%{0}%", userNameToMatch), BinaryOperatorType.Like), New ContainsOperator("Roles", New BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal))))

            Dim usersList As New List(Of String)()
            For i As Integer = 0 To xpvUsers.Count - 1
                usersList.Add(xpvUsers(i)(0).ToString())
            Next i
            users = usersList.ToArray()
        End Using

		Return users
	End Function

	Public Overrides Function GetAllRoles() As String()
		Dim roles() As String

		Using session As Session = XpoHelper.GetNewSession()
            Dim collection As CriteriaOperatorCollection = New CriteriaOperatorCollection()
            collection.Add(OperandProperty.Parse("RoleName"))
            Dim xpvRoles As New XPView(session, GetType(XpoRole), collection, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal))

			Dim rolesList As New List(Of String)()
			For i As Integer = 0 To xpvRoles.Count - 1
				rolesList.Add(xpvRoles(i)(0).ToString())
			Next i
			roles = rolesList.ToArray()
		End Using

		Return roles
	End Function

	Public Overrides Function GetRolesForUser(ByVal username As String) As String()
		If String.IsNullOrEmpty(username) Then
			Throw New ProviderException("User name cannot be empty or null.")
		End If

		Dim roles() As String

		Using session As Session = XpoHelper.GetNewSession()
            Dim collection As CriteriaOperatorCollection = New CriteriaOperatorCollection()
            collection.Add(OperandProperty.Parse("RoleName"))
            Dim xpvRoles As New XPView(session, GetType(XpoRole), collection, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New ContainsOperator("Users", New BinaryOperator("UserName", username, BinaryOperatorType.Equal))))
			Dim rolesList As New List(Of String)()
			For i As Integer = 0 To xpvRoles.Count - 1
				rolesList.Add(xpvRoles(i)(0).ToString())
			Next i
			roles = rolesList.ToArray()
		End Using

		Return roles
	End Function

	Public Overrides Function GetUsersInRole(ByVal roleName As String) As String()
		If String.IsNullOrEmpty(roleName) Then
			Throw New ProviderException("Role name cannot be empty or null.")
		End If
		If (Not RoleExists(roleName)) Then
			Throw New ProviderException("Role does not exist.")
		End If

		Dim users() As String

		Using session As Session = XpoHelper.GetNewSession()
            Dim collection As CriteriaOperatorCollection = New CriteriaOperatorCollection()
            collection.Add(OperandProperty.Parse("UserName"))
            Dim xpvUsers As New XPView(session, GetType(XpoUser), collection, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New ContainsOperator("Roles", New BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal))))

			Dim usersList As New List(Of String)()
			For i As Integer = 0 To xpvUsers.Count - 1
				usersList.Add(xpvUsers(i)(0).ToString())
			Next i
			users = usersList.ToArray()
		End Using

		Return users
	End Function

	Public Overrides Function IsUserInRole(ByVal username As String, ByVal roleName As String) As Boolean
		If String.IsNullOrEmpty(username) Then
			Throw New ProviderException("User name cannot be empty or null.")
		End If
		If String.IsNullOrEmpty(roleName) Then
			Throw New ProviderException("Role name cannot be empty or null.")
		End If

		Using session As Session = XpoHelper.GetNewSession()
			Dim collection As New XPCollection(Of XpoUser)(session, New GroupOperator(GroupOperatorType.And, New CriteriaOperator() { New ContainsOperator("Roles", New BinaryOperator("RoleName", roleName)), New BinaryOperator("UserName", username, BinaryOperatorType.Equal)}))

			Return (collection.Count <> 0)
		End Using
	End Function

	Public Overrides Sub RemoveUsersFromRoles(ByVal usernames() As String, ByVal roleNames() As String)
		For Each rolename As String In roleNames
			If String.IsNullOrEmpty(rolename) Then
				Throw New ProviderException("Role name cannot be empty or null.")
			End If
			If (Not RoleExists(rolename)) Then
				Throw New ProviderException("Role name not found.")
			End If
		Next rolename

		For Each username As String In usernames
			If String.IsNullOrEmpty(username) Then
				Throw New ProviderException("User name cannot be empty or null.")
			End If

			For Each rolename As String In roleNames
				If (Not IsUserInRole(username, rolename)) Then
					Throw New ProviderException("User is not in role.")
				End If
			Next rolename
		Next username

		Using uow As UnitOfWork = XpoHelper.GetNewUnitOfWork()
			Dim xpcRoles As New XPCollection(Of XpoRole)(uow, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New InOperator("RoleName", roleNames)))

			Dim xpcUsers As XPCollection(Of XpoUser)
			For Each role As XpoRole In xpcRoles
				xpcUsers = New XPCollection(Of XpoUser)(uow, New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New InOperator("UserName", usernames), New ContainsOperator("Roles", New BinaryOperator("RoleName", role.RoleName, BinaryOperatorType.Equal))))
				For i As Integer = xpcUsers.Count - 1 To 0 Step -1
					role.Users.Remove(xpcUsers(i))
				Next i
			Next role
			uow.CommitChanges()
		End Using
	End Sub

	Public Overrides Function RoleExists(ByVal roleName As String) As Boolean
		If String.IsNullOrEmpty(roleName) Then
			Throw New ProviderException("Role name cannot be empty or null.")
		End If

		Using session As Session = XpoHelper.GetNewSession()
			Dim role As XpoRole = session.FindObject(Of XpoRole)(New GroupOperator(GroupOperatorType.And, New BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal), New BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal)))

			Return (role IsNot Nothing)
		End Using
	End Function
End Class