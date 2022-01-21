<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="POS.WebApp.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CHANGE PASSWORD</title>
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
                <label for="<%#txtPassword.ClientID %>" class="sr-only">Mật khẩu mới</label>
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" AutoCompleteType="None" CssClass="form-control username" placeholder="Mật khẩu mới" />
                <label for="<%#txtConfirmPassword.ClientID %>" class="sr-only">Nhập lại mật khẩu</label>
                <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" AutoCompleteType="None" CssClass="form-control password" placeholder="Nhập lại mật khẩu" />
                <asp:Button runat="server" ID="btnChangePassword" CssClass="btn btn-lg btn-primary btn-block" Text="Đổi mật khẩu" OnClick="BtnChangePasswordClick" />
                <asp:Button runat="server" ID="btnBack" CssClass="btn btn-lg btn-danger btn-block" Text="Quay lại" OnClick="BtnBackClick" />
            </telerik:RadAjaxPanel>
        </form>

    </div>
</body>
</html>
