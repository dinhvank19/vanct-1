<%@ Page Title="" Language="C#" MasterPageFile="~/Baocao/Report.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Namviet.Baocao.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Đổi mật khẩu | NamvietKhanhHoa.com</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-6">
                <h2>Đổi mật khẩu 
        <asp:Label ID="lbl" runat="server" ForeColor="red" /></h2>
                <div class="form-group">
                    <label>Mật khẩu mới</label>
                    <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Button ID="RadButton1" runat="server" CssClass="btn btn-success" OnClick="RadButton1Click" Text="Đổi mật khẩu" />
                </div>
                <div class="form-group">
                    <asp:Button ID="RadButton2" runat="server" CssClass="btn-default btn" OnClick="RadButton2Click" Text="Thoát, đăng nhập lại" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
