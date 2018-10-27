using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;

public partial class Site : System.Web.UI.MasterPage {
    protected void Page_Load(object sender, EventArgs e) {
        lbOnline.Text = Membership.GetNumberOfUsersOnline().ToString();
    }

    protected void lbLoginName_Load(object sender, EventArgs e) {
        ASPxLabel lbLoginName = sender as ASPxLabel;
        String userName = Page.User.Identity.Name;

        if (!String.IsNullOrEmpty(userName))
            lbLoginName.Text = userName;
    }

    protected void btnLogout_Click(object sender, EventArgs e) {
        FormsAuthentication.SignOut();
        Response.Redirect(Page.Request.RawUrl);
    }
    protected void tvSiteMap_DataBound(object sender, EventArgs e) {
        ASPxTreeView tvSiteMap = sender as ASPxTreeView;

        if (!IsPostBack)
            tvSiteMap.ExpandAll();
    }
}
