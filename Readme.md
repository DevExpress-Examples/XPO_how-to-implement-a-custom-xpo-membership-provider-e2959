<!-- default file list -->
*Files to look at*:

* [ChangePassword.aspx](./CS/WebSite/Account/ChangePassword.aspx) (VB: [ChangePassword.aspx.vb](./VB/WebSite/Account/ChangePassword.aspx.vb))
* [ChangePassword.aspx.cs](./CS/WebSite/Account/ChangePassword.aspx.cs) (VB: [ChangePassword.aspx.vb](./VB/WebSite/Account/ChangePassword.aspx.vb))
* [Login.aspx](./CS/WebSite/Account/Login.aspx) (VB: [Login.aspx.vb](./VB/WebSite/Account/Login.aspx.vb))
* [Login.aspx.cs](./CS/WebSite/Account/Login.aspx.cs) (VB: [Login.aspx.vb](./VB/WebSite/Account/Login.aspx.vb))
* [NewAccount.aspx](./CS/WebSite/Account/NewAccount.aspx) (VB: [NewAccount.aspx.vb](./VB/WebSite/Account/NewAccount.aspx.vb))
* [NewAccount.aspx.cs](./CS/WebSite/Account/NewAccount.aspx.cs) (VB: [NewAccount.aspx.vb](./VB/WebSite/Account/NewAccount.aspx.vb))
* [RetrievePassword.aspx](./CS/WebSite/Account/RetrievePassword.aspx) (VB: [RetrievePassword.aspx.vb](./VB/WebSite/Account/RetrievePassword.aspx.vb))
* [RetrievePassword.aspx.cs](./CS/WebSite/Account/RetrievePassword.aspx.cs) (VB: [RetrievePassword.aspx.vb](./VB/WebSite/Account/RetrievePassword.aspx.vb))
* [XpoHelper.cs](./CS/WebSite/App_Code/XpoHelper.cs) (VB: [XpoHelper.vb](./VB/WebSite/App_Code/XpoHelper.vb))
* [XpoMembershipProvider.cs](./CS/WebSite/App_Code/XpoMembershipProvider.cs) (VB: [XpoMembershipProvider.vb](./VB/WebSite/App_Code/XpoMembershipProvider.vb))
* [XpoRole.cs](./CS/WebSite/App_Code/XpoRole.cs) (VB: [XpoRole.vb](./VB/WebSite/App_Code/XpoRole.vb))
* [XpoRoleProvider.cs](./CS/WebSite/App_Code/XpoRoleProvider.cs) (VB: [XpoRoleProvider.vb](./VB/WebSite/App_Code/XpoRoleProvider.vb))
* [XpoUser.cs](./CS/WebSite/App_Code/XpoUser.cs) (VB: [XpoUser.vb](./VB/WebSite/App_Code/XpoUser.vb))
* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Site.master.cs](./CS/WebSite/Site.master.cs) (VB: [Site.master.vb](./VB/WebSite/Site.master.vb))
* [UserList.aspx](./CS/WebSite/UserList.aspx) (VB: [UserList.aspx.vb](./VB/WebSite/UserList.aspx.vb))
* [UserList.aspx.cs](./CS/WebSite/UserList.aspx.cs) (VB: [UserList.aspx.vb](./VB/WebSite/UserList.aspx.vb))
<!-- default file list end -->
# How to implement a custom XPO Membership Provider


<p>The example is based on a couple of articles regarding custom Membership and Roles provider implementation using the XPO framework. The XPO framework has a great advantage, allowing you to store objects in different databases. The example demonstrates some basic implementation of a custom membership provider. There are some known issues with the example, and we plan to improve it in future.</p><p>Each version of the example will have some description about its main features, or known limitations.</p><p><strong>See also:</strong><br />
<a href="http://community.devexpress.com/forums/p/87983/310475.aspx"><u>Forum: XpoMembershipProvider Implemented</u></a><br />
<a href="http://msdn.microsoft.com/en-us/library/yh26yfzy.aspx"><u>MSDN: Introduction to Membership</u></a><br />
<a href="http://msdn.microsoft.com/en-us/library/44w5aswa.aspx"><u>MSDN: Sample Membership Provider Implementation</u></a></p>

<br/>


