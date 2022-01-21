<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="HomeGalleries.aspx.cs" Inherits="Vanct.WebApp.Admin.Others.HomeGalleries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Hình ảnh trang chủ</h2>
    <table>
        <tr>
            <td>Mô tả</td>
            <td><telerik:RadTextBox ID="txtDescription" runat="server" Width="400"/></td>
        </tr>
        <tr>
            <td>Đường dẫn</td>
            <td><telerik:RadTextBox ID="txtLink" runat="server" Width="400"/></td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Hình ảnh từ máy tính</td>
            <td style="vertical-align: top;"><telerik:RadUpload ID="imageURL" runat="server" AllowedFileExtensions=".jpg, .gif, .png" ControlObjectsVisibility="None" />
                <b>(Dài 650, Cao 300, .jpg, .gif, .png)</b></td>
        </tr>
        <tr>
            <td></td>
            <td><telerik:RadButton ID="RadButton1" OnClick="BtnSaveClicked" runat="server" Text="Lưu lại"/></td>
        </tr>
    </table>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
        <asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label>
        <telerik:RadGrid AutoGenerateColumns="False" ID="grid" runat="server"
            AllowPaging="True" AllowSorting="True" AllowFilteringByColumn="False" PageSize="20" OnItemCommand="GridItemCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="Mô tả" DataField="Description" SortExpression="Description" HeaderStyle-Width="100px" />

                    <telerik:GridBoundColumn HeaderText="Đường dẫn" DataField="Link" SortExpression="Link" HeaderStyle-Width="200px" />
                    
                    <telerik:GridTemplateColumn AllowFiltering="False"  HeaderStyle-Width="680px" HeaderText="Hình">
                        <ItemTemplate>
                            <img src='<%#ResolveUrl("~/UploadManage/HomeGallery/") + Eval("ImageUrl") %>'/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
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