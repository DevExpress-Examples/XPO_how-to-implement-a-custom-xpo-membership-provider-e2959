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


