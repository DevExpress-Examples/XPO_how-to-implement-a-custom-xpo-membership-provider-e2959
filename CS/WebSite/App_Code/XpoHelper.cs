using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

/// <summary>
/// Summary description for XpoHelper
/// </summary>
public static class XpoHelper {
    static XpoHelper() {
        CreateDefaultObjects();
    }

    public static Session GetNewSession() {
        return new Session(DataLayer);
    }

    public static UnitOfWork GetNewUnitOfWork() {
        return new UnitOfWork(DataLayer);
    }

    private readonly static object lockObject = new object();

    static IDataLayer fDataLayer;
    static IDataLayer DataLayer {
        get {
            if (fDataLayer == null) {
                lock (lockObject) {
                    fDataLayer = GetDataLayer();
                }
            }
            return fDataLayer;
        }
    }

    private static IDataLayer GetDataLayer() {
        XpoDefault.Session = null;

        InMemoryDataStore ds = new InMemoryDataStore();
        //string connectionString = AccessConnectionProvider.GetConnectionString(@"...\XPO_Membership\App_Data\my.mdb");
        //IDataStore ds = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema);

        XPDictionary dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(typeof(XpoUser).Assembly);

        return new ThreadSafeDataLayer(dict, ds);
    }

    static void CreateDefaultObjects() {
        MembershipCreateStatus status;
        Membership.CreateUser("test", "test", "just@ask.me", "The answer is \"test\"", "test", true, out status);
        Membership.CreateUser("admin", "admin", "admin@ask.me", "The answer is \"admin\"", "admin", true, out status);

        //for (Int32 i = 0; i < 300; i++) {
        //    Membership.CreateUser(String.Format("test{0}", i), "test", String.Format("just{0}@ask.me", i), "The answer is \"test\"", "test", true, out status);
        //}
    }
}