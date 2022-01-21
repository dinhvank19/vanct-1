<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="FileInsert.aspx.cs" Inherits="Vanct.WebApp.Admin.Files.FileInsert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Thêm tải về</h2>
    <telerik:RadButton ID="btnSave" runat="server" Text="Lưu lại" OnClick="BtnSaveClick" />
    <telerik:RadButton ID="btnBack" runat="server" Text="Xem danh sách" OnClick="BtnBackClick" />
    <telerik:RadButton ID="btnCreate" runat="server" Text="Hủy bỏ" OnClick="BtnCreateClick" />
    <asp:Label runat="server" ID="lblMessage" ForeColor="red" />
    <table>
        <tr>
            <td>Tên hiển thị</td>
            <td><telerik:RadTextBox ID="txtName" runat="server" Width="400"/></td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Chọn tập tin từ máy tính</td>
            <td style="vertical-align: top;"><telerik:RadUpload ID="fileUrl" runat="server" AllowedFileExtensions=".docx, .doc, .pdf" ControlObjectsVisibility="None" /></td>
        </tr>
        <tr>
            <td>Vị trí hiển thị</td>
            <td><telerik:RadNumericTextBox ID="txtPosition" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="9999999" DataType="System.Int32" Value="0" /></td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Chọn hình từ máy tính</td>
            <td style="vertical-align: top;"><telerik:RadUpload ID="imageURL" runat="server" AllowedFileExtensions=".jpg, .gif, .png" ControlObjectsVisibility="None" /></td>
        </tr>
    </table>
    <h3>Nội dung</h3>
    <telerik:RadEditor ID="txtContent" runat="server" Width="100%" Skin="Metro" />
    <br/>
    <telerik:RadButton ID="RadButton1" runat="server" Text="Lưu lại" OnClick="BtnSaveClick" />
    <telerik:RadButton ID="RadButton2" runat="server" Text="Xem danh sách" OnClick="BtnBackClick" />
    <telerik:RadButton ID="RadButton3" runat="server" Text="Hủy bỏ" OnClick="BtnCreateClick" />
</asp:Content>
