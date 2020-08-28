<%@ Page Language="vb" AutoEventWireup="true" CodeFile="RetrievePassword.aspx.vb"Inherits="Account_RetrievePassword" %>

<%@ Register Assembly="DevExpress.Web.v18.2, Version=18.2.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Retrieve Password</title>
</head>
<body style="background-color: #DDD">
    <form id="form1" runat="server">
    <div>
        <div style="border: 1px solid #3399FF; padding: 1px 2px 1px 2px; width: 250px; margin: 0px auto;
            background-color: #EEE;">
            <asp:PasswordRecovery ID="PasswordRecovery" runat="server" OnUserLookupError="PasswordRecovery_UserLookupError"
                OnAnswerLookupError="PasswordRecovery_AnswerLookupError" OnSendMailError="PasswordRecovery_SendMailError"
                Width="100%">
                <QuestionTemplate>
                    <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                        padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                        Identity Confirmation</div>
                    <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                        padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                        <dx:ASPxLabel ID="lblDescription" runat="server" Text="Answer the following question to receive your password.">
                        </dx:ASPxLabel>
                    </div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="lbUserName" runat="server" Text="User Name:">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="UserName" runat="server" ForeColor="Red">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="lblQuestion" runat="server" Text="Question:">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="Question" runat="server">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="lblAnswer" runat="server" Text="Answer:" AssociatedControlID="Answer">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="Answer" runat="server" Width="100%">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PasswordRecoveryGroup">
                                        <RequiredField IsRequired="true" ErrorText="Answer is required." />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <dx:ASPxButton ID="btnSubmit" runat="server" Text="Submit" CommandName="Submit" CausesValidation="true"
                                    ValidationGroup="PasswordRecoveryGroup" ValidationContainerID="login">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <div id="answerErrorRow" style="background-color: #FFC8C8; padding: 2px 4px 2px 4px;
                        text-align: justify; display: none;">
                        <dx:ASPxLabel ID="FailureText" runat="server" Text="" EnableViewState="false" ForeColor="Red">
                        </dx:ASPxLabel>
                    </div>
                </QuestionTemplate>
                <SuccessTemplate>
                    <div style="background-color: #C8FFC8; padding: 2px 4px 2px 4px; text-align: justify;
                        border: 1px solid green">
                        <dx:ASPxLabel ID="lblComplete" runat="server" Text="Your password has been sent to you."
                            EnableViewState="false" ForeColor="Green">
                        </dx:ASPxLabel>
                    </div>
                </SuccessTemplate>
                <UserNameTemplate>
                    <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                        padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                        Forgot Your Password?</div>
                    <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                        padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                        <dx:ASPxLabel ID="lblDescription" runat="server" Text="Enter your User Name to receive your password.">
                        </dx:ASPxLabel>
                    </div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 0px; white-space: nowrap">
                                <dx:ASPxLabel ID="UserNameLabel" runat="server" Text="User Name:" AssociatedControlID="UserName">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="UserName" runat="server" Width="100%">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PasswordRecoveryGroup">
                                        <RequiredField IsRequired="true" ErrorText="User Name is required." />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <dx:ASPxButton ID="btnSubmit" runat="server" Text="Submit" CommandName="Submit" CausesValidation="true"
                                    ValidationGroup="PasswordRecoveryGroup" ValidationContainerID="login">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <div id="userNameErrorRow" style="background-color: #FFC8C8; padding: 2px 4px 2px 4px;
                        text-align: justify; display: none;">
                        <dx:ASPxLabel ID="FailureText" runat="server" Text="" EnableViewState="false" ForeColor="Red">
                        </dx:ASPxLabel>
                    </div>
                </UserNameTemplate>
            </asp:PasswordRecovery>
        </div>
    </div>
    </form>
</body>
</html>