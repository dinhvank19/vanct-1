<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ProductInsert.aspx.cs" Inherits="Vanct.WebApp.Admin.Product.ProductInsert" %>

<%@ Register Src="~/UserControls/CmbProductTypeGroup.ascx" TagName="CmbProductTypeGroup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/CmbProductType.ascx" TagName="CmbProductType" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Thêm mới sản phẩm</h2>
    <telerik:RadButton ID="RadButton1" runat="server" Text="Lưu lại" OnClick="BtnSaveClick" />
    <telerik:RadButton ID="RadButton2" runat="server" Text="Xem sản phẩm" OnClick="BtnBackClick" />
    <telerik:RadButton ID="RadButton3" runat="server" Text="Hủy bỏ" OnClick="BtnCreateClick" />
    <asp:Label runat="server" ID="lblMessage" ForeColor="red" />
    <table>
        <tr>
            <td>Tên sản phẩm <span class="_important">(*)</span></td>
            <td>
                <telerik:RadTextBox ID="txtName" runat="server" Width="500" /></td>
        </tr>
        <tr>
            <td>Nhóm sản phẩm <span class="_important">(*)</span></td>
            <td>
                <uc1:CmbProductTypeGroup ID="CmbProductGroup" runat="server" Width="300" EmptyFirstRow="True" ProductTypeControlName="CmbProductType" />
            </td>
        </tr>
        <tr>
            <td>Nhóm con</td>
            <td>
                <uc2:CmbProductType ID="CmbProductType" runat="server" Width="300" EmptyFirstRow="True" />
            </td>
        </tr>
        <tr>
            <td>Giá sản phẩm</td>
            <td>
                <telerik:RadTextBox ID="txtPriceVnd" runat="server" Width="300" /></td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Hình đại diện <span class="_important">(*)</span></td>
            <td style="vertical-align: top;">
                <telerik:RadUpload ID="imageURL" runat="server" AllowedFileExtensions=".jpg, .gif, .png" ControlObjectsVisibility="None" />
                <b>(.jpg, .gif, .png)</b>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Sơ lượt</td>
            <td style="vertical-align: top;">
                <telerik:RadTextBox ID="txtNote" runat="server" Width="500" Height="100" TextMode="MultiLine" />
            </td>
        </tr>
        <tr style="display: none;">
            <td>Bảo hành</td>
            <td>
                <telerik:RadTextBox ID="txtWarranty" runat="server" Width="300" /></td>
        </tr>
        <tr>
            <td>Vị trí hiển thị</td>
            <td><telerik:RadNumericTextBox ID="txtPosition" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="9999999" DataType="System.Int32" Value="0" /></td>
        </tr>
    </table>
    <table>
        <tr>
            <td>Hiện thị trang chủ</td>
            <td><asp:CheckBox ID="ckIsHomeShowed" runat="server" Checked="False" /></td>
            <td style="display: none;"><b>Khuyễn mãi</b></td>
            <td style="display: none;"><asp:CheckBox ID="ckIsSaleOff" runat="server" Checked="False" /></td>
        </tr>
        <tr>
            <td>Hoạt động</td>
            <td><asp:CheckBox ID="ckIsActive" runat="server" Checked="True" /></td>
            <td style="display: none;"><b>Đang HOT</b></td>
            <td style="display: none;"><asp:CheckBox ID="ckIsHot" runat="server" Checked="False" /></td>
        </tr>
    </table>
    <h3>Mô tả chi tiết</h3>
    <telerik:RadEditor ID="txtContent" runat="server" Width="100%" Skin="Metro" />
    <br />
    <telerik:RadButton ID="btnSave" runat="server" Text="Lưu lại" OnClick="BtnSaveClick" />
    <telerik:RadButton ID="btnBack" runat="server" Text="Xem sản phẩm" OnClick="BtnBackClick" />
    <telerik:RadButton ID="btnCreate" runat="server" Text="Hủy bỏ" OnClick="BtnCreateClick" />
</asp:Content>
