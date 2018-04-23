<%@ Page Title="" Language="vb" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="UserList.aspx.vb" Inherits="Default2" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Xpo.v10.2.Web, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Xpo" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <title>A List of Availble Users</title>
</asp:content>
<asp:content id="Content2" contentplaceholderid="body" runat="Server">
    <dx:xpodatasource id="xpoUsers" runat="server" typename="XpoUser">
    </dx:xpodatasource>
    <dx:aspxhyperlink id="lnkColumnsChooser" runat="server" text="Open Customization Window"
        navigateurl="javascript:void(0);">
        <clientsideevents click="function (s, e) { gvUsers.ShowCustomizationWindow(); }" />
    </dx:aspxhyperlink>
    <dx:aspxgridview id="gvUsers" runat="server" autogeneratecolumns="False" datasourceid="xpoUsers"
        keyfieldname="Oid" clientinstancename="gvUsers" oncustomcolumndisplaytext="gvUsers_CustomColumnDisplayText"
        onhtmldatacellprepared="gvUsers_HtmlDataCellPrepared" enablerowscache="false">
        <columns>
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
		</columns>
        <settingscustomizationwindow enabled="true" width="290px" height="314px" />
    </dx:aspxgridview>
</asp:content>
