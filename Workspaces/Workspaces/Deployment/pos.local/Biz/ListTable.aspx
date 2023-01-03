<%@ Page Title="" Language="C#" MasterPageFile="~/Biz/Site1.Master" AutoEventWireup="true" CodeBehind="ListTable.aspx.cs" Inherits="POS.LocalWeb.Biz.ListTable" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"/>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div class="divHeader shadow">
            <table>
                <tr>
                    <%--<td>Received:&nbsp;<asp:Literal runat="server" ID="lblTotalReceived" /></td>--%>
                    <%--<td>Current:&nbsp;<asp:Literal runat="server" ID="lblTotalCurrent" /></td>--%>
                    <td style="width: 50px;">
                        <button type="button" class="btn btn-default" onclick="showModalOptions();">
                            <span class="fa fa-bars"></span>
                        </button>
                    </td>
                    <td>
                        <span class="label label-default"><asp:Literal runat="server" ID="lblCountTable"/> Tables</span>
                        <span class="label label-danger"><asp:Literal runat="server" ID="lblCountBusy"/> Busy</span>
                        <span class="label label-warning"><asp:Literal runat="server" ID="lblTableInProgress"/> Pending</span>
                    </td>
                    <td>
                        <button type="button" class="btn btn-success" onclick="refreshData();">
                            <span class="fa fa-refresh"></span>
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <telerik:RadListView ID="gridTables" runat="server" ItemPlaceholderID="tableContainer">
            <LayoutTemplate>
                <div class="divTables">
                    <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                    <div class="clr"></div>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div class='table table-x<%= CurrentColumnOption %> <%#Eval("Css") %>' onclick="gotoDetails('<%#Eval("TableNo") %>')">
                    <span class="name">[<%#Eval("TableNo") %>]</span>
                    <span class="moment"><%#Eval("Moment") %></span>
                    <div class="total"><%#Eval("TotalText") %></div>
                </div>
            </ItemTemplate>
        </telerik:RadListView>
        <div style="display: none;">
            <asp:Button runat="server" ID="btnRefresh" OnClick="BtnRefresh"/>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
    <script>
        function gotoDetails(id) {
            window.location = '<%=ResolveUrl("~/Biz/TableDetails.aspx") %>?no=' + id;
        }

        function refreshData() {
            <%=ClientScript.GetPostBackClientHyperlink(btnRefresh, "") %>;
        }
    </script>
</asp:Content>