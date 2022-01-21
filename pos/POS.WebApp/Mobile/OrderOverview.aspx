<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/MobileMaster.Master" AutoEventWireup="true" CodeBehind="OrderOverview.aspx.cs" Inherits="POS.WebApp.Mobile.OrderOverview" %>
<%@ Import Namespace="POS.WebApp.AppCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <div class="toolbar">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <button class="btn btn-success btn-lg" type="button" onclick="gotoPage('<%=ResolveUrl("~/Mobile/Default.aspx") %>')">
                    <i class="fa fa-refresh" aria-hidden="true"></i>&nbsp;Làm mới
                </button>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 text-right">
                <button class="btn btn-warning btn-lg" type="button" onclick="gotoPage('<%=ResolveUrl("~/ChangePassword.aspx") %>')">
                    <i class="fa fa-key" aria-hidden="true"></i>&nbsp;Đổi mật khẩu
                </button>
                <button class="btn btn-primary btn-lg" type="button" onclick="logout()">
                    <i class="fa fa-sign-out" aria-hidden="true"></i>&nbsp;Kết ca
                </button>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Blue" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div style="display: none;">
            <asp:Button runat="server" ID="btnLogout" OnClick="BtnLogout" />
        </div>
        <div class="divTables">
            <asp:Repeater runat="server" ID="gridAreas" OnItemDataBound="GridAreaItemDataBound">
                <ItemTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading"><b><%#Eval("Name") %></b></div>
                        <telerik:RadListView ID="gridTables" runat="server" ItemPlaceholderID="tableContainer">
                            <LayoutTemplate>
                                <div class="panel-body">
                                    <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                                    <div class="clr"></div>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class='table <%#Eval("Order") != null ? ((int)Eval("Order.SessionId") == PosContext.User.Session.Id ? Eval("Order.DisplayCss") : "locked") : ""%>' onclick="gotoPage('<%#ResolveUrl("~/Mobile/OrderDetails.aspx?id=" + Eval("Id")) %>')">
                                    <div><b><%#Eval("Name") %></b></div>
                                    <div class="orange"><b><%#Eval("Order.TotalOrderText") %></b></div>
                                    <div class="blue"><b><%#Eval("Order.StartTimeText") %></b></div>
                                </div>
                            </ItemTemplate>
                        </telerik:RadListView>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script>
        function logout() {
            if (confirm("Bạn muốn kết ca?")) {
                <%=ClientScript.GetPostBackClientHyperlink(btnLogout, "") %>
            }
        }

        function gotoPage(url) {
            window.location = url;
        }

        function myFunction(n) {
            alert("Đang có " + n + " đơn hàng chưa xử lý xong, không thể kết ca!");
        }

        $('.divTables').height(window.WindownHeight - 52);
    </script>
</asp:Content>
