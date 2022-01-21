<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostLinkPartnerControl.ascx.cs" Inherits="Vanct.WebApp.UserControls.PostLinkPartnerControl" %>
<div class="item-typegroup" onclick="gotoPostLinkPage('Partner')">Đối tác</div>
<asp:Repeater runat="server" ID="repeater">
    <ItemTemplate>
        <div style="margin: 9px; border: 1px solid #ccc;">
            <a target="_blank" href="<%#Eval("Link") %>">
                <img alt="" src="<%#ResolveUrl("~/UploadManage/PostLinkImages/" + Eval("ImageUrl")) %>" style="width: 220px" />
            </a>
        </div>
    </ItemTemplate>
</asp:Repeater>
