<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductViewerControl.ascx.cs" Inherits="Vanct.WebApp.UserControls.ProductViewerControl" %>
<div style="position: relative;">
    <asp:Repeater runat="server" ID="repeater">
        <ItemTemplate>
            <a href="<%#ResolveUrl("~/Product.aspx?Id=" + Eval("Id")) %>">
                <div class="product _left" data-hint="<%#Eval("Note") %>">
                    <div class="image">
                        <img alt="" src="<%#ResolveUrl("~/UploadManage/ProductImages/" + Eval("ImageUrl")) %>" />
                    </div>
                    <div class="name"><%#Eval("Name") %></div>
                </div>
            </a>
        </ItemTemplate>
    </asp:Repeater>
    <div class="_clr"></div>
    <div style="position: absolute; height: 1px; width: 100%; bottom: 0; left: 0; background-color: #fff;"></div>
    <div style="position: absolute; height: 100%; width: 1px; top: 0; right: 0; background-color: #fff;"></div>
</div>

