<%@ Page Language="vb" AutoEventWireup="true" CodeFile="ChangePassword.aspx.vb" Inherits="Account_ChangePassword" %>

<%@ Register Assembly="DevExpress.Web.v18.2, Version=18.2.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
</head>
<body style="background-color: #DDD">
    <form id="form1" runat="server">
    <div>
        <div style="border: 1px solid #3399FF; padding: 0px 2px 0px 2px; width: 250px; margin: 0px auto;
            background-color: #EEE;">
            <asp:ChangePassword ID="ChangePassword" runat="server" Width="100%" 
                OnChangePasswordError="ChangePassword_ChangePasswordError" 
                oncontinuebuttonclick="ChangePassword_ContinueButtonClick">
                <ChangePasswordTemplate>
                    <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                        padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                        Change Your Password</div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="CurrentPasswordLabel" runat="server" Text="Password:" AssociatedControlID="CurrentPassword">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="CurrentPassword" runat="server" Width="100%" Password="true">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="ChangePasswordGroup">
                                        <RequiredField IsRequired="true" ErrorText="Password is required." />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="NewPasswordLabel" runat="server" Text="New Password:" AssociatedControlID="NewPassword">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="NewPassword" runat="server" Width="100%" Password="true" ClientInstanceName="pass1">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="ChangePasswordGroup">
                                        <RequiredField IsRequired="true" ErrorText="New Password is required." />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:" AssociatedControlID="ConfirmNewPassword">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="ConfirmNewPassword" runat="server" Width="100%" Password="true"
                                    ClientInstanceName="pass2">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="ChangePasswordGroup"
                                        EnableCustomValidation="true" ErrorText="The Confirm New Password must match the New Password entry.">
                                        <RequiredField IsRequired="true" ErrorText="Confirm New Password is required." />
                                    </ValidationSettings>
                                    <ClientSideEvents Validation="function(s, e) { if (e.isValid) e.isValid = (pass1.GetText() == pass2.GetText()); }" />
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <div style="display: inline-block;">
                                    <dx:ASPxButton ID="ChangePasswordPushButton" runat="server" Text="Change Password"
                                        CommandName="ChangePassword" CausesValidation="true" ValidationGroup="ChangePasswordGroup"
                                        ValidationContainerID="ChangePassword">
                                    </dx:ASPxButton>
                                </div>
                                <div style="display: inline-block;">
                                    <dx:ASPxButton ID="CancelPushButton" runat="server" Text="Cancel" CommandName="Cancel"
                                        CausesValidation="false" ValidationGroup="ChangePasswordGroup">
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="changePasswordErrorRow" style="background-color: #FFC8C8; padding: 2px 4px 2px 4px;
                        text-align: justify; display: none;">
                        <dx:ASPxLabel ID="FailureText" runat="server" Text="" EnableViewState="false" ForeColor="Red">
                        </dx:ASPxLabel>
                    </div>
                </ChangePasswordTemplate>
                <SuccessTemplate>
                    <div style="background-color: #C8FFC8; padding: 2px 4px 2px 4px; text-align: justify;
                        border: 1px solid green">
                        <dx:ASPxLabel ID="lblComplete" runat="server" Text="Change Password Complete" EnableViewState="false"
                            ForeColor="Green">
                        </dx:ASPxLabel>
                        <br />
                        <dx:ASPxLabel ID="lblStatus" runat="server" Text="Your password has been changed!"
                            EnableViewState="false" ForeColor="Green">
                        </dx:ASPxLabel>
                    </div>
                    <div style="width: 70px; margin-left: auto;">
                        <dx:ASPxButton ID="ContinuePushButton" runat="server" Text="Continue" CommandName="Continue"
                            CausesValidation="false" Width="70px">
                        </dx:ASPxButton>
                    </div>
                </SuccessTemplate>
            </asp:ChangePassword>
        </div>
    </div>
    </form>
</body>
</html>