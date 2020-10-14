<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v18.2, Version=18.2.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
</head>
<body style="background-color: #DDD">
    <form id="form1" runat="server">
    <div>
        <div style="border: 1px solid #3399FF; padding: 0px 2px 0px 2px; width: 250px; margin: 0px auto;
            background-color: #EEE;">
            <asp:Login ID="login" runat="server" Width="100%" OnLoginError="login_LoginError">
                <LayoutTemplate>
                    <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                        padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                        Log In</div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="UserNameLabel" runat="server" Text="User Name:" AssociatedControlID="UserName">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="UserName" runat="server" Width="100%">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="LoginGroup">
                                        <RequiredField IsRequired="true" ErrorText="User Name is required." />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="PasswordLabel" runat="server" Text="Password:" AssociatedControlID="Password">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="Password" runat="server" Width="100%" Password="true">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="LoginGroup">
                                        <RequiredField IsRequired="true" ErrorText="Password is required." />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <dx:ASPxCheckBox ID="RememberMe" runat="server" Text="Remember me next time.">
                                </dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                                <dx:ASPxButton ID="btnLogin" runat="server" Text="Log In" CommandName="Login" CausesValidation="true"
                                    ValidationGroup="LoginGroup" ValidationContainerID="login" Width="60px">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <div id="errorRow" style="background-color: #FFC8C8; padding: 2px 4px 2px 4px; text-align: justify;
                        display: none;">
                        <dx:ASPxLabel ID="FailureText" runat="server" Text="" EnableViewState="false" ForeColor="Red">
                        </dx:ASPxLabel>
                    </div>
                </LayoutTemplate>
            </asp:Login>
            <dx:ASPxLabel ID="lbl1" runat="server" Text="Do not have an account? ">
            </dx:ASPxLabel>
            <dx:ASPxHyperLink ID="lnkCreateAccount" runat="server" Text="Create" NavigateUrl="NewAccount.aspx">
            </dx:ASPxHyperLink>
            <dx:ASPxLabel ID="lbl2" runat="server" Text="it now!">
            </dx:ASPxLabel>
        </div>
    </div>
    </form>
</body>
</html>
