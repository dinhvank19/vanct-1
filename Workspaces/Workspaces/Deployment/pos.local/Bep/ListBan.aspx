<%@ Page Title="" Language="C#" MasterPageFile="~/Bep/BepLayout.Master" AutoEventWireup="true" CodeBehind="ListBan.aspx.cs" Inherits="POS.LocalWeb.Bep.ListBan" %>

<%@ Import Namespace="POS.LocalWeb.AppCode" %>
<%@ Import Namespace="POS.LocalWeb.Dal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadListView ID="gridTables" runat="server" ItemPlaceholderID="tableContainer">
        <LayoutTemplate>
            <div class="ps-3 pt-3 d-flex flex-wrap">
                <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="btn ban p-2 me-3 mb-3 fw-bolder <%#Eval("Css") %>" onclick="gotoDetails('<%#Eval("TableNo") %>')">
                <div class="name">[<%#Eval("TableNo") %>]</div>
                <div class="moment"><%#Eval("Moment") %></div>
            </div>
        </ItemTemplate>
    </telerik:RadListView>
    <script>
        function gotoDetails(id) {
            window.location = '<%=ResolveUrl("~/Bep/Ban.aspx") %>?no=' + id;
        }

        setTimeout(function () {
            window.location.reload(true)
        }, <%= new AceDbContext().RefreshInMins() %> * 1000)
    </script>
</asp:Content>
