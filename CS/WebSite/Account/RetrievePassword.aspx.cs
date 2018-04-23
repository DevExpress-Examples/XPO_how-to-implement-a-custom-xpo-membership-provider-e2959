using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_RetrievePassword : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void PasswordRecovery_UserLookupError(object sender, EventArgs e) {
        GenerateErrorRowScript("userNameErrorKey", "userNameErrorRow");
    }
    protected void PasswordRecovery_AnswerLookupError(object sender, EventArgs e) {
        GenerateErrorRowScript("answerErrorRowKey", "answerErrorRow");
    }

    private void GenerateErrorRowScript(String key, String errorRowId) {
        // Define the type of the client scripts on the page.
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs = Page.ClientScript;

        // Check to see if the startup script is already registered.
        if (!cs.IsStartupScriptRegistered(cstype, key)) {
            cs.RegisterStartupScript(cstype, key,
                String.Format("document.getElementById(\"{0}\").style.display=\"block\";", errorRowId),
                true);
        }
    }
    protected void PasswordRecovery_SendMailError(object sender, SendMailErrorEventArgs e) {
        // we do not send any emails
        e.Handled = true;
    }
}