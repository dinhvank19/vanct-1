<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="Vanct.WebApp.Admin.Product.Products" %>

<%@ Register Src="~/UserControls/CmbProductTypeGroup.ascx" TagName="CmbProductTypeGroup" TagPrefix="uc1" %>

<%@ Register Src="~/UserControls/CmbProductType.ascx" TagName="CmbProductType" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <h2>Danh sách sản phẩm</h2>
        <table>
            <tr>
                <td>
                    <uc1:CmbProductTypeGroup ID="cmbGroup" runat="server" Width="300" EmptyFirstRow="True" ProductTypeControlName="cmbType" />
                </td>
                <td>
                    <uc2:CmbProductType ID="cmbType" runat="server" Width="300" EmptyFirstRow="True" />
                </td>
                <td>
                    <telerik:RadButton runat="server" ID="btnFind" OnClick="BtnFindClick" Text="Tìm" />
                </td>
            </tr>
        </table>
        <telerik:RadGrid AutoGenerateColumns="False" ID="gridPro" runat="server"
            AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False"
            OnItemCommand="GridItemCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="Tên" DataField="Name" SortExpression="Name" />
                    <telerik:GridBoundColumn HeaderStyle-Width="200px" HeaderText="Nhóm sản phẩm" DataField="TypeGroupName" SortExpression="TypeGroupName" />
                    <telerik:GridBoundColumn HeaderStyle-Width="200px" HeaderText="Nhóm con" DataField="TypeName" SortExpression="TypeName" />
                    <%--<telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Bảo hành" DataField="Warranty" SortExpression="Warranty" />
                    <telerik:GridBoundColumn HeaderStyle-Width="150px" HeaderText="Giá" DataField="PriceVnd" SortExpression="PriceVnd" />
                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Khuyến mãi" DataField="IsSaleOff" SortExpression="IsSaleOff" />
                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Đang HOT" DataField="IsHot" SortExpression="IsHot" />--%>
                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Trang chủ" DataField="HomeShow" SortExpression="HomeShow" />
                    <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Hoạt động" DataField="Active" SortExpression="Active" />
                    <telerik:GridTemplateColumn HeaderStyle-Width="60px" AllowFiltering="False">
                        <HeaderTemplate>
                            <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/Content/Themes/Icons/new.png" OnClientClick=" window.location = 'ProductInsert.aspx'; return false; "
                                ToolTip="Thêm mới sản phẩm" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png" ToolTip="Sửa sản phẩm"
                                CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Content/Themes/Icons/delete.png"
                                OnClientClick=" return confirm('Xóa không?'); "
                                ToolTip="Xóa sản phẩm" CommandName="cmdDelete" CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <EditFormSettings>
                    <EditColumn ButtonType="ImageButton" />
                    <PopUpSettings Modal="true" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
</asp:Content>
