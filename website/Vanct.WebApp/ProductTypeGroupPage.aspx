<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ProductTypeGroupPage.aspx.cs" Inherits="Vanct.WebApp.ProductTypeGroupPage" %>
<%@ Register src="~/UserControls/ProductViewerControl.ascx" tagname="ProductViewerControl" tagprefix="uc1" %>
<%@ Register src="~/UserControls/MenuProductControl.ascx" tagname="MenuProductControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3><asp:Literal runat="server" ID="lblTitle"/></h3>
    <div style="width: 724px;" class="_left">
        <uc1:ProductViewerControl ID="productViewer" runat="server" />
    </div>
    <div style="width: 252px;" class="_right">
        <uc1:MenuProductControl ID="MenuProductControl1" runat="server" />        
    </div>
    <div class="_clr"></div>
</asp:Content>
