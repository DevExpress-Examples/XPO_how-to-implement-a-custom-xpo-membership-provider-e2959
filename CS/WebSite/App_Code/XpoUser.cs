using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;

public class XpoUser : XPObject {

    public XpoUser()
        : base() { }

    public XpoUser(Session session)
        : base(session) { }

    public override void AfterConstruction() {
        base.AfterConstruction();
    }

    private String _ApplicationName;
    [Size(255)]
    [Indexed("UserName", Unique = true)]
    public String ApplicationName {
        get {
            return _ApplicationName;
        }
        set {
            SetPropertyValue("ApplicationName", ref _ApplicationName, value);
        }
    }

    private String _UserName;
    [Size(255)]
    [Indexed("ApplicationName", Unique = true)]
    public String UserName {
        get {
            return _UserName;
        }
        set {
            SetPropertyValue("UserName", ref _UserName, value);
        }
    }

    private String _Email;
    [Size(128)]
    public String Email {
        get {
            return _Email;
        }
        set {
            SetPropertyValue("Email", ref _Email, value);
        }
    }

    private String _Comment;
    [Size(SizeAttribute.Unlimited)]
    public String Comment {
        get {
            return _Comment;
        }
        set {
            SetPropertyValue("Comment", ref _Comment, value);
        }
    }

    private String _Password;
    [Size(128)]
    public String Password {
        get {
            return _Password;
        }
        set {
            SetPropertyValue("Password", ref _Password, value);
        }
    }

    private String _PasswordQuestion;
    [Size(255)]
    public String PasswordQuestion {
        get {
            return _PasswordQuestion;
        }
        set {
            SetPropertyValue("PasswordQuestion", ref _PasswordQuestion, value);
        }
    }

    private String _PasswordAnswer;
    [Size(255)]
    public String PasswordAnswer {
        get {
            return _PasswordAnswer;
        }
        set {
            SetPropertyValue("PasswordAnswer", ref _PasswordAnswer, value);
        }
    }

    private Boolean _IsApproved;
    public Boolean IsApproved {
        get {
            return _IsApproved;
        }
        set {
            SetPropertyValue("IsApproved", ref _IsApproved, value);
        }
    }

    private DateTime _LastActivityDate;
    public DateTime LastActivityDate {
        get {
            return _LastActivityDate;
        }
        set {
            SetPropertyValue("LastActivityDate", ref _LastActivityDate, value);
        }
    }

    private DateTime _LastLoginDate;
    public DateTime LastLoginDate {
        get {
            return _LastLoginDate;
        }
        set {
            SetPropertyValue("LastLoginDate", ref _LastLoginDate, value);
        }
    }

    private DateTime _LastPasswordChangedDate;
    public DateTime LastPasswordChangedDate {
        get {
            return _LastPasswordChangedDate;
        }
        set {
            SetPropertyValue("LastPasswordChangedDate", ref _LastPasswordChangedDate, value);
        }
    }

    private DateTime _CreationDate;
    public DateTime CreationDate {
        get {
            return _CreationDate;
        }
        set {
            SetPropertyValue("CreationDate", ref _CreationDate, value);
        }
    }

    public Boolean IsOnline {
        get {
            TimeSpan span = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime time = DateTime.UtcNow.Subtract(span);
            return (this.LastActivityDate.ToUniversalTime() > time);
        }
    }

    private Boolean _IsLockedOut;
    public Boolean IsLockedOut {
        get {
            return _IsLockedOut;
        }
        set {
            SetPropertyValue("IsLockedOut", ref _IsLockedOut, value);
        }
    }

    private DateTime _LastLockedOutDate;
    public DateTime LastLockedOutDate {
        get {
            return _LastLockedOutDate;
        }
        set {
            SetPropertyValue("LastLockedOutDate", ref _LastLockedOutDate, value);
        }
    }

    private int _FailedPasswordAttemptCount;
    public int FailedPasswordAttemptCount {
        get {
            return _FailedPasswordAttemptCount;
        }
        set {
            SetPropertyValue("FailedPasswordAttemptCount", ref _FailedPasswordAttemptCount, value);
        }
    }

    private DateTime _FailedPasswordAttemptWindowStart;
    public DateTime FailedPasswordAttemptWindowStart {
        get {
            return _FailedPasswordAttemptWindowStart;
        }
        set {
            SetPropertyValue("FailedPasswordAttemptWindowStart", ref _FailedPasswordAttemptWindowStart, value);
        }
    }

    private int _FailedPasswordAnswerAttemptCount;
    public int FailedPasswordAnswerAttemptCount {
        get {
            return _FailedPasswordAnswerAttemptCount;
        }
        set {
            SetPropertyValue("FailedPasswordAnswerAttemptCount", ref _FailedPasswordAnswerAttemptCount, value);
        }
    }

    private DateTime _FailedPasswordAnswerAttemptWindowStart;
    public DateTime FailedPasswordAnswerAttemptWindowStart {
        get {
            return _FailedPasswordAnswerAttemptWindowStart;
        }
        set {
            SetPropertyValue("FailedPasswordAnswerAttemptWindowStart", ref _FailedPasswordAnswerAttemptWindowStart, value);
        }
    }

    [Association("XpoRoles-XpoUsers")]
    public XPCollection<XpoRole> Roles {
        get {
            return GetCollection<XpoRole>("Roles");
        }
    }
}

