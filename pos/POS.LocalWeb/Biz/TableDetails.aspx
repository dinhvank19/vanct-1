<%@ Page Title="" Language="C#" MasterPageFile="~/Biz/Site1.Master" AutoEventWireup="true" CodeBehind="TableDetails.aspx.cs" Inherits="POS.LocalWeb.Biz.TableDetails" %>

<%@ Import Namespace="POS.LocalWeb.AppCode" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div class="divHeader shadow">
            <table>
                <tr>
                    <td>
                        <button type="button" class="btn btn-danger" onclick="goBack();">
                            <span class="fa fa-chevron-left"></span>
                        </button>
                    </td>
                    <td>
                        <asp:Panel runat="server" ID="changeTablePanel">
                            <button type="button" class="btn btn-info" onclick="selectingTableForChange()">
                                <span class="fa fa-exchange"></span>
                                Chuyển bàn
                            </button>
                            <button type="button" class="btn btn-danger" onclick="cancelChangeTable()">
                                <span class="fa fa-ban"></span>
                                Huỷ
                            </button>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="buttonsPanel">
                            <button type="button" class="btn btn-info" onclick="changeTable()">
                                <span class="fa fa-exchange"></span>
                                C.bàn
                            </button>
                            <button type="button" class="btn btn-warning" onclick="printTemporaryOrder()">
                                <span class="fa fa-print"></span> Bill
                            </button>
                            <button type="button" class="btn btn-primary" onclick="printOrder();">
                                <span class="fa fa-print"></span>
                                Bếp
                            </button>
                            <button type="button" class="btn btn-success" onclick="goProducts()">
                                <span class="fa fa-plus"></span>
                                Món
                            </button>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divDetails">
            <div class="panel panel-danger">
                <div class="panel-heading">
                    <strong>
                        Bàn <asp:Literal runat="server" ID="lblTableNo" /> - [<asp:Literal runat="server" ID="lblMoment" />]
                        <br />
                        Check In:
                        <asp:Literal runat="server" ID="lblCheckIn" />
                    </strong>
                </div>
                <table class="table table-striped">
                    <telerik:RadListView ID="gridLines" runat="server" ItemPlaceholderID="tableContainer">
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 20px; vertical-align: middle;">
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
                        <asp:DropDownList runat="server" ID="ddlChangedToTableId" CssClass="form-control"/>
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
            $('.order-line.fa.fa-check-square-o').each(function(index, element) {
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
