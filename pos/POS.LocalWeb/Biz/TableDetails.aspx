<%@ Page Title="" Language="C#" MasterPageFile="~/Biz/Site1.Master" AutoEventWireup="true" CodeBehind="TableDetails.aspx.cs" Inherits="POS.LocalWeb.Biz.TableDetails" %>

<%@ Import Namespace="POS.LocalWeb.AppCode" %>
<%@ Import Namespace="POS.LocalWeb.Dal" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div class="divHeader shadow d-flex p-2">
            <asp:Panel runat="server" ID="changeTablePanel">
                <div class="d-flex">
                    <button type="button" class="btn me-1 p-2 btn-info" onclick="selectingTableForChange()">
                        <span class="fa fa-exchange"></span>
                        <span class="ms-1">Chuyển bàn</span>
                    </button>
                    <button type="button" class="btn me-1 p-2 btn-danger" onclick="cancelChangeTable()">
                        <span class="fa fa-ban"></span>
                        <span class="ms-1">Huỷ</span>
                    </button>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="buttonsPanel">
                <div class="d-flex">
                    <button type="button" class="btn me-1 p-2 btn-info" onclick="changeTable()">
                        <span class="fa fa-exchange"></span>
                        <span class="ms-1">C.bàn</span>
                    </button>
                    <button type="button" class="btn me-1 p-2 btn-warning" onclick="printTemporaryOrder()">
                        <span class="fa fa-print"></span>
                        <span class="ms-1">Bill</span>
                    </button>
                    <button type="button" class="btn me-1 p-2 btn-primary" onclick="printOrder();">
                        <span class="fa fa-print"></span>
                        <span class="ms-1">Bếp</span>
                    </button>
                    <button type="button" class="btn me-1 p-2 btn-success" onclick="goProducts()">
                        <span class="fa fa-plus"></span>
                        <span class="ms-1">Món</span>
                    </button>
                    <%
                        if (new AceDbContext().IsRefundable())
                        {
                    %>
                    <button type="button" class="btn me-1 p-2 btn-danger" onclick="goProductsButRefund()">
                        <span class="fa fa-minus"></span>
                        <span class="ms-1">Món</span>
                    </button>
                    <%
                        }
                    %>
                </div>
            </asp:Panel>
        </div>
        <div class="divDetails">
            <div class="panel panel-danger">
                <div class="panel-heading d-flex p-3">
                    <button type="button" class="btn btn-danger" onclick="goBack();">
                        <span class="fa fa-chevron-left"></span>
                    </button>
                    <div class="ms-2">
                        <div class="fw-bolder">
                            Bàn <asp:Literal runat="server" ID="lblTableNo" /> - [<asp:Literal runat="server" ID="lblMoment" />]
                        </div>
                        <div class="fw-bolder">
                            Check In: <asp:Literal runat="server" ID="lblCheckIn" />
                        </div>
                    </div>
                </div>
                <table class="table">
                    <telerik:RadListView ID="gridLines" runat="server" ItemPlaceholderID="tableContainer">
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class="dadoc-<%#Eval("DaDoc") %> dachuyen-<%#Eval("DaChuyen") %>">
                                <td style="width: 20px; vertical-align: middle; padding-right: 0">
                                    <%
                                        if (PosContext.RequestChangeTable)
                                        {
                                    %>
                                    <span class="order-line fa fa-check-square-o" data-id="<%#Eval("Id") %>"></span>
                                    <%
                                        }
                                        else
                                        {
                                    %>
                                    <button type="button" class="btn btn-danger" onclick="deleteProduct('<%#Eval("Id") %>');">
                                        <span class="fa fa-times"></span>
                                    </button>
                                    <%
                                        }
                                    %>
                                </td>
                                <td class='<%#Eval("PrintCss") %>' id="<%#Eval("Id") %>">
                                    <div class="name"><%#Eval("ProductName") %> (<%#Eval("Moment") %>)</div>
                                    <div class="text-right"><%#Eval("Amout") %> x <%#Eval("PriceText") %> = <%#Eval("TotalText") %></div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </telerik:RadListView>
                    <tr>
                        <td></td>
                        <td class="total text-right">Total:
                   
                            <asp:Literal runat="server" ID="lblTotal" /></td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="display: none;">
            <asp:HiddenField runat="server" ID="txtDeleteOrderLineId" />
            <asp:Button runat="server" ID="btnPrintOrder" OnClick="BtnPrintOrder" />
            <asp:Button runat="server" ID="btnPrintTemporaryOrder" OnClick="BtnTemporaryPrintOrder" />
            <asp:Button runat="server" ID="btnDeleteProduct" OnClick="BtnDeleteProduct" />
            <asp:Button runat="server" ID="btnPerformChangeTable" OnClick="BtnPerformChangeTable" />
        </div>
        <div id="changeTableModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <h4 class="modal-title">Chuyển đến bàn nào ?</h4>
                        <asp:HiddenField runat="server" ID="txtMoveToNewTableOrderLineSelectedIDs" />
                        <asp:DropDownList runat="server" ID="ddlChangedToTableId" CssClass="form-control" />
                        <button type="button" class="btn btn-success" onclick="performChangeTable()">
                            OK
                       
                        </button>
                        <button type="button" class="btn btn-danger" onclick="closeChangeTableModal()">
                            Đóng
                       
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
    <script>
        function closeChangeTableModal() {
            $('#changeTableModal').modal('hide');
        }
        function performChangeTable() {
            var selectedIDs = [];
            $('.order-line.fa.fa-check-square-o').each(function (index, element) {
                selectedIDs.push("'" + $(element).data('id') + "'");
            });

            $('#<%=txtMoveToNewTableOrderLineSelectedIDs.ClientID %>').val(selectedIDs.join(','));
            <%=ClientScript.GetPostBackClientHyperlink(btnPerformChangeTable, "") %>
        }
        function selectingTableForChange() {
            if ($('.order-line.fa.fa-check-square-o').length === 0) {
                alert('Bạn chưa chọn món');
            } else {
                $('#changeTableModal').modal({ backdrop: 'static', keyboard: false });
            }
        }
        function changeTable() {
            window.location = '<%=ResolveUrl("~/Biz/TableDetails.aspx?changeTable=true&no=" + PosContext.RequestTableNo) %>';
        }
        function cancelChangeTable() {
            window.location = '<%=ResolveUrl("~/Biz/TableDetails.aspx?no=" + PosContext.RequestTableNo) %>';
        }
        function goBack() {
            window.location = '<%=ResolveUrl("~/Biz/ListTable.aspx") %>';
        }
        function goProducts() {
            window.location = '<%=ResolveUrl("~/Biz/ListProduct.aspx?no=" + PosContext.RequestTableNo) %>';
        }
        function goProductsButRefund() {
            window.location = '<%=ResolveUrl("~/Biz/ListProduct.aspx?refund=true&no=" + PosContext.RequestTableNo) %>';
        }
        function printOrder() {
            <%=ClientScript.GetPostBackClientHyperlink(btnPrintOrder, "") %>
        }
        function printTemporaryOrder() {
            <%=ClientScript.GetPostBackClientHyperlink(btnPrintTemporaryOrder, "") %>
        }
        function deleteProduct(productId) {
            if ($('#' + productId).hasClass('deleting')) {
                if (confirm("Xóa món này")) {
                    $('#<%=txtDeleteOrderLineId.ClientID %>').val(productId);
                    <%=ClientScript.GetPostBackClientHyperlink(btnDeleteProduct, "") %>
                }
            }
        }
        $('.order-line.fa').on('click', function (e) {
            var checkbox = $(this);
            if (checkbox.hasClass('fa-square-o')) {
                checkbox.addClass('fa-check-square-o').removeClass('fa-square-o');
            } else {
                checkbox.addClass('fa-square-o').removeClass('fa-check-square-o');
            }
        });
    </script>
</asp:Content>
