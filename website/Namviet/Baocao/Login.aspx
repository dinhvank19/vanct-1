<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Namviet.Baocao.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng Nhập | NamvietKhanhHoa.com</title>
    <link href="~/assets/bootstrap3.3.7/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-md-6">
                <form id="form1" runat="server">
                    <div class="form-group">
                        <label>Tài khoản</label>
                        <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Mật khẩu</label>
                        <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtPassword" runat="server" />
                    </div>
                    <asp:Button runat="server" CssClass="btn btn-success" Text="Đăng Nhập" ID="btnLogin" OnClick="BtnLoginClick" />
                </form>
            </div>
        </div>
        <asp:Panel runat="server" ID="panelError" Visible="False">
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="alert alert-danger" role="alert">
                        <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                        Tài khoản hoặc mật khẩu không đúng, vui lòng thử lại!
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</body>
</html>
