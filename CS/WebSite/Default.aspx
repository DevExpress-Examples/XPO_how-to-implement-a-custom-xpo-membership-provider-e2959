<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Default Page</title>
    <style type="text/css">
        /* http://www.smashingmagazine.com/2008/08/13/top-10-css-table-designs/ (no hover) */
        #xpoUsers
        {
            border-collapse: collapse;
            font-family: "Lucida Sans Unicode" , "Lucida Grande" ,Sans-Serif;
            font-size: 12px;
            margin: 20px;
            text-align: left;
            width: 480px;
        }
        #xpoUsers th
        {
            background: none repeat scroll 0 0 #B9C9FE;
            border-bottom: 1px solid #FFFFFF;
            border-top: 4px solid #AABCFE;
            color: #003399;
            font-size: 13px;
            font-weight: normal;
            padding: 8px;
        }
        #xpoUsers td
        {
            background: none repeat scroll 0 0 #E8EDFF;
            border-bottom: 1px solid #FFFFFF;
            border-top: 1px solid transparent;
            color: #666699;
            padding: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1>
        Congratulation, you have opened our demo!</h1>
    The example demonstrates the XPO Membership provider. If you enjoy using it, please
    post your comments, we would appreciate it :)
    <br />
    In the example, all user profiles are stored in the <strong>InMemoryDataStore</strong>
    data source. This means that if this demo has been stopped, all modifications might
    be lost. By default, two users should be available for everyone:
    <table summary="default users" id="xpoUsers">
        <thead>
            <tr>
                <th scope="col">
                    Username
                </th>
                <th scope="col">
                    Password
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    admin
                </td>
                <td>
                    admin
                </td>
            </tr>
            <tr>
                <td>
                    test
                </td>
                <td>
                    test
                </td>
            </tr>
        </tbody>
    </table>
    Regards,<br />
    Vest
</asp:Content>
