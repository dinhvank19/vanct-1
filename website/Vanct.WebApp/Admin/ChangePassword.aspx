<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Vanct.WebApp.Admin.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Đổi mật khẩu 
        <asp:Label ID="lbl" runat="server" ForeColor="red"/></h2>
    <table>
        <tr>
            <td>Mật khẩu mới</td>
            <td>
                <telerik:RadTextBox ID="txtNewPass" Runat="server" TextMode="Password"/>
                
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <telerik:RadButton ID="RadButton1" runat="server" onclick="RadButton1Click" Text="Đổi mật khẩu"/>
                
                <telerik:RadButton ID="RadButton2" runat="server" onclick="RadButton2Click" Text="Thoát, đăng nhập lại"/>
                
            </td>
        </tr>
    </table>
</asp:Content>