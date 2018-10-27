<%@ Page Language="vb" AutoEventWireup="true" CodeFile="NewAccount.aspx.vb" Inherits="NewAccount" %>

<%@ Register Assembly="DevExpress.Web.v18.2, Version=18.2.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create An Account</title>
</head>
<body style="background-color: #DDD">
    <form id="form1" runat="server">
    <div>
        <div style="border: 1px solid #3399FF; padding: 0px 2px 1px 2px; width: 250px; margin: 0px auto;
            background-color: #EEE;">
            <asp:CreateUserWizard ID="CreateUser" runat="server" OnCreateUserError="CreateUser_CreateUserError"
                OnContinueButtonClick="CreateUser_ContinueButtonClick" Width="100%">

                <WizardSteps>
                    <asp:CreateUserWizardStep runat="server">
                        <ContentTemplate>
                            <div style="border: 1px solid #9999FF; background-color: #99CCFF; text-align: center;
                                padding-top: 2px; padding-bottom: 2px; margin-top: 2px; margin-bottom: 2px;">
                                Sign Up for Your New Account</div>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 0px; white-space: nowrap">
                                        <dx:ASPxLabel ID="UserNameLabel" runat="server" Text="User Name:" AssociatedControlID="UserName">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="UserName" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="CreateUserGroup">
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
                                        <dx:ASPxTextBox ID="Password" runat="server" Width="100%" Password="true" ClientInstanceName="pass1">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="CreateUserGroup">
                                                <RequiredField IsRequired="true" ErrorText="Password is required." />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password:" AssociatedControlID="ConfirmPassword">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ConfirmPassword" runat="server" Width="100%" Password="true"
                                            ClientInstanceName="pass2">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="CreateUserGroup"
                                                EnableCustomValidation="true" ErrorText="The Password and Confirmation Password must match.">
                                                <RequiredField IsRequired="true" ErrorText="Confirm Password is required." />
                                            </ValidationSettings>
                                            <ClientSideEvents Validation="function(s, e) { if (e.isValid) e.isValid = (pass1.GetText() == pass2.GetText()); }" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="Email" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="CreateUserGroup">
                                                <RegularExpression ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    ErrorText="E-mail is invalid." />
                                                <RequiredField IsRequired="true" ErrorText="E-mail is required." />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="QuestionLabel" runat="server" AssociatedControlID="Question" Text="Security Question:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="Question" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="CreateUserGroup">
                                                <RequiredField IsRequired="true" ErrorText="Security question is required." />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="AnswerLabel" runat="server" AssociatedControlID="Answer" Text="Security Answer:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="Answer" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="CreateUserGroup">
                                                <RequiredField IsRequired="true" ErrorText="Security answer is required." />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="errorRow" style="background-color: #FFC8C8; padding: 2px 4px 2px 4px; text-align: justify;
                                display: none;">
                                <dx:ASPxLabel ID="ErrorMessage" runat="server" Text="" EnableViewState="false" ForeColor="Red">
                                </dx:ASPxLabel>
                            </div>
                        </ContentTemplate>
                        <CustomNavigationTemplate>
                            <table border="0" cellspacing="5" style="width: 100%; height: 100%;">
                                <tr align="right">
                                    <td align="right" colspan="0">
                                        <dx:ASPxButton ID="StepNextButton" runat="server" Text="Create User" CommandName="MoveNext"
                                            CausesValidation="true" ValidationGroup="CreateUserGroup" ValidationContainerID="CreateUser">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </CustomNavigationTemplate>
                    </asp:CreateUserWizardStep>
                    <asp:CompleteWizardStep runat="server">
                        <ContentTemplate>
                            <div style="background-color: #FFFCCA; border: 1px solid #CCCCCC; border-top-style: none;
                                padding: 8px 20px 5px; color: #696969;">
                                <dx:ASPxLabel ID="lblComplete" runat="server" EnableViewState="false" EncodeHtml="false"
                                    Text="<b>Complete</b><br />Your account has been successfully created.">
                                </dx:ASPxLabel>
                            </div>
                            <div style="width: 70px; margin-left: auto;">
                                <dx:ASPxButton ID="ContinueButton" runat="server" Text="Continue" CommandName="Continue"
                                    CausesValidation="false" Width="70px">
                                </dx:ASPxButton>
                            </div>
                        </ContentTemplate>
                    </asp:CompleteWizardStep>
                </WizardSteps>
            </asp:CreateUserWizard>

        </div>
    </div>
    </form>
</body>
</html>