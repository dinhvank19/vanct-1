<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Site1.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Vanct.WebApp.Report.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/MetroUI/min/iconFont.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/MetroUI/min/metro-bootstrap.min.css") %>" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Content/MetroUI/min/jquery.widget.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Content/MetroUI/min/metro.min.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="rightInfoContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="metro _header-margin50">
        <div id="lblMessage" style="color: red"></div>
        <div class="login">
            <div class="input-control password" data-role="input-control">
                <input type="password" id="txtOldPassword" placeholder="Mật khẩu hiện tại" />
                <button class="btn-reveal" tabindex="-1"></button>
            </div>
            <div class="input-control password" data-role="input-control">
                <input type="password" id="txtNewPassword" placeholder="Mật khẩu mới" />
                <button class="btn-reveal" tabindex="-1"></button>
            </div>
            <div class="input-control password" data-role="input-control">
                <input type="password" id="txtConfirmNewPass" placeholder="Nhập lại mật khẩu mới" />
                <button class="btn-reveal" tabindex="-1"></button>
            </div>
            <div>
                <button class="command-button warning" onclick="changePassword();" style="width: 250px;">
                    <i class="icon-locked on-left"></i>
                    <span style="font-size: 1.1em; line-height: 1.2em;">Đổi mật khẩu</span>
                    <small style="font-size: 14px; line-height: 18px;">Nhập đầy đủ thông tin</small>
                </button>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var lblMessage = $("#lblMessage");

        function changePassword() {
            var postData = {
                Pass: $('#txtOldPassword').val(),
                NewPass: $('#txtNewPassword').val(),
                ConfirmNewPass: $('#txtConfirmNewPass').val()
            };

            if (postData.Pass == '') {
                lblMessage.text('Vui lòng nhập mật khẩu hiện tại');
                return;
            }

            if (postData.NewPass == '') {
                lblMessage.text('Vui lòng nhập mật khẩu mới');
                return;
            }

            if (postData.ConfirmNewPass == '') {
                lblMessage.text('Vui lòng nhập lại mật khẩu mới');
                return;
            }

            if (postData.NewPass != postData.ConfirmNewPass) {
                lblMessage.text('Mật khẩu mới không hợp lệ');
                return;
            }

            $.Hulk.CallbackRequestUrl('<%=ResolveUrl("~/Report/Worker.ashx?action=ChangePassword") %>', 'json', postData, function (rs) {
                lblMessage.text(rs.Message);
            });
        }
    </script>
</asp:Content>
