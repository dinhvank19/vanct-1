<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="PostLinks.aspx.cs" Inherits="Vanct.WebApp.Admin.Postlinks.PostLinks" %>
<%@ Import Namespace="Vanct.WebApp.AppCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Danh sách
        <asp:Literal runat="server" ID="lblTitle" /></h2>

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
                <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Vị trí" DataField="Position" SortExpression="Position" />
                <telerik:GridBoundColumn HeaderStyle-Width="100px" HeaderText="Trang chủ" DataField="HomeShowed" SortExpression="HomeShowed" />
                <telerik:GridTemplateColumn HeaderStyle-Width="60px" AllowFiltering="False">
                    <HeaderTemplate>
                        <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/Content/Themes/Icons/new.png"
                            OnClientClick="return gotoInsertPage();" ToolTip="Thêm mới sản phẩm" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png"
                            ToolTip="Sửa sản phẩm" CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Content/Themes/Icons/delete.png"
                            OnClientClick=" return confirm('Xóa không?'); "
                            ToolTip="Xóa sản phẩm" CommandName="cmdDelete" CommandArgument='<%#Eval("Id") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <script>
        function gotoInsertPage() {
            window.location = 'PostLinkInsert.aspx?n=<%=VanctContext.RequestName %>';
            return false;
        }
    </script>
</asp:Content>
