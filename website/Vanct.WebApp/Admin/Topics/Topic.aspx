<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Topic.aspx.cs" Inherits="Vanct.WebApp.Admin.Topics.Topic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Cập nhật bài viết</h2>
    <table>
        <tr>
            <td>Id</td>
            <td>
                <telerik:RadTextBox ID="txtId" runat="server" ReadOnly="True" Width="500"/>
            </td>
        </tr>
        <tr>
            <td>Tiêu đề</td>
            <td>
                <telerik:RadTextBox ID="txtName" runat="server" Width="500"/>
            </td>
        </tr>
    </table>
    <telerik:RadEditor ID="txtContent" runat="server" Width="100%" Skin="Metro" />
    <br/>
    <telerik:RadButton runat="server" ID="btnSave" Text="Lưu lại" OnClick="BtnSaveClick"/>
    <telerik:RadButton runat="server" ID="btnBack" Text="Quay lại" OnClick="BtnBackClick"/>
</asp:Content>