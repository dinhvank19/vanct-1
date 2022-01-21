<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Vanct.WebApp.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Đăng nhập :: Quản Lý Website</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:Login ID="Login1" runat="server" onauthenticate="Login1Authenticate">
            </asp:Login>
        </form>
    </body>
</html>