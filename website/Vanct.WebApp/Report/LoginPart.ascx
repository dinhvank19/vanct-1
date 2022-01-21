<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginPart.ascx.cs" Inherits="Vanct.WebApp.Report.LoginPart" %>
<link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/MetroUI/min/iconFont.min.css") %>" />
<link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/MetroUI/min/metro-bootstrap.min.css") %>" />
<script type="text/javascript" src="<%=ResolveUrl("~/Content/MetroUI/min/jquery.widget.min.js") %>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Content/MetroUI/min/metro.min.js") %>"></script>
<div class="metro _header-margin50">
    <div id="lblMessage" style="color: red"></div>
    <div class="login">
        <div class="input-control text" data-role="input-control">
            <input type="text" placeholder="Tài khoản" id="txtUsername" />
            <button class="btn-clear" tabindex="-1"></button>
        </div>
        <div class="input-control password" data-role="input-control">
            <input type="password" id="txtPassword" placeholder="Mật khẩu" />
            <button class="btn-reveal" tabindex="-1"></button>
        </div>
        <div>
            <div class="input-control checkbox" data-role="input-control">
                <label class="inline-block">
                    <input type="checkbox" id="chkKeepLogin" />
                    <span class="check"></span>
                    Giữ thông tin đăng nhập
                </label>
            </div>
        </div>
        <div>
            <button class="command-button warning" onclick="login();" style="width: 250px;">
                <i class="icon-locked on-left"></i>
                <span style="font-size: 1.1em; line-height: 1.2em;">Đăng nhập</span>
                <small style="font-size: 14px; line-height: 18px;">Nhập tài khoản và mật khẩu</small>
            </button>
        </div>
    </div>
</div>
<script type="text/javascript">
    var lblMessage = $("#lblMessage");
    var LoginControl = {
        Username: $('#txtUsername'),
        Password: $('#txtPassword'),
        KeepBox: $('#chkKeepLogin').get(0)
    };

    function login() {
        var postData = {
            checked: LoginControl.KeepBox.checked,
            username: LoginControl.Username.val(),
            password: LoginControl.Password.val()
        };

        if (postData.checked) {
            SetLoginData(postData);
        } else {
            delete localStorage.VanctLoginData;
        }

        $.Hulk.CallbackRequestUrl('<%=ResolveUrl("~/Report/Worker.ashx?action=Login") %>', 'json', postData, function (rs) {
            if (!rs.Result) {
                lblMessage.text(rs.Message);
                return;
            }

            window.location = '<%=ResolveUrl("~/Report/Default.aspx") %>';
        });
    }

    function GetLoginData() {
        return localStorage.VanctLoginData
            ? JSON.parse(localStorage.VanctLoginData)
            : { checked: false, username: '', password: '' };
    }

    function SetLoginData(value) {
        localStorage.VanctLoginData = JSON.stringify(value);
    }

    $(document).ready(function () {
        var postData = GetLoginData();
        if (postData.checked) {
            LoginControl.Username.val(postData.username);
            LoginControl.Password.val(postData.password);
            LoginControl.KeepBox.checked = true;
        } else {
            LoginControl.Username.val('');
            LoginControl.Password.val('');
            LoginControl.KeepBox.checked = false;
        }
    });
</script>