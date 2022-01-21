<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PostLinkDetails.aspx.cs" Inherits="Vanct.WebApp.PostLinkDetails" %>

<%@ Import Namespace="Vanct.WebApp.AppCode" %>
<%@ Register Src="~/UserControls/PostlinkViewerControl.ascx" TagName="PostlinkViewerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/MenuProductControl.ascx" TagName="MenuProductControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        <asp:Literal runat="server" ID="lblTitle" /></h3>
    <div style="width: 724px;" class="_left">
        <div class="_line"></div>
        <br />
        <div>
            <asp:Literal runat="server" ID="lblOverview" /></div>
        <br />
        <div>
            <asp:Literal runat="server" ID="lblDescription" /></div>
        <div class="_clr"></div>
        <br />
        <h3>
            <asp:Literal runat="server" ID="lblTitleSame" />
            khác</h3>
        <uc1:PostlinkViewerControl ID="viewer" runat="server" />
        <br />
        <h3>Facebook</h3>
        <div class="fb-comments" data-href="http://vanct.com/PostLinkDetails.aspx?Id=<%=VanctContext.RequestId %>" data-numposts="5" data-colorscheme="light"></div>
    </div>
    <div style="width: 252px;" class="_right">
        <uc1:MenuProductControl ID="MenuProductControl1" runat="server" />
    </div>
    <div class="_clr"></div>
</asp:Content>
