<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="SupportOnline.aspx.cs" Inherits="Vanct.WebApp.Admin.Others.SupportOnline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <h2>Skype|Yahoo|Email|Facebook</h2>
        <table>
            <tr>
                <td>Skype</td>
                <td><telerik:RadTextBox ID="txtSkype" runat="server" Width="500"/></td>
            </tr>
            <tr>
                <td>Yahoo</td>
                <td><telerik:RadTextBox ID="txtYahoo" runat="server" Width="500"/></td>
            </tr>
            <tr>
                <td>Facebook</td>
                <td><telerik:RadTextBox ID="txtFacebook" runat="server" Width="500"/></td>
            </tr>
            <tr>
                <td>Hotline</td>
                <td><telerik:RadTextBox ID="txtHotline" runat="server" Width="500"/></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><telerik:RadTextBox ID="txtEmail" runat="server" Width="500"/></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <telerik:RadButton runat="server" ID="btnSave" Text="Thêm mới" OnClick="BtnSaveClick" />
                </td>
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
                    <telerik:GridBoundColumn HeaderText="Skype" DataField="Skype" SortExpression="Skype" />
                    <telerik:GridBoundColumn HeaderText="Yahoo" DataField="Yahoo" SortExpression="Yahoo" />
                    <telerik:GridBoundColumn HeaderText="Facebook" DataField="Facebook" SortExpression="Facebook" />
                    <telerik:GridBoundColumn HeaderText="Hotline" DataField="Hotline" SortExpression="Hotline" />
                    <telerik:GridBoundColumn HeaderText="Email" DataField="Email" SortExpression="Email" />
                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" AllowFiltering="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Content/Themes/Icons/delete.png"
                                OnClientClick=" return confirm('Xóa không?'); "
                                ToolTip="Xóa sản phẩm" CommandName="cmdDelete" CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
</asp:Content>
