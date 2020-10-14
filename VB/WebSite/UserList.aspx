<%@ Page Title="" Language="vb" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="UserList.aspx.vb" Inherits="Default2" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Xpo.v13.1.Web, Version=13.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Xpo" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>A List of Availble Users</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <dx:XpoDataSource ID="xpoUsers" runat="server" TypeName="XpoUser">
    </dx:XpoDataSource>
    <dx:ASPxHyperLink ID="lnkColumnsChooser" runat="server" Text="Open Customization Window"
        NavigateUrl="javascript:void(0);">
        <ClientSideEvents Click="function (s, e) { gvUsers.ShowCustomizationWindow(); }" />
    </dx:ASPxHyperLink>
    <dx:ASPxGridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
        DataSourceID="xpoUsers" KeyFieldName="Oid" ClientInstanceName="gvUsers" OnCustomColumnDisplayText="gvUsers_CustomColumnDisplayText"
        OnHtmlDataCellPrepared="gvUsers_HtmlDataCellPrepared" EnableRowsCache="false">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Oid" ReadOnly="True" VisibleIndex="0" SortIndex="0"
                Caption="#" SortOrder="Ascending">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ApplicationName" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="3">
                <Settings SortMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Comment" VisibleIndex="4" Visible="false">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Password" VisibleIndex="5" Visible="False">
                <Settings SortMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PasswordQuestion" VisibleIndex="6" Visible="False">
                <Settings SortMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PasswordAnswer" VisibleIndex="7" Visible="False">
                <Settings SortMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="IsApproved" VisibleIndex="8">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn FieldName="LastActivityDate" VisibleIndex="9">
                <PropertiesDateEdit DisplayFormatString="G">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="LastLoginDate" VisibleIndex="10">
                <PropertiesDateEdit DisplayFormatString="G">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="LastPasswordChangedDate" VisibleIndex="11"
                Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="CreationDate" VisibleIndex="12">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataCheckColumn FieldName="IsOnline" VisibleIndex="13">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="IsLockedOut" VisibleIndex="14" Visible="False">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn FieldName="LastLockedOutDate" VisibleIndex="15" Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="FailedPasswordAttemptCount" VisibleIndex="16"
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="FailedPasswordAttemptWindowStart" VisibleIndex="17"
                Visible="False">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="FailedPasswordAnswerAttemptCount" VisibleIndex="18"
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="FailedPasswordAnswerAttemptWindowStart" VisibleIndex="19"
                Visible="False">
            </dx:GridViewDataDateColumn>
        </Columns>
        <SettingsBehavior EnableCustomizationWindow="true" />
        <SettingsPopup CustomizationWindow-Width="290px" CustomizationWindow-Height="314px" /> 
    </dx:ASPxGridView>
</asp:Content>