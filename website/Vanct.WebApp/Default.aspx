<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Vanct.WebApp.Default" %>
<%@ Import Namespace="Vanct.WebApp.AppCode" %>

<%@ Register Src="~/UserControls/ProductViewerControl.ascx" TagName="ProductViewerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PostlinkViewerControl.ascx" TagName="PostlinkViewerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/HomeGalleryControl.ascx" TagName="HomeGalleryControl" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/MenuProductControl.ascx" tagname="MenuProductControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 330px;" class="_left">
        <a href="<%=ResolveUrl("~/ProductTypeGroupPage.aspx?Id=1") %>">
            <div class="_my-tile bg-lightBlue _left">
                <i class="icon-box"></i>
                <div class="name">Phần mềm</div>
            </div>
        </a>
        <a href="<%=ResolveUrl("~/ProductTypeGroupPage.aspx?Id=2") %>">
            <div class="_my-tile bg-green _left">
                <i class="icon-printer"></i>
                <div class="name">Thiết bị</div>
            </div>
        </a>
        <a href="<%=ResolveUrl("~/PostLinkPage.aspx?n=giai-phap") %>">
            <div class="_my-tile bg-yellow _left">
                <i class="icon-snowy-4"></i>
               <div class="name">Giải Pháp</div>
            </div>
        </a>
        <a href="<%=ResolveUrl("~/PostLinkPage.aspx?n=tin-tuc") %>">
            <div class="_my-tile bg-orange _left">
                <i class="icon-newspaper"></i>
                <div class="name">Tin Tức</div>
            </div>
        </a>
        <a href="<%=ResolveUrl("~/Download.aspx") %>">
            <div class="_my-tile bg-red _left">
                <i class="icon-download"></i>
                <div class="name">Tải Về</div>
            </div>
        </a>
        <a href="<%=ResolveUrl("~/TopicPage.aspx?n=lien-he") %>">
            <div class="_my-tile bg-teal _left">
                <i class="icon-phone"></i>
                <div class="name">Liên Hệ</div>
            </div>
        </a>
    </div>
    <div style="width: 650px;" class="_right">
        <uc1:HomeGalleryControl ID="HomeGalleryControl1" runat="server" />
    </div>
    <div class="_clr"></div>
    <div style="width: 724px;" class="_left">
        <div class="fg-black _title">Sản Phẩm</div>
        <uc1:ProductViewerControl ID="productViewer" runat="server" ProductHome="True" />
        
        <asp:Repeater runat="server" ID="repeater" OnItemDataBound="RepeaterItemDataBound">
            <ItemTemplate>
                <div style="" class="fg-black _title">
                    <%#VanctContext.Translater.Translate(Eval("Id").ToString()) %>
                </div>
                <uc1:PostlinkViewerControl ID="postlinkView" runat="server" PostlinkHome="True" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div style="width: 252px;" class="_right">
        <uc1:MenuProductControl ID="MenuProductControl1" runat="server" />        
    </div>
    <div class="_clr"></div>
    
</asp:Content>
