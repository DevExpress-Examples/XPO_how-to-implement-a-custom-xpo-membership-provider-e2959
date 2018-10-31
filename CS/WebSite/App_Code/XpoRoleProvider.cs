using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

/// <summary>
/// Summary description for XpoRoleProvider
/// </summary>
public sealed class XpoRoleProvider : RoleProvider {

    //
    // Global connection String.
    //

    private String connectionString;

    public override void Initialize(String name, System.Collections.Specialized.NameValueCollection config) {
        //
        // Initialize values from web.config.
        //

        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "XpoRoleProvider";

        if (String.IsNullOrEmpty(config["description"])) {
            config.Remove("description");
            config.Add("description", "XPO Role provider");
        }

        base.Initialize(name, config);

        pApplicationName = GetConfigValue(config["applicationName"],
                                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);

        //
        // Initialize XPO Connection string.
        //

        ConnectionStringSettings ConnectionStringSettings =
          ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

        if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "") {
            throw new ProviderException("Connection String cannot be blank.");
        }

        connectionString = ConnectionStringSettings.ConnectionString;
    }

    //
    // A helper function to retrieve config values from the configuration file.
    //

    private String GetConfigValue(String configValue, String defaultValue) {
        if (String.IsNullOrEmpty(configValue))
            return defaultValue;

        return configValue;
    }

    //
    // System.Web.Security.RoleProvider properties.
    //

    private String pApplicationName;

    public override String ApplicationName {
        get { return pApplicationName; }
        set { pApplicationName = value; }
    }

    public override void AddUsersToRoles(String[] usernames, String[] roleNames) {
        foreach (string rolename in roleNames) {
            if (!RoleExists(rolename))
                throw new ProviderException("Role name not found.");
        }

        foreach (string username in usernames) {
            if (username.Contains(","))
                throw new ArgumentException("User names cannot contain commas.");

            foreach (string rolename in roleNames) {
                if (IsUserInRole(username, rolename))
                    throw new ProviderException("User is already in role.");
            }
        }

        using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork()) {
            XPCollection<XpoUser> xpcUsers = new XPCollection<XpoUser>(uow, new GroupOperator(
                        GroupOperatorType.And,
                        new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                        new InOperator("UserName", usernames)));
            XPCollection<XpoRole> xpcRoles = new XPCollection<XpoRole>(uow, new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new InOperator("RoleName", roleNames)));

            foreach (XpoUser user in xpcUsers)
                user.Roles.AddRange(xpcRoles);

            uow.CommitChanges();
        }
    }

    public override void CreateRole(String roleName) {
        CreateRole(roleName, String.Empty);
    }

    public void CreateRole(String roleName, String description) {
        if (String.IsNullOrEmpty(roleName))
            throw new ProviderException("Role name cannot be empty or null.");
        if (roleName.Contains(","))
            throw new ArgumentException("Role names cannot contain commas.");
        if (RoleExists(roleName))
            throw new ProviderException("Role name already exists.");
        if (roleName.Length > 256)
            throw new ProviderException("Role name cannot exceed 256 characters.");

        using (Session session = XpoHelper.GetNewSession()) {
            XpoRole role = new XpoRole(session) { RoleName = roleName, ApplicationName = this.ApplicationName, Description = description };
            role.Save();
        }
    }

    public override Boolean DeleteRole(String roleName, Boolean throwOnPopulatedRole) {
        if (!RoleExists(roleName))
            throw new ProviderException("Role does not exist.");

        if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
            throw new ProviderException("Cannot delete a populated role.");

        using (Session session = XpoHelper.GetNewSession()) {
            XpoRole role = session.FindObject<XpoRole>(new GroupOperator(
                    GroupOperatorType.And,
                    new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                    new BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal)));
            role.Delete();
            role.Save();
        }

        return !RoleExists(roleName);
    }

    public override String[] FindUsersInRole(String roleName, String userNameToMatch) {
        String[] users;

        if (!RoleExists(roleName))
            throw new ProviderException("Role does not exist.");

        using (Session session = XpoHelper.GetNewSession()) {
            CriteriaOperator theCriteria = CriteriaOperator.Parse("ApplicationName = ? and contains(UserName, ?) and Roles[RoleName = ?]", ApplicationName, userNameToMatch, roleName);
            XPView xpvUsers = new XPView(session, typeof(XpoUser), new CriteriaOperatorCollection() { OperandProperty.Parse("UserName") }, theCriteria);

            List<String> usersList = new List<String>();
            for (int i = 0; i < xpvUsers.Count; i++) {
                usersList.Add(xpvUsers[i][0].ToString());
            }
            users = usersList.ToArray();
        }

        return users;
    }

    public override String[] GetAllRoles() {
        String[] roles;

        using (Session session = XpoHelper.GetNewSession()) {
            XPView xpvRoles = new XPView(session, typeof(XpoRole),
                new CriteriaOperatorCollection() { OperandProperty.Parse("RoleName") },
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal));

            List<String> rolesList = new List<String>();
            for (int i = 0; i < xpvRoles.Count; i++) {
                rolesList.Add(xpvRoles[i][0].ToString());
            }
            roles = rolesList.ToArray();
        }

        return roles;
    }

    public override String[] GetRolesForUser(String username) {
        if (String.IsNullOrEmpty(username))
            throw new ProviderException("User name cannot be empty or null.");

        String[] roles;

        using (Session session = XpoHelper.GetNewSession()) {
            XPView xpvRoles = new XPView(session, typeof(XpoRole),
                new CriteriaOperatorCollection() { OperandProperty.Parse("RoleName") },
                new GroupOperator(
                    GroupOperatorType.And,
                    new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                    new ContainsOperator("Users", new BinaryOperator("UserName", username, BinaryOperatorType.Equal))));
            List<String> rolesList = new List<String>();
            for (int i = 0; i < xpvRoles.Count; i++) {
                rolesList.Add(xpvRoles[i][0].ToString());
            }
            roles = rolesList.ToArray();
        }

        return roles;
    }

    public override String[] GetUsersInRole(String roleName) {
        if (String.IsNullOrEmpty(roleName))
            throw new ProviderException("Role name cannot be empty or null.");
        if (!RoleExists(roleName))
            throw new ProviderException("Role does not exist.");

        string[] users;

        using (Session session = XpoHelper.GetNewSession()) {
            XPView xpvUsers = new XPView(session, typeof(XpoUser),
                new CriteriaOperatorCollection() { OperandProperty.Parse("UserName") },
                new GroupOperator(
                    GroupOperatorType.And,
                    new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                    new ContainsOperator("Roles", new BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal))));

            List<String> usersList = new List<String>();
            for (int i = 0; i < xpvUsers.Count; i++) {
                usersList.Add(xpvUsers[i][0].ToString());
            }
            users = usersList.ToArray();
        }

        return users;
    }

    public override Boolean IsUserInRole(String username, String roleName) {
        if (String.IsNullOrEmpty(username))
            throw new ProviderException("User name cannot be empty or null.");
        if (String.IsNullOrEmpty(roleName))
            throw new ProviderException("Role name cannot be empty or null.");

        using (Session session = XpoHelper.GetNewSession()) {
            XPCollection<XpoUser> collection = new XPCollection<XpoUser>(session,
                    new GroupOperator(GroupOperatorType.And, new CriteriaOperator[] {
                    new ContainsOperator("Roles", new BinaryOperator("RoleName", roleName)),
                    new BinaryOperator("UserName", username, BinaryOperatorType.Equal)})
                );

            return (collection.Count != 0);
        }
    }

    public override void RemoveUsersFromRoles(String[] usernames, String[] roleNames) {
        foreach (string rolename in roleNames) {
            if (String.IsNullOrEmpty(rolename))
                throw new ProviderException("Role name cannot be empty or null.");
            if (!RoleExists(rolename))
                throw new ProviderException("Role name not found.");
        }

        foreach (string username in usernames) {
            if (String.IsNullOrEmpty(username))
                throw new ProviderException("User name cannot be empty or null.");

            foreach (string rolename in roleNames) {
                if (!IsUserInRole(username, rolename))
                    throw new ProviderException("User is not in role.");
            }
        }

        using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork()) {
            XPCollection<XpoRole> xpcRoles = new XPCollection<XpoRole>(uow, new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new InOperator("RoleName", roleNames)));

            XPCollection<XpoUser> xpcUsers;
            foreach (XpoRole role in xpcRoles) {
                xpcUsers = new XPCollection<XpoUser>(uow, new GroupOperator(
                    GroupOperatorType.And,
                    new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                    new InOperator("UserName", usernames),
                    new ContainsOperator("Roles", new BinaryOperator("RoleName", role.RoleName, BinaryOperatorType.Equal))));
                for (int i = xpcUsers.Count - 1; i >= 0; i--)
                    role.Users.Remove(xpcUsers[i]);
            }
            uow.CommitChanges();
        }
    }

    public override Boolean RoleExists(String roleName) {
        if (String.IsNullOrEmpty(roleName))
            throw new ProviderException("Role name cannot be empty or null.");

        using (Session session = XpoHelper.GetNewSession()) {
            XpoRole role = session.FindObject<XpoRole>(new GroupOperator(
                        GroupOperatorType.And,
                        new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                        new BinaryOperator("RoleName", roleName, BinaryOperatorType.Equal)));

            return (role != null);
        }
    }
}