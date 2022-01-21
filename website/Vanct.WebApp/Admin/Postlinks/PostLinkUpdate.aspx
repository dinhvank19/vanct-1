<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="PostLinkUpdate.aspx.cs" Inherits="Vanct.WebApp.Admin.Postlinks.PostLinkUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Chỉnh sửa <asp:Literal runat="server" ID="lblTitle" /></h2>
    <telerik:RadButton ID="RadButton4" OnClick="BtnSaveClicked" runat="server" Text="Lưu lại"/>
    <telerik:RadButton ID="RadButton5" OnClick="BtnBackClicked" runat="server" Text="Xem danh sách"/>
    <telerik:RadButton ID="RadButton6" OnClick="BtnAddNewClicked" runat="server" Text="Tạo mới"/>
    <asp:Label runat="server" ID="lblMessage" ForeColor="red"/>
    <asp:HiddenField runat="server" ID="txtPostlinkType"/>
    <table>
        <tr>
            <td>Tên</td>
            <td><telerik:RadTextBox ID="txtName" runat="server" Width="400"/></td>
            <td rowspan="3">
                <asp:Image runat="server" ID="oldImage" Width="200"/>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Logo chọn từ máy tính</td>
            <td style="vertical-align: top;">
                <telerik:RadUpload ID="imageURL" runat="server" AllowedFileExtensions=".jpg, .gif, .png" ControlObjectsVisibility="None" />
                <b>(.jpg, .gif, .png)</b>
            </td>
        </tr>
        <tr style="display: none">
            <td>Đường dẫn khi click chuôt lên logo</td>
            <td><telerik:RadTextBox ID="txtLink" runat="server" Width="400"/></td>
        </tr>
        <tr>
            <td>Vị trí hiển thị</td>
            <td>
                <telerik:RadNumericTextBox ID="txtPosition" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="9999999" DataType="System.Int32" Value="0" /></td>
        </tr>
        <tr>
            <td style="vertical-align: top">Thông tin sơ lượt</td>
            <td><telerik:RadTextBox runat="server" ID="txtOverviewContent" Width="400" Height="100" TextMode="MultiLine" MaxLength="1000" /></td>
        </tr>
        <tr>
            <td>Hiện thị trang chủ</td>
            <td><asp:CheckBox ID="ckIsHomeShowed" runat="server" /></td>
        </tr>
        <tr>
            <td>Hoạt động</td>
            <td><asp:CheckBox ID="ckIsActive" runat="server" /></td>
        </tr>
    </table>
    <div>
        <h3>Thông tin chi tiết</h3>
        <telerik:RadEditor runat="server" ID="txtDescription" Width="100%" Skin="Metro"/>
    </div>
    <br/>
    <telerik:RadButton ID="RadButton1" OnClick="BtnSaveClicked" runat="server" Text="Lưu lại"/>
    <telerik:RadButton ID="RadButton2" OnClick="BtnBackClicked" runat="server" Text="Xem danh sách"/>
    <telerik:RadButton ID="RadButton3" OnClick="BtnAddNewClicked" runat="server" Text="Tạo mới"/>
</asp:Content>
