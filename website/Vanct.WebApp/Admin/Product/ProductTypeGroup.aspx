<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ProductTypeGroup.aspx.cs" Inherits="Vanct.WebApp.Admin.Product.ProductTypeGroup" %>

<%@ Register Src="~/UserControls/CmbProductTypeGroup.ascx" TagName="CmbProductTypeGroup" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel runat="server" ID="loading"/>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" LoadingPanelID="loading">
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align: top; width: 50%;">
                    <h2>Form chỉnh sửa/thêm mới nhóm sản phẩm</h2>
                    <table>
                        <tr>
                            <td>Tên nhóm</td>
                            <td>
                                <telerik:RadTextBox ID="txtName" runat="server" Width="300" /></td>
                        </tr>
                        <tr>
                            <td>Vị trí hiển thị</td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtPosition" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="9999999" DataType="System.Int32" Value="0" /></td>
                        </tr>
                        <tr>
                            <td>Hoạt động</td>
                            <td>
                                <asp:CheckBox ID="ckIsActive" runat="server" Checked="True" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:HiddenField runat="server" ID="txtTypeGroupId" />
                                <telerik:RadButton ID="btnSaveUpdateTypeGroup" runat="server" Text="Lưu lại - chỉnh sửa" OnClick="BtnSaveUpdateTypeGroupClick" />
                                <telerik:RadButton ID="btnSaveInsertTypeGroup" runat="server" Text="Lưu lại - thêm mới" OnClick="BtnSaveInsertTypeGroupClick" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding-left: 50px; vertical-align: top;">
                    <h2>Form chỉnh sửa/thêm mới nhóm con</h2>
                    <table>
                        <tr>
                            <td>Tên nhóm con</td>
                            <td>
                                <telerik:RadTextBox ID="txtNameType" runat="server" Width="300" /></td>
                        </tr>
                        <tr>
                            <td>Nhóm sản phẩm</td>
                            <td>
                                <uc1:CmbProductTypeGroup ID="CmbProductTypeGroup" runat="server" Width="300" />
                            </td>
                        </tr>
                        <tr>
                            <td>Vị trí hiển thị</td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtPositionType" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="9999999" DataType="System.Int32" Value="0" /></td>
                        </tr>
                        <tr>
                            <td>Hiện thị trang chủ</td>
                            <td>
                                <asp:CheckBox ID="ckIsHomeShowed" runat="server" Checked="True" /></td>
                        </tr>
                        <tr>
                            <td>Hoạt động</td>
                            <td>
                                <asp:CheckBox ID="ckIsActiveType" runat="server" Checked="True" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:HiddenField runat="server" ID="txtTypeId" />
                                <telerik:RadButton ID="btnSaveUpdateType" runat="server" Text="Lưu lại - chỉnh sửa" OnClick="BtnSaveUpdateTypeClick" />
                                <telerik:RadButton ID="btnSaveInsertType" runat="server" Text="Lưu lại - thêm mới" OnClick="BtnSaveInsertTypeClick" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <h2>Nhóm sản phẩm</h2>
        <telerik:RadGrid AutoGenerateColumns="False" ID="gridTypeGroup" runat="server"
            AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False"
            PageSize="20" OnItemCommand="GridTypeGroupItemCommand" OnItemDataBound="GridTypeGroupItemDataBound">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png"
                                ToolTip="Sửa nhóm" CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Id" Visible="False" DataField="Id" SortExpression="Id" />

                    <telerik:GridBoundColumn HeaderText="Tên" HeaderStyle-Width="300px" DataField="Name" SortExpression="Name" />
                    <telerik:GridTemplateColumn HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <ul>
                                <li>Ví trí hiển thị: <%#Eval("Position") %></li>
                                <li><%#((bool)Eval("IsActive") ? "Hoạt động" : "Không hoạt động") %></li>
                            </ul>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Nhóm con" AllowFiltering="False">
                        <ItemTemplate>
                            <telerik:RadGrid AutoGenerateColumns="False" ID="gridType" runat="server"
                                AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False"
                                PageSize="20" OnItemCommand="GridTypeItemCommand">
                                <PagerStyle Mode="NextPrevAndNumeric" />
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="true" />
                                    <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                                </ClientSettings>
                                <MasterTableView DataKeyNames="Id">
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderText="Id" Visible="False" DataField="Id" SortExpression="Id" />

                                        <telerik:GridBoundColumn HeaderText="Tên" HeaderStyle-Width="300px" DataField="Name" SortExpression="Name" />

                                        <telerik:GridTemplateColumn HeaderText="Hoạt động">
                                            <ItemTemplate>
                                                <%#((bool)Eval("IsActive") ? "Có" : "Không") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Hiển thị trang chủ">
                                            <ItemTemplate>
                                                <%#((bool)Eval("IsHomeShowed") ? "Có" : "Không") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Ví trí hiển thị">
                                            <ItemTemplate>
                                                <%#Eval("Position") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Content/Themes/Icons/edit.png"
                                                    ToolTip="Sửa nhóm con" CommandName="cmdEdit" CommandArgument='<%#Eval("Id") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>



</asp:Content>
