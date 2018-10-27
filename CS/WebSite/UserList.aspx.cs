using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;

public partial class Default2 : System.Web.UI.Page {
    Session session = XpoHelper.GetNewSession();

    readonly String[] RestrictedFields = { "Email", "Password", "PasswordQuestion", "PasswordAnswer" };

    protected void Page_Init(object sender, EventArgs e) {
        xpoUsers.Session = session;
    }
    protected void gvUsers_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e) {
        if (RestrictedFields.Contains(e.Column.FieldName))
            e.DisplayText = "********";
    }
    protected void gvUsers_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e) {
        if (RestrictedFields.Contains(e.DataColumn.FieldName))
            e.Cell.ForeColor = System.Drawing.Color.Red;
    }
}