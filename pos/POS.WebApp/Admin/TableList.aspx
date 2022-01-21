<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="TableList.aspx.cs" Inherits="POS.WebApp.Admin.TableList" %>

<%@ Register Src="~/UserControls/CmbValidStatus.ascx" TagPrefix="uc1" TagName="CmbValidStatus" %>
<%@ Register Src="~/UserControls/CmbTableArea.ascx" TagPrefix="uc1" TagName="CmbTableArea" %>


<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <h2>Bàn</h2>
        <div style="float: left; width: 50%;">
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
                        <telerik:GridBoundColumn HeaderText="Tên" DataField="Name" SortExpression="Name" />
                        <telerik:GridBoundColumn HeaderText="Khu vực" DataField="AreaName" SortExpression="AreaName" />
                        <telerik:GridBoundColumn HeaderText="#" DataField="ValidStatus" SortExpression="ValidStatus" />
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px" AllowFiltering="False">
                            <HeaderTemplate>
                                <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/Content/icons/new.png" ToolTip="Tạo mới"
                                    CommandName="cmdInsert" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/icons/edit.png" ToolTip="Sửa"
                                    CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div style="float: right; width: 50%;">
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
                            <td style="text-align: right">Khu vực bàn</td>
                            <td>
                                <uc1:CmbTableArea runat="server" id="cmbTableArea" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Trạng thái</td>
                            <td><uc1:CmbValidStatus runat="server" id="cmbValidStatus" /></td>
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
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
