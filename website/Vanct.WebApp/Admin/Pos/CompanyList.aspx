<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="CompanyList.aspx.cs" Inherits="Vanct.WebApp.Admin.Pos.CompanyList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <script>
        function gotoUserList(self) {
            window.location = '<%=ResolveUrl("~/Admin/Pos/CompanyUserList.aspx") %>?cid=' + $(self).attr('alt');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <h2>Quản lý nhà hàng - Bán hàng trực tuyến</h2>
        <div style="float: left; width: 60%;">
            <telerik:RadGrid AutoGenerateColumns="False" ID="grid" runat="server"
                AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False"
                OnItemCommand="GridItemCommand">
                <PagerStyle Mode="NextPrevAndNumeric" />
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                </ClientSettings>
                <MasterTableView DataKeyNames="Id">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px" AllowFiltering="False">
                            <ItemTemplate>
                                <img alt='<%#((bool)Eval("IsActive")? "Đã kích hoạt" : "Tạm dừng") %>' 
                                    src='<%#ResolveUrl("~/Content/Themes/Icons/" + ((bool)Eval("IsActive")? "active" : "expired") + ".png") %>' 
                                    title='<%#((bool)Eval("IsActive")? "Đã kích hoạt" : "Tạm dừng") %>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Tên" DataField="Name" SortExpression="Name" />
                        <telerik:GridBoundColumn HeaderText="Ghi chú" DataField="Description" SortExpression="Description" />
                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" AllowFiltering="False">
                            <HeaderTemplate>
                                <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/Content/Themes/Icons/new.png" ToolTip="Tạo mới nhà hàng"
                                    CommandName="cmdInsert" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png" ToolTip="Sửa"
                                    CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                                <asp:ImageButton ID="btnAddUser" runat="server" ImageUrl="~/Content/Themes/Icons/users.png" ToolTip="Sửa người dùng"
                                    CommandName="cmdAddUser" CommandArgument='<%#Eval("Id") %>' AlternateText='<%#Eval("Id") %>'
                                    OnClientClick=' return gotoUserList(this) ' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div style="float: right; width: 40%;">
            <asp:Panel runat="server" ID="panCrud" Visible="False">
                <div style="margin-left: 10px; border: 1px solid #ccc; padding: 10px; width: 420px;">
                    <h3 style="margin: 0 0 20px 0;"><asp:Literal runat="server" ID="lblTitle" /></h3>
                    <asp:Label runat="server" ID="lblMessage" ForeColor="red" />
                    <table>
                        <tr>
                            <td style="text-align: right">Tên</td>
                            <td><telerik:RadTextBox ID="txtName" runat="server" Width="300" /></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Ghi chú</td>
                            <td><telerik:RadTextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="300" /></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Hoạt động</td>
                            <td><asp:CheckBox ID="ckIsActive" runat="server" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:HiddenField runat="server" ID="txtRecordId" />
                                <telerik:RadButton ID="btnSaveNew" runat="server" Text="Tạo mới" OnClick="BtnSaveNew" />
                                <telerik:RadButton ID="btnReset" runat="server" Text="Đóng" OnClick="BtnReset" />
                                <telerik:RadButton ID="btnSave" runat="server" Text="Lưu lại" OnClick="BtnSave" />
                                <telerik:RadButton ID="btnReload" runat="server" Text="Làm lại" OnClick="BtnReload" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>
        <div style="clear: both;"></div>
    </telerik:RadAjaxPanel>
    
</asp:Content>
