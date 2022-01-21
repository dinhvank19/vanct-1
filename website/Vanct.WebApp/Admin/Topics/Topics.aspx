<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="Vanct.WebApp.Admin.Topics.Topics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Danh sách bài viết</h2>
    <telerik:RadGrid AutoGenerateColumns="False" ID="grid" runat="server"
                     AllowPaging="True" AllowSorting="True" AllowFilteringByColumn="False"
                     PageSize="20" Width="600" onitemcommand="GridItemCommand">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true" />
            <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
        </ClientSettings>
        <MasterTableView DataKeyNames="Id">
            <Columns>
                <telerik:GridBoundColumn HeaderText="Id" DataField="Id" SortExpression="Id" Visible="False"/>

                <telerik:GridBoundColumn HeaderText="Tên" DataField="Name" SortExpression="Name"/>
                        
                <telerik:GridTemplateColumn HeaderStyle-Width="30px" AllowFiltering="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png"
                                         ToolTip="Sửa sản phẩm" CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>