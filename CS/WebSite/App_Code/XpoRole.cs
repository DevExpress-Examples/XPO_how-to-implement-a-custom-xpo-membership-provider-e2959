using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;

public class XpoRole : XPObject {

    public XpoRole()
        : base() { }

    public XpoRole(Session session)
        : base(session) { }

    public override void AfterConstruction() {
        base.AfterConstruction();
    }

    private string _ApplicationName;
    [Size(255)]
    [Indexed("RoleName", Unique = true)]
    public string ApplicationName {
        get {
            return _ApplicationName;
        }
        set {
            SetPropertyValue("ApplicationName", ref _ApplicationName, value);
        }
    }

    private string _RoleName;
    [Size(255)]
    [Indexed("ApplicationName", Unique = true)]
    public string RoleName {
        get {
            return _RoleName;
        }
        set {
            SetPropertyValue("RoleName", ref _RoleName, value);
        }
    }

    private string _Description;
    [Size(255)]
    public string Description {
        get {
            return _Description;
        }
        set {
            SetPropertyValue("Description", ref _Description, value);
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

    [Association("XpoRoles-XpoUsers")]
    public XPCollection<XpoUser> Users {
        get {
            return GetCollection<XpoUser>("Users");
        }
    }
}

