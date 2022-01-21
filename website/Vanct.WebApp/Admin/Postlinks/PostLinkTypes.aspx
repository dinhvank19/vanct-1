<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="PostLinkTypes.aspx.cs" Inherits="Vanct.WebApp.Admin.Postlinks.PostLinkTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Xắp xếp trang chủ</h2>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <asp:HiddenField runat="server" ID="txtId"/>
        <table>
            <tr>
                <td>Tên</td>
                <td><telerik:RadTextBox ReadOnly="True" ID="txtName" runat="server" Width="500"/></td>
            </tr>
            <tr>
                <td>Vị trí hiển thị</td>
                <td><telerik:RadNumericTextBox ID="txtPosition" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="9999999" DataType="System.Int32" Value="0" /></td>
            </tr>
            <tr>
                <td>Hiển thị trang chủ</td>
                <td><asp:CheckBox ID="ckIsHomeShowed" runat="server" Checked="False" /></td>
            </tr>
            <tr>
                <td></td>
                <td><telerik:RadButton runat="server" ID="btnSave" Text="Chỉnh sửa" OnClick="BtnSaveClick" /></td>
            </tr>
        </table>
        <telerik:RadGrid AutoGenerateColumns="False" ID="grid" runat="server" OnItemCommand="GridItemCommand"
            AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="Id" DataField="Id" SortExpression="Id" Visible="False" />
                    <telerik:GridBoundColumn HeaderText="Tên" DataField="Name" SortExpression="Name" />
                    <telerik:GridBoundColumn HeaderText="Vị trí hiển thị" DataField="Position" SortExpression="Position" />
                    <telerik:GridBoundColumn HeaderText="Hiển thị trang chủ" DataField="HomeShowed" SortExpression="HomeShowed" />
                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" AllowFiltering="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png"
                                ToolTip="Sửa" CommandName="cmEdit" CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
</asp:Content>
