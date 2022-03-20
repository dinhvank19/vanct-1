<%@ Page Title="" Language="C#" MasterPageFile="~/Biz/Site1.Master" AutoEventWireup="true" CodeBehind="ListProduct.aspx.cs" Inherits="POS.LocalWeb.Biz.ListProduct" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="body" runat="server">
    <div class="divProductGroups shadow">
        <telerik:RadListView ID="gridProductGroups" runat="server" ItemPlaceholderID="groupsContainer">
            <LayoutTemplate>
                <table>
                    <tr>
                        <asp:PlaceHolder ID="groupsContainer" runat="server"></asp:PlaceHolder>
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <td>
                    <button type="button" class="btn btn-warning btn-product-group" onclick="onClickProductGroup('<%#Eval("Id") %>')">
                        <%#Eval("Name") %>
                    </button>
                </td>
            </ItemTemplate>
        </telerik:RadListView>
    </div>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div class="divHeader">
            <table>
                <tr>
                    <td>
                        <button type="button" class="btn btn-danger" onclick="goBack();">
                            <span class="fa fa-chevron-left"></span>
                            Bàn
                            <asp:Literal runat="server" ID="lblTableNo" />
                        </button>
                    </td>
                    <td>
                        <asp:TextBox runat="server" placeholder="Món" Width="100%" CssClass="form-control" ID="txtSearch" />
                    </td>
                    <td style="width: 50px;">
                        <button type="button" class="btn btn-success" onclick="search();">
                            <span class="badge"><asp:Literal runat="server" ID="lblOrderLinesCounter" /></span>
                            <span class="fa fa-search"></span>
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel runat="server" ID="panelList">
            <telerik:RadListView ID="gridProducts" runat="server" ItemPlaceholderID="productsContainer">
                <LayoutTemplate>
                    <div class="divProducts" id="divProducts">
                        <asp:PlaceHolder ID="productsContainer" runat="server"></asp:PlaceHolder>
                        <div class="clr"></div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div <%#Eval("BackgroundPhoto") %> class="product product-x<%= CurrentColumnOption %>" onclick="ShowAddProductDialog('<%#Eval("Id") %>', '<%#Eval("Name") %>')">
                        <span><%#Eval("Name") %></span>
                    </div>
                </ItemTemplate>
            </telerik:RadListView>
        </asp:Panel>
        <div style="display: none;">
            <asp:HiddenField runat="server" ID="txtProductId" />
            <asp:HiddenField runat="server" ID="txtProductGroupId" />
            <asp:Button runat="server" ID="btnSearch" OnClick="BtnSearch" />
            <asp:Button runat="server" ID="btnAddProduct" OnClick="BtnAddProduct" />
            <asp:Button runat="server" ID="btnBack" OnClick="BtnBack" />
            <asp:Button runat="server" ID="btnChangeProductGroup" OnClick="ProductGroupChanged" />
        </div>
        <div id="dialogAddProduct" class="dialogAddProduct">
            <div id="selectedProductName"></div>
            <div class="divAmount">
                <div class="amount-label">
                    <span class="label label-info">Số lượng</span>
                </div>
                <asp:TextBox runat="server" Width="100%" CssClass="form-control amount" ID="txtAmount" />
            </div>
            <div class="number-board">
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('1')">1</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('2')">2</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('3')">3</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('4')">4</button>
                </div>

                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('5')">5</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('6')">6</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('7')">7</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('8')">8</button>
                </div>

                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('9')">9</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('0')">0</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="setAmount('.')">.</button>
                </div>
                <div class="number-column">
                    <button type="button" class="btn btn-default" onclick="cleanAmount()">Xoá</button>
                </div>
                <div class="clr"></div>
            </div>
            <div>
                <asp:TextBox runat="server" ID="txtGhiChu" Width="100%" TextMode="MultiLine" CssClass="form-control note" Height="50" placeholder="Nhập ghi chú" />
            </div>
            <div class="divButtons">
                <div>
                    <button type="button" class="btn btn-default" onclick="CloseAddProductDialog();">
                        <span class="fa fa-times"></span>
                        Huỷ
                    </button>
                </div>
                <div>
                    <button type="button" class="btn btn-success" onclick="addProduct();">
                        <span class="fa fa-check"></span>
                        Thêm món
                    </button>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
    <script>
        function ShowAddProductDialog(productId, productName) {
            $('#<%=txtProductId.ClientID %>').val(productId);
            $('#selectedProductName').html(productName);
            $('#dialogAddProduct').show();
            $('#divProducts').hide();
            return false;
        }

        function CloseAddProductDialog() {
            $('#<%=txtProductId.ClientID %>').val("");
            $('#dialogAddProduct').hide();
            $('#divProducts').show();
            return false;
        }

        function addProduct() {
            if ($('#<%=txtAmount.ClientID %>').val().length === 0) {
                alert('Vui lòng nhập số lượng');
            } else {
                <%=ClientScript.GetPostBackClientHyperlink(btnAddProduct, "") %>
            }
        }

        function goBack() {
            <%=ClientScript.GetPostBackClientHyperlink(btnBack, "") %>
        }

        function search() {
            <%=ClientScript.GetPostBackClientHyperlink(btnSearch, "") %>
        }

        function onClickProductGroup(productGroupId) {
            $('#<%=txtProductGroupId.ClientID %>').val(productGroupId);
            <%=ClientScript.GetPostBackClientHyperlink(btnChangeProductGroup, "") %>
        }

        function setAmount(value) {
            var txtAmount = $('#<%=txtAmount.ClientID %>');
            txtAmount.val(txtAmount.val() + "" + value);
        }

        function cleanAmount() {
            $('#<%=txtAmount.ClientID %>').val('');
        }
    </script>
</asp:Content>
