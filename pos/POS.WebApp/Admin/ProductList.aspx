<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="POS.WebApp.Admin.ProductList" %>

<%@ Register Src="~/UserControls/CmbValidStatus.ascx" TagPrefix="uc1" TagName="CmbValidStatus" %>
<%@ Register Src="~/UserControls/CmbProductGroup.ascx" TagPrefix="uc1" TagName="CmbProductGroup" %>
<%@ Register Src="~/UserControls/CmbProductOm.ascx" TagPrefix="uc1" TagName="CmbProductOm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <h2>Thực đơn - Món</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" OnClientLoaded="loadSplitter">
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="Y">
                <div style="margin: -1px">
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
                                <telerik:GridBoundColumn HeaderText="Nhóm" DataField="GroupName" SortExpression="GroupName" />
                                <telerik:GridBoundColumn HeaderText="Giá" DataField="Price" SortExpression="Price" />
                                <telerik:GridBoundColumn HeaderText="ĐVT" DataField="ProductOm" SortExpression="ProductOm" />
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
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="None" />
            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="Y" Width="442">
                <asp:Panel runat="server" ID="panCrud" Visible="False">
                    <h3 style="margin: 0 0 20px 0;">
                            <asp:Literal runat="server" ID="lblTitle" /></h3>
                        <asp:Label runat="server" ID="lblMessage" ForeColor="red" />
                        <table>
                            <tr>
                                <td style="text-align: right">Tên</td>
                                <td>
                                    <telerik:RadTextBox ID="txtName" runat="server" Width="300" /></td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Nhóm thực đơn</td>
                                <td>
                                    <uc1:CmbProductGroup runat="server" ID="cmbProductGroup" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Đơn vị tính</td>
                                <td>
                                    <uc1:CmbProductOm runat="server" ID="cmbProductOm" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Giá</td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txtPrice" runat="server" NumberFormat-DecimalDigits="0"
                                        MinValue="0" DataType="System.Double" NumberFormat="{0:0.00}" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Giảm giá</td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txtDiscount" runat="server" MaxLength="3" Width="90"
                                        ShowSpinButtons="true" MaxValue="100" MinValue="0" DataType="System.Int32"
                                        NumberFormat-DecimalDigits="0"
                                        Type="Percent" /></td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Ghi chú</td>
                                <td>
                                    <telerik:RadTextBox ID="txtDescription" runat="server" Width="300" TextMode="MultiLine" Height="80" /></td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Trạng thái</td>
                                <td>
                                    <uc1:CmbValidStatus runat="server" ID="cmbValidStatus" />
                                </td>
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
                </asp:Panel>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </telerik:RadAjaxPanel>
    <script>
        function loadSplitter(sender) {
            sender.set_height(window.WindownHeight - 90);
        }
    </script>
</asp:Content>
