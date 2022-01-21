<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="FileManage.aspx.cs" Inherits="Vanct.WebApp.Admin.Files.FileManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Tải về</h2>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
        <asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label>
        <telerik:RadGrid AutoGenerateColumns="False" ID="grid" runat="server" Width="800"
            AllowPaging="True" AllowSorting="True" AllowFilteringByColumn="False" PageSize="20" OnItemCommand="GridItemCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="Mô tả" DataField="Name" SortExpression="Name" />
                    <telerik:GridTemplateColumn HeaderStyle-Width="60px" AllowFiltering="False">
                        <HeaderTemplate>
                            <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/Content/Themes/Icons/new.png" OnClientClick=" window.location = 'FileInsert.aspx'; return false; "
                                ToolTip="Thêm mới" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png" ToolTip="Sửa"
                                CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Content/Themes/Icons/delete.png"
                                OnClientClick=" return confirm('Xóa không?'); "
                                ToolTip="Xóa" CommandName="cmdDelete" CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
</asp:Content>
