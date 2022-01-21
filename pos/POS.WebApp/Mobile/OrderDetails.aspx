<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/MobileMaster.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="POS.WebApp.Mobile.OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Blue" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div style="display: none;">
            <asp:HiddenField runat="server" ID="txtProductGroupId" Value="0" />
            <asp:HiddenField runat="server" ID="txtProductId" Value="0" />
            <asp:HiddenField runat="server" ID="txtAmountLine" Value="0" />
            <asp:HiddenField runat="server" ID="txtDiscountLine" Value="0" />
            <asp:HiddenField runat="server" ID="txtBillContentBase64" />
            <asp:HiddenField runat="server" ID="txtOrderLocked" />
            <asp:Button runat="server" ID="btnAddProductToOrder" OnClick="BtnAddProductToOrder" />
            <asp:Button runat="server" ID="btnPrintOrder" OnClick="BtnPrintOrder" />
            <asp:Button runat="server" ID="btnPrintBill" OnClick="BtnPrintBill" />
            <asp:Button runat="server" ID="btnComplete" OnClick="BtnComplete" />
            <asp:Button runat="server" ID="btnBack" OnClick="BtnBack" />
            <asp:Button runat="server" ID="btnCancel" OnClick="BtnCancel" />
        </div>
        <div class="toolbar">
            <table>
                <tr>
                    <td style="width: 5%; display: none;">
                        <button class="btn btn-primary btn-lg" type="button" onclick="printBill()">
                            <i class="fa fa-print" aria-hidden="true"></i>&nbsp;In Bill
                        </button>
                    </td>
                    <td style="width: 5%;">
                        <button class="btn btn-warning btn-lg" type="button" onclick="printOrder()">
                            <i class="fa fa-print" aria-hidden="true"></i>&nbsp;In Bếp
                        </button>
                    </td>
                    <td style="width: 5%; display: none;">
                        <button class="btn btn-success btn-lg" type="button" onclick="completeOrder()">
                            <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Hoàn Tất
                        </button>
                    </td>
                    <td style="display: none;">
                        <button class="btn btn-danger btn-lg" type="button" onclick="cancelOrder()">
                            <i class="fa fa-ban" aria-hidden="true"></i>&nbsp;Hủy
                        </button>
                    </td>

                    <td style="width: 5%; display: none;">
                        <div class="input-group input-group-lg">
                            <input type="text" class="form-control" placeholder="Tìm món">
                        </div>
                    </td>
                    <td style="width: 40%;">
                        <div class="input-group input-group-lg">
                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProductGroup"
                                AutoPostBack="True" OnSelectedIndexChanged="ProductGroupChanged" />
                        </div>
                    </td>
                    <td style="width: 95%;">
                        <button class="btn btn-danger btn-lg" type="button" onclick="back()">
                            <i class="fa fa-arrow-left" aria-hidden="true"></i>&nbsp;Quay lại
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" OnClientLoaded="loadSplitter">
            <telerik:RadPane ID="leftPanel" runat="server" Scrolling="None">
                <div class="panelLeft">
                    <telerik:RadListView ID="gridBill" runat="server" ItemPlaceholderID="tableContainer">
                        <LayoutTemplate>
                            <div class="divBill">
                                <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                                <div class="clr"></div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="bill-line">
                                <table>
                                    <tr>
                                        <td onclick="addProductToOrder(<%#Eval("Product.Id") %>, -1, 0)">
                                            <i class="fa fa-chevron-left fa-2x" aria-hidden="true"></i>
                                        </td>
                                        <td>
                                            <div class="name"><%#Eval("Product.Name") %></div>
                                            <div class="total">
                                                <span class="orange"><%#Eval("Amount") %></span> x <%#Eval("PriceText") %> = <span class="orange"><%#Eval("TotalText") %></span>
                                            </div>
                                        </td>
                                        <td onclick="addProductToOrder(<%#Eval("Product.Id") %>, 1, 0)">
                                            <i class="fa fa-chevron-right fa-2x" aria-hidden="true"></i>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </telerik:RadListView>
                    <div class="order-summary-bar">
                        <table>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" ID="lblTotal" /></td>
                                <td>
                                    <asp:Literal runat="server" ID="lblStartTime" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="None" />
            <telerik:RadPane ID="rightPanel" runat="server" Scrolling="None">
                <div class="panelRight">
                    <telerik:RadListView ID="gridProducts" runat="server" ItemPlaceholderID="tableContainer">
                        <LayoutTemplate>
                            <div class="divProducts">
                                <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                                <div class="clr"></div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="product" onclick="addProductToOrder(<%#Eval("Id") %>, 1, 0)">
                                <div class="orange"><%#Eval("Name") %></div>
                                <div><%#Eval("PriceText") %></div>
                            </div>
                        </ItemTemplate>
                    </telerik:RadListView>
                    <div class="products-bar">
                        <table>
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <div class="divBillPrintContent">
            <telerik:RadListView ID="gridBillPrint" runat="server">
                <ItemTemplate>
                    <div class="line">
                        <div><%#Eval("ProductName") %></div>
                        <div class="total text-right">
                            <%#Eval("Amount") %> x <%#Eval("PriceText") %> = <%#Eval("TotalText") %>
                        </div>
                    </div>
                </ItemTemplate>
            </telerik:RadListView>
            <div class="line text-right">
                <asp:Literal runat="server" ID="lblBillTotal" />
            </div>
        </div>
        <asp:Repeater runat="server" ID="gridProductGroup" OnItemDataBound="BilOrderItemDataBound">
            <ItemTemplate>
                <div class="divOrderPrintContent" id='printGroup<%#Eval("Id") %>'>
                    <div class="print-header">
                        <h3>FOOD ORDER</h3>
                        <h4><asp:Literal runat="server" ID="lblTableName" /></h4>
                        <h4><asp:Literal runat="server" ID="lblDateTime" /></h4>
                    </div>

                    <telerik:RadListView ID="gridBillPrint" runat="server" ItemPlaceholderID="tableContainer">
                        <LayoutTemplate>
                            <table>
                                <tr>
                                    <td style="width: 180px;">Tên món</td>
                                    <td style="width: 40px;">SL</td>
                                </tr>
                                <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("Product.Name") %></td>
                                <td><%#Eval("Amount") %></td>
                            </tr>
                        </ItemTemplate>
                    </telerik:RadListView>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script>
        var locked = eval($('#<%=txtOrderLocked.ClientID %>').val());
        function loadSplitter(sender) {
            var spliterHeight = (window.WindownHeight - 52);
            sender.set_height(spliterHeight);
            $('.divBill, .divProducts').height(spliterHeight - 41);
        }

        function addProductToOrder(productId, amount, discount) {
            if (locked) {
                alert("Đơn hàng này không phải của bạn !!");
                return;
            }

            $('#<%=txtProductId.ClientID %>').val(productId);
            $('#<%=txtAmountLine.ClientID %>').val(amount);
            $('#<%=txtDiscountLine.ClientID %>').val(discount);
            <%=ClientScript.GetPostBackClientHyperlink(btnAddProductToOrder, "") %>
        }

        function back() {
            <%=ClientScript.GetPostBackClientHyperlink(btnBack, "") %>
        }

        function printOrder() {
            if (locked) {
                alert("Đơn hàng này không phải của bạn !!");
                return;
            }

            $(".divOrderPrintContent").each(function(index, div) {
                html2canvas(div).then(function (canvas) {
                    var base64 = canvas.toDataURL();
                });
            });

            <%=ClientScript.GetPostBackClientHyperlink(btnPrintOrder, "") %>
        }

        function printBill() {
            if (locked) {
                alert("Đơn hàng này không phải của bạn !!");
                return;
            }

            var div = $(".divBillPrintContent")[0];
            html2canvas(div).then(function (canvas) {
                var base64 = canvas.toDataURL();
                $('#<%=txtBillContentBase64.ClientID %>').val(base64);
                <%=ClientScript.GetPostBackClientHyperlink(btnPrintBill, "") %>
            });
        }

        function completeOrder() {
            if (locked) {
                alert("Đơn hàng này không phải của bạn !!");
                return;
            }

            <%=ClientScript.GetPostBackClientHyperlink(btnComplete, "") %>
        }

        function cancelOrder() {
            if (locked) {
                alert("Đơn hàng này không phải của bạn !!");
                return;
            }

            if (confirm("Chắc chắn hủy đơn hàng?")) {
                <%=ClientScript.GetPostBackClientHyperlink(btnCancel, "") %>
            }
        }
    </script>
</asp:Content>
