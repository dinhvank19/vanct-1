<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="CompanyUserList.aspx.cs" Inherits="Vanct.WebApp.Admin.Pos.CompanyUserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <h2>Tài khoản trực tuyến - <asp:Literal runat="server" ID="lblTitle"/></h2>
        <p>
            <telerik:RadButton ID="btnGenerate" runat="server" Text="Tạo mới" OnClick="BtnGenerate" />
        </p>
        <telerik:RadGrid AutoGenerateColumns="False" ID="grid" runat="server"
            AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-Width="30px" AllowFiltering="False">
                        <ItemTemplate>
                            <img alt='<%#(bool)Eval("IsError")? "Lỗi" : "Đã kích hoạt" %>'
                                src='<%#ResolveUrl("~/Content/Themes/Icons/" + ((bool)Eval("IsError")? "expired" : "active") + ".png") %>'
                                title='<%#(bool)Eval("IsError")? "Lỗi" : "Đã kích hoạt" %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="KEY" DataField="UniqueId" SortExpression="UniqueId" />
                    <telerik:GridBoundColumn HeaderText="Ngày tạo" DataField="CreatedDate" SortExpression="CreatedDate" DataFormatString="{0:dd-MM-yyyy hh:mm}" />
                    <telerik:GridBoundColumn HeaderText="Mã máy" DataField="DeviceUuid" SortExpression="DeviceUuid" />
                    <telerik:GridBoundColumn HeaderText="Tên máy" DataField="DeviceName" SortExpression="DeviceName" />
                    <telerik:GridBoundColumn HeaderText="Lần đầu đăng nhập" DataField="FirstLoginDate" SortExpression="FirstLoginDate" />
                    <telerik:GridBoundColumn HeaderText="Lần cuối đăng nhập" DataField="LastLoginDate" SortExpression="LastLoginDate" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
</asp:Content>
