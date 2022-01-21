<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Vanct.WebApp.Product" %>

<%@ Import Namespace="Vanct.WebApp.AppCode" %>
<%@ Register Src="~/UserControls/MenuProductControl.ascx" TagName="MenuProductControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/ProductViewerControl.ascx" TagName="ProductViewerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .product-image {
            max-width: 200px;
            border: 1px double #ccc;
            margin: 5px;
        }
    </style>
    <div style="width: 724px;" class="_left">
        <h3>
            <asp:Literal runat="server" ID="lblTitle" /></h3>
        <div class="_line"></div>
        <br />
        <div style="width: 200px;" class="_left">
            <asp:Image runat="server" ID="image" CssClass="product-image" />
        </div>
        <div style="width: 500px; margin: 7px 10px 0 0;" class="_right">
            <p>Loại:
                <asp:Literal runat="server" ID="lblGroup" /></p>
            <p style="display: none">Bảo hành:
                <asp:Literal runat="server" ID="lblWarranty" /></p>
            <p>
                <asp:Literal runat="server" ID="lblNote" /></p>
            <p style="color: #ff002b">
                <asp:Literal runat="server" ID="lblPriceVnd" /></p>
        </div>
        <div class="_clr"></div>

        <h3>Mô tả chi tiết</h3>
        <div class="_line"></div>
        <br />
        <div style="position: relative">
            <asp:Literal runat="server" ID="lblDescription" />
        </div>
        <div class="_clr"></div>
        <br />
        <h3>Sản phẩm khác</h3>
        <div class="_line"></div>
        <uc1:ProductViewerControl ID="productViewer" runat="server" />
        <br />
        <h3>Facebook</h3>
        <div class="fb-comments" data-href="http://vanct.com/Product.aspx?Id=<%=VanctContext.RequestId %>" data-numposts="5" data-colorscheme="light"></div>
    </div>
    <div style="width: 252px;" class="_right">
        <uc1:MenuProductControl ID="MenuProductControl1" runat="server" />
    </div>
    <div class="_clr"></div>
</asp:Content>
