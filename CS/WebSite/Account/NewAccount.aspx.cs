using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class NewAccount : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void CreateUser_CreateUserError(object sender, CreateUserErrorEventArgs e) {
        // Define the name and type of the client scripts on the page.
        String csErrorRowName = "errorRowScript";
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs = Page.ClientScript;

        // Check to see if the startup script is already registered.
        if (!cs.IsStartupScriptRegistered(cstype, csErrorRowName)) {
            cs.RegisterStartupScript(cstype, csErrorRowName, "document.getElementById(\"errorRow\").style.display=\"block\";", true);
        }
    }

    protected void CreateUser_ContinueButtonClick(object sender, EventArgs e) {
        FormsAuthentication.SetAuthCookie(CreateUser.UserName, false /* createPersistentCookie */);

        String continueUrl = CreateUser.ContinueDestinationPageUrl;
        if (String.IsNullOrEmpty(continueUrl)) {
            continueUrl = "~/";
        }
        Response.Redirect(continueUrl);
    }
}