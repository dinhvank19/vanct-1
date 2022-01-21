<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="POS.WebApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LOGIN</title>
    <link href="~/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/css/signin.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <form id="form1" runat="server" class="form-signin">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Blue" />
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <asp:Panel runat="server" ID="panel" Visible="False">
                    <div class="alert alert-danger" role="alert">
                        <asp:Literal runat="server" ID="lblMessage"/>
                    </div>
                </asp:Panel>
                <label for="<%#txtUsername.ClientID %>" class="sr-only">Tài khoản</label>
                <asp:TextBox runat="server" ID="txtUsername" AutoCompleteType="None" CssClass="form-control username" placeholder="Tài khoản" />
                <label for="<%#txtPassword.ClientID %>" class="sr-only">Mật khẩu</label>
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" AutoCompleteType="None" CssClass="form-control password" placeholder="Mật khẩu" />
                <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-lg btn-primary btn-block" Text="Đăng nhập" OnClick="BtnLoginClick" />
            </telerik:RadAjaxPanel>
        </form>

    </div>
</body>
</html>
