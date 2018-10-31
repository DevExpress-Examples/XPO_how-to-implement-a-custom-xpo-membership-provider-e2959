using System;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration.Provider;
using System.Configuration;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Summary description for XpoMembershipProvider
/// </summary>
public sealed class XpoMembershipProvider : MembershipProvider {
    private enum FailureType {
        Password = 1,
        PasswordAnswer = 2
    }

    //
    // Global connection String.
    //

    private String connectionString;

    //
    // Used when determining encryption key values.
    //

    private MachineKeySection machineKey;

    //
    // If false, exceptions are thrown to the caller. If true,
    // exceptions are written to the event log.
    //

    public override void Initialize(String name, System.Collections.Specialized.NameValueCollection config) {
        //
        // Initialize values from web.config.
        //

        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "XpoMembershipProvider";

        if (String.IsNullOrEmpty(config["description"])) {
            config.Remove("description");
            config.Add("description", "XPO Membership provider");
        }

        // Initialize the abstract base class.
        base.Initialize(name, config);

        pApplicationName = GetConfigValue(config["applicationName"],
                                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
        pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
        pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
        pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
        pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
        pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
        pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
        pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
        pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

        String temp_format = config["passwordFormat"];
        if (temp_format == null) {
            temp_format = "Hashed";
        }

        switch (temp_format) {
            case "Hashed":
                pPasswordFormat = MembershipPasswordFormat.Hashed;
                break;
            case "Encrypted":
                pPasswordFormat = MembershipPasswordFormat.Encrypted;
                break;
            case "Clear":
                pPasswordFormat = MembershipPasswordFormat.Clear;
                break;
            default:
                throw new ProviderException("Password format not supported.");
        }

        //
        // Initialize XPO Connection string.
        //

        ConnectionStringSettings ConnectionStringSettings =
          ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

        if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "") {
            throw new ProviderException("Connection String cannot be blank.");
        }

        connectionString = ConnectionStringSettings.ConnectionString;


        // Get encryption and decryption key information from the configuration.
        Configuration cfg =
          WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

        if (machineKey.ValidationKey.Contains("AutoGenerate"))
            if (PasswordFormat != MembershipPasswordFormat.Clear)
                throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");

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
    // System.Web.Security.MembershipProvider properties.
    //

    private String pApplicationName;
    private Boolean pEnablePasswordReset;
    private Boolean pEnablePasswordRetrieval;
    private Boolean pRequiresQuestionAndAnswer;
    private Boolean pRequiresUniqueEmail;
    private int pMaxInvalidPasswordAttempts;
    private int pPasswordAttemptWindow;
    private MembershipPasswordFormat pPasswordFormat;

    public override String ApplicationName {
        get { return pApplicationName; }
        set { pApplicationName = value; }
    }

    public override Boolean EnablePasswordReset {
        get { return pEnablePasswordReset; }
    }

    public override Boolean EnablePasswordRetrieval {
        get { return pEnablePasswordRetrieval; }
    }

    public override Boolean RequiresQuestionAndAnswer {
        get { return pRequiresQuestionAndAnswer; }
    }

    public override Boolean RequiresUniqueEmail {
        get { return pRequiresUniqueEmail; }
    }

    public override int MaxInvalidPasswordAttempts {
        get { return pMaxInvalidPasswordAttempts; }
    }

    public override int PasswordAttemptWindow {
        get { return pPasswordAttemptWindow; }
    }

    public override MembershipPasswordFormat PasswordFormat {
        get { return pPasswordFormat; }
    }

    private int pMinRequiredNonAlphanumericCharacters;

    public override int MinRequiredNonAlphanumericCharacters {
        get { return pMinRequiredNonAlphanumericCharacters; }
    }

    private int pMinRequiredPasswordLength;

    public override int MinRequiredPasswordLength {
        get { return pMinRequiredPasswordLength; }
    }

    private String pPasswordStrengthRegularExpression;

    public override String PasswordStrengthRegularExpression {
        get { return pPasswordStrengthRegularExpression; }
    }


    public override Boolean ChangePassword(String username, String oldPassword, String newPassword) {
        ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);

        OnValidatingPassword(args);

        if (args.Cancel) {
            if (args.FailureInformation != null) {
                throw args.FailureInformation;
            }
            else {
                throw new Exception("Change password canceled due to new password validation failure.");
            }
        }

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new BinaryOperator("UserName", username, BinaryOperatorType.Equal)));
            if (user != null) {
                user.Password = EncodePassword(newPassword);
                user.LastPasswordChangedDate = DateTime.Now;
            }
            else {
                return false;
            }
            user.Save();
        }

        return true;
    }

    public override Boolean ChangePasswordQuestionAndAnswer(String username, String password, String newPasswordQuestion, String newPasswordAnswer) {
        throw new NotImplementedException();
    }

    public override MembershipUser CreateUser(String username, String password, String email, String passwordQuestion, String passwordAnswer, Boolean isApproved, object providerUserKey, out MembershipCreateStatus status) {
        ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

        OnValidatingPassword(args);

        if (args.Cancel) {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }

        if (RequiresQuestionAndAnswer && String.IsNullOrEmpty(passwordAnswer)) {
            status = MembershipCreateStatus.InvalidAnswer;
            return null;
        }

        if (RequiresUniqueEmail) {
            if (!IsEmail(email)) {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }
            if (!String.IsNullOrEmpty(GetUserNameByEmail(email))) {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
        }

        MembershipUser mUser = GetUser(username, false);

        if (mUser != null) {
            status = MembershipCreateStatus.DuplicateUserName;
            return null;
        }

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = new XpoUser(session) {
                ApplicationName = ApplicationName,
                UserName = username,
                Password = EncodePassword(password),
                Email = email,
                PasswordQuestion = passwordQuestion,
                PasswordAnswer = EncodePassword(passwordAnswer),
                IsApproved = isApproved,
                CreationDate = DateTime.Now,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = DateTime.MinValue,
                IsLockedOut = false,
                LastActivityDate = DateTime.Now,
                LastLockedOutDate = DateTime.MinValue,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = DateTime.MinValue
            };

            user.Save();
            status = MembershipCreateStatus.Success;
        }

        return GetUser(username, false);
    }

    /* Used by Web Site Administration Tool */

    public override Boolean DeleteUser(String username, Boolean deleteAllRelatedData) {
        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new BinaryOperator("UserName", username, BinaryOperatorType.Equal)));

            if (user == null)
                return false;

            user.Delete();
            user.Save();
        }
        return true;
    }

    /* Used by Web Site Administration Tool */

    public override MembershipUserCollection FindUsersByEmail(String emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
        MembershipUserCollection mclUsers = new MembershipUserCollection();

        using (Session session = XpoHelper.GetNewSession()) {
            CriteriaOperator theCriteria = CriteriaOperator.Parse("ApplicationName = ? and contains(Email, ?)", ApplicationName, emailToMatch);
            XPCollection<XpoUser> xpcUsers = new XPCollection<XpoUser>(session, theCriteria,
                    new SortProperty("UserName", DevExpress.Xpo.DB.SortingDirection.Ascending));

            totalRecords = Convert.ToInt32(session.Evaluate<XpoUser>(CriteriaOperator.Parse("Count()"), theCriteria));

            xpcUsers.SkipReturnedObjects = pageIndex * pageSize;
            xpcUsers.TopReturnedObjects = pageSize;

            foreach (XpoUser xpoUser in xpcUsers) {
                MembershipUser mUser = GetUserFromXpoUser(xpoUser);
                mclUsers.Add(mUser);
            }
        }

        return mclUsers;
    }

    /* Used by Web Site Administration Tool */

    public override MembershipUserCollection FindUsersByName(String usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
        MembershipUserCollection mclUsers = new MembershipUserCollection();

        using (Session session = XpoHelper.GetNewSession()) {
            CriteriaOperator theCriteria = CriteriaOperator.Parse("ApplicationName = ? and contains(UserName, ?)", ApplicationName, usernameToMatch);
            XPCollection<XpoUser> xpcUsers = new XPCollection<XpoUser>(session, theCriteria,
                    new SortProperty("UserName", DevExpress.Xpo.DB.SortingDirection.Ascending));

            totalRecords = Convert.ToInt32(session.Evaluate<XpoUser>(CriteriaOperator.Parse("Count()"), theCriteria));

            xpcUsers.SkipReturnedObjects = pageIndex * pageSize;
            xpcUsers.TopReturnedObjects = pageSize;

            foreach (XpoUser xpoUser in xpcUsers) {
                MembershipUser mUser = GetUserFromXpoUser(xpoUser);
                mclUsers.Add(mUser);
            }
        }

        return mclUsers;
    }

    /* Used by Web Site Administration Tool */

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out Int32 totalRecords) {
        MembershipUserCollection mclUsers = new MembershipUserCollection();

        using (Session session = XpoHelper.GetNewSession()) {
            XPCollection<XpoUser> xpcUsers = new XPCollection<XpoUser>(session,
                    new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                    new SortProperty("UserName", DevExpress.Xpo.DB.SortingDirection.Ascending));

            totalRecords = Convert.ToInt32(session.Evaluate<XpoUser>(CriteriaOperator.Parse("Count()"), null));

            xpcUsers.SkipReturnedObjects = pageIndex * pageSize;
            xpcUsers.TopReturnedObjects = pageSize;

            foreach (XpoUser xpoUser in xpcUsers) {
                MembershipUser mUser = GetUserFromXpoUser(xpoUser);
                mclUsers.Add(mUser);
            }
        }

        return mclUsers;
    }

    public override int GetNumberOfUsersOnline() {
        using (Session session = XpoHelper.GetNewSession()) {
            XPCollection<XpoUser> xpcUsers = new XPCollection<XpoUser>(session,
                new GroupOperator(GroupOperatorType.And,
                 new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                 new BinaryOperator("IsOnline", true, BinaryOperatorType.Equal)));

            return xpcUsers.Count;
        }
    }

    public override String GetPassword(String username, String answer) {
        if (!EnablePasswordRetrieval) {
            throw new ProviderException("Password Retrieval Not Enabled.");
        }

        if (PasswordFormat == MembershipPasswordFormat.Hashed) {
            throw new ProviderException("Cannot retrieve Hashed passwords.");
        }

        String password;
        String passwordAnswer;

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new BinaryOperator("UserName", username, BinaryOperatorType.Equal)));

            if (user == null) {
                throw new MembershipPasswordException("The specified user is not found.");
            }
            if (user.IsLockedOut) {
                throw new MembershipPasswordException("The specified user is locked out.");
            }

            password = user.Password;
            passwordAnswer = user.PasswordAnswer;
        }

        if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer)) {
            UpdateFailureCount(username, FailureType.PasswordAnswer);

            throw new MembershipPasswordException("Incorrect password answer.");
        }

        if (PasswordFormat == MembershipPasswordFormat.Encrypted) {
            password = DecodePassword(password);
        }

        return password;
    }

    public override MembershipUser GetUser(String username, Boolean userIsOnline) {
        MembershipUser mUser;

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                        GroupOperatorType.And,
                        new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                        new BinaryOperator("UserName", username, BinaryOperatorType.Equal)));

            if (user == null)
                return null;

            mUser = GetUserFromXpoUser(user);

            if (userIsOnline)
                user.LastActivityDate = DateTime.Now;

            user.Save();
        }

        return mUser;
    }

    public override MembershipUser GetUser(object providerUserKey, Boolean userIsOnline) {
        MembershipUser mUser;

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                        GroupOperatorType.And,
                        new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                        new BinaryOperator("Oid", providerUserKey, BinaryOperatorType.Equal)));

            if (user == null)
                return null;

            mUser = GetUserFromXpoUser(user);

            if (userIsOnline)
                user.LastActivityDate = DateTime.Now;

            user.Save();
        }

        return mUser;
    }

    public override String GetUserNameByEmail(String email) {
        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                        GroupOperatorType.And,
                        new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                        new BinaryOperator("Email", email, BinaryOperatorType.Equal)));

            if (user == null)
                return String.Empty;

            return user.UserName;
        }
    }

    public override String ResetPassword(String username, String answer) {
        throw new NotImplementedException("ResetPassword");
    }

    public override Boolean UnlockUser(String userName) {
        throw new NotImplementedException("UnlockUser");
    }

    /* Used by Web Site Administration Tool */

    public override void UpdateUser(MembershipUser mUser) {
        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser xpoUser = session.FindObject<XpoUser>(new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new BinaryOperator("UserName", mUser.UserName, BinaryOperatorType.Equal)));

            if (xpoUser == null) {
                throw new ProviderException("The specified user is not found.");
            }

            xpoUser.Email = mUser.Email;
            xpoUser.Comment = mUser.Comment;
            xpoUser.IsApproved = mUser.IsApproved;
            xpoUser.LastLoginDate = mUser.LastLoginDate;
            xpoUser.LastActivityDate = mUser.LastActivityDate;

            xpoUser.Save();
        }
    }

    public override Boolean ValidateUser(String username, String password) {
        Boolean isValid = false;

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                        GroupOperatorType.And,
                        new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                        new BinaryOperator("UserName", username, BinaryOperatorType.Equal)));

            if (user == null)
                return false;

            // http://msdn.microsoft.com/en-us/library/system.web.security.membershipuser.lastactivitydate.aspx

            if (CheckPassword(password, user.Password)) {
                if ((!user.IsLockedOut) && (user.IsApproved)) {
                    isValid = true;
                    user.LastLoginDate = DateTime.Now;
                    user.LastActivityDate = DateTime.Now;

                    user.Save();
                }
            }
        }

        return isValid;
    }

    private void UpdateFailureCount(String username, FailureType failureType) {
        DateTime windowStart;
        DateTime windowEnd;
        int failureCount;

        using (Session session = XpoHelper.GetNewSession()) {
            XpoUser user = session.FindObject<XpoUser>(new GroupOperator(
                GroupOperatorType.And,
                new BinaryOperator("ApplicationName", ApplicationName, BinaryOperatorType.Equal),
                new BinaryOperator("UserName", username, BinaryOperatorType.Equal)));

            switch (failureType) {
                case FailureType.Password:
                    failureCount = user.FailedPasswordAttemptCount;
                    windowStart = user.FailedPasswordAttemptWindowStart;
                    windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                    user.FailedPasswordAttemptWindowStart = DateTime.Now;

                    if (DateTime.Now > windowEnd) {
                        user.FailedPasswordAttemptCount = 1;
                    }
                    else {
                        user.FailedPasswordAttemptCount++;
                    }

                    if (user.FailedPasswordAttemptCount >= MaxInvalidPasswordAttempts) {
                        if (!user.IsLockedOut) {
                            user.LastLockedOutDate = DateTime.Now;
                            user.IsLockedOut = true;
                        }
                    }
                    break;

                case FailureType.PasswordAnswer:
                    failureCount = user.FailedPasswordAnswerAttemptCount;
                    windowStart = user.FailedPasswordAnswerAttemptWindowStart;
                    windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                    user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;

                    if (DateTime.Now > windowEnd) {
                        user.FailedPasswordAnswerAttemptCount = 1;
                    }
                    else {
                        user.FailedPasswordAnswerAttemptCount++;
                    }

                    if (user.FailedPasswordAnswerAttemptCount >= MaxInvalidPasswordAttempts) {
                        if (!user.IsLockedOut) {
                            user.LastLockedOutDate = DateTime.Now;
                            user.IsLockedOut = true;
                        }
                    }
                    break;
            }
            user.Save();
        }

    }

    private Boolean CheckPassword(String password, String dbpassword) {
        String pass1 = password;
        String pass2 = dbpassword;

        switch (PasswordFormat) {
            case MembershipPasswordFormat.Clear:
                break;
            case MembershipPasswordFormat.Encrypted:
                pass2 = DecodePassword(dbpassword);
                break;
            case MembershipPasswordFormat.Hashed:
                pass1 = EncodePassword(password);
                break;
            default:
                break;
        }

        return pass1 == pass2;
    }

    private String DecodePassword(String encodedPassword) {
        String password = encodedPassword;

        if (String.IsNullOrEmpty(password)) {
            return password;
        }

        switch (PasswordFormat) {
            case MembershipPasswordFormat.Clear:
                break;
            case MembershipPasswordFormat.Encrypted:
                password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                break;
            case MembershipPasswordFormat.Hashed:
                throw new ProviderException("Cannot decode a hashed password.");
            default:
                throw new ProviderException("Unsupported password format.");
        }

        return password;
    }

    private String EncodePassword(String password) {
        String encodedPassword = password;

        if (String.IsNullOrEmpty(encodedPassword))
            return encodedPassword;

        switch (PasswordFormat) {
            case MembershipPasswordFormat.Clear:
                break;
            case MembershipPasswordFormat.Encrypted:
                encodedPassword = Convert.ToBase64String(this.EncryptPassword(Encoding.Unicode.GetBytes(password)));
                break;
            case MembershipPasswordFormat.Hashed:
                HMACSHA512 hash = new HMACSHA512(HexToByte(machineKey.ValidationKey));
                encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                break;
            default:
                throw new ProviderException("Unsupported password format.");
        }

        return encodedPassword;
    }

    private static Boolean IsEmail(String inputEmail) {
        String strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        Regex re = new Regex(strRegex);
        if (re.IsMatch(inputEmail))
            return (true);
        else
            return (false);
    }

    private static byte[] HexToByte(String hexString) {
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        return returnBytes;
    }

    private MembershipUser GetUserFromXpoUser(XpoUser xUser) {
        MembershipUser mUser = new MembershipUser(
            this.Name,
            xUser.UserName,
            xUser.Oid,
            xUser.Email,
            xUser.PasswordQuestion,
            xUser.Comment,
            xUser.IsApproved,
            xUser.IsLockedOut,
            xUser.CreationDate,
            xUser.LastLoginDate,
            xUser.LastActivityDate,
            xUser.LastPasswordChangedDate,
            xUser.LastLockedOutDate
            );
        return mUser;
    }
}