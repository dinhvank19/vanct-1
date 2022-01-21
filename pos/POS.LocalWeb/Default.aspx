<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="POS.LocalWeb.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACE SOFT</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ForeColor="red" ID="lblMessage" /></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtUsername" /></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtPassword" TextMode="Password" /></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton runat="server" Text="Đăng nhập" ID="btnLogin" OnClick="BtnLogin" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
