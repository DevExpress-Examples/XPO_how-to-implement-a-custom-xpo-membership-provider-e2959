<!-- default file list -->
*Files to look at*:

* [ChangePassword.aspx](./CS/WebSite/Account/ChangePassword.aspx) (VB: [ChangePassword.aspx](./VB/WebSite/Account/ChangePassword.aspx))
* [ChangePassword.aspx.cs](./CS/WebSite/Account/ChangePassword.aspx.cs) (VB: [ChangePassword.aspx.vb](./VB/WebSite/Account/ChangePassword.aspx.vb))
* [Login.aspx](./CS/WebSite/Account/Login.aspx) (VB: [Login.aspx](./VB/WebSite/Account/Login.aspx))
* [Login.aspx.cs](./CS/WebSite/Account/Login.aspx.cs) (VB: [Login.aspx.vb](./VB/WebSite/Account/Login.aspx.vb))
* [NewAccount.aspx](./CS/WebSite/Account/NewAccount.aspx) (VB: [NewAccount.aspx](./VB/WebSite/Account/NewAccount.aspx))
* [NewAccount.aspx.cs](./CS/WebSite/Account/NewAccount.aspx.cs) (VB: [NewAccount.aspx.vb](./VB/WebSite/Account/NewAccount.aspx.vb))
* [RetrievePassword.aspx](./CS/WebSite/Account/RetrievePassword.aspx) (VB: [RetrievePassword.aspx](./VB/WebSite/Account/RetrievePassword.aspx))
* [RetrievePassword.aspx.cs](./CS/WebSite/Account/RetrievePassword.aspx.cs) (VB: [RetrievePassword.aspx.vb](./VB/WebSite/Account/RetrievePassword.aspx.vb))
* [XpoHelper.cs](./CS/WebSite/App_Code/XpoHelper.cs) (VB: [XpoHelper.vb](./VB/WebSite/App_Code/XpoHelper.vb))
* [XpoMembershipProvider.cs](./CS/WebSite/App_Code/XpoMembershipProvider.cs) (VB: [XpoMembershipProvider.vb](./VB/WebSite/App_Code/XpoMembershipProvider.vb))
* [XpoRole.cs](./CS/WebSite/App_Code/XpoRole.cs) (VB: [XpoRoleProvider.vb](./VB/WebSite/App_Code/XpoRoleProvider.vb))
* [XpoRoleProvider.cs](./CS/WebSite/App_Code/XpoRoleProvider.cs) (VB: [XpoRoleProvider.vb](./VB/WebSite/App_Code/XpoRoleProvider.vb))
* [XpoUser.cs](./CS/WebSite/App_Code/XpoUser.cs) (VB: [XpoUser.vb](./VB/WebSite/App_Code/XpoUser.vb))
* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
* [Site.master.cs](./CS/WebSite/Site.master.cs) (VB: [Site.master.vb](./VB/WebSite/Site.master.vb))
* [UserList.aspx](./CS/WebSite/UserList.aspx) (VB: [UserList.aspx](./VB/WebSite/UserList.aspx))
* [UserList.aspx.cs](./CS/WebSite/UserList.aspx.cs) (VB: [UserList.aspx.vb](./VB/WebSite/UserList.aspx.vb))
<!-- default file list end -->
# How to implement a custom XPO Membership Provider


<p>The example is based on a couple of articles regarding custom Membership and Roles provider implementation using the XPO framework. The XPO framework has a great advantage, allowing you to store objects in different databases. The example demonstrates some basic implementation of a custom membership provider. There are some known issues with the example, and we plan to improve it in future.</p><p>Each version of the example will have some description about its main features, or known limitations.</p><p><strong>See also:</strong><br />
<a href="http://community.devexpress.com/forums/p/87983/310475.aspx"><u>Forum: XpoMembershipProvider Implemented</u></a><br />
<a href="http://msdn.microsoft.com/en-us/library/yh26yfzy.aspx"><u>MSDN: Introduction to Membership</u></a><br />
<a href="http://msdn.microsoft.com/en-us/library/44w5aswa.aspx"><u>MSDN: Sample Membership Provider Implementation</u></a></p>


<h3>Description</h3>

<p><strong>Known features:<br />
</strong>Almost all functionality regarding Users and Roles is implemented. The <strong>Web Site Administration Tool</strong> should deal with users and roles correctly.</p><p><strong>Known limitations:</strong><br />
Following methods of the <strong>XpoMembershipProvider </strong>class are not implemented:<br />
<i>ChangePasswordQuestionAndAnswer<br />
ResetPassword<br />
UnlockUser</i></p><p>Exceptions logging code was cut removed, and if you want to log unsuccessful login attempts, you should use the code from the Forum link in the &quot;See also&quot; section.</p><p>It seems that because of a peculiarity regarding the <strong>Web Site Administration Tool</strong>, it is not recommended to use the InMemoryDataSource, because each time the tool creates the user, all changes are lost.</p><p>Roles are implemented. However, the example does not demonstrate them in action. For example, the <strong>Security Trimming</strong> option is not enabled in the demo.</p>

<br/>


