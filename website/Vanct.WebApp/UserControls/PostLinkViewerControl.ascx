<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostLinkViewerControl.ascx.cs" Inherits="Vanct.WebApp.UserControls.PostLinkViewerControl" %>
<%@ Import Namespace="Hulk.Shared" %>
<div style="position: relative;">
    <asp:Repeater runat="server" ID="repeater">
        <ItemTemplate>
            <a href="<%#ResolveUrl("~/PostLinkDetails.aspx?Id=" + Eval("Id")) %>">
                <div class="postlink _left">
                    <div class="image _left">
                        <img alt="" src="<%#ResolveUrl("~/UploadManage/PostLinkImages/" + Eval("ImageUrl")) %>" />
                    </div>
                    <div class="col2 _right">
                        <div class="name" data-hint="<%#Eval("Note") %>"><%#Eval("Name").ToString().Limited(60) %></div>
                        <div class="details"><%#Eval("Note").ToString().Limited(130) %></div>
                    </div>
                    <div class="_clr"></div>
                </div>
            </a>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <a href="<%#ResolveUrl("~/PostLinkDetails.aspx?Id=" + Eval("Id")) %>">
                <div class="postlink _left">
                    <div class="col2 _left">
                        <div class="name" data-hint="<%#Eval("Note") %>"><%#Eval("Name").ToString().Limited(60) %></div>
                        <div class="details"><%#Eval("Note").ToString().Limited(130) %></div>
                    </div>
                    <div class="image _right">
                        <img alt="" src="<%#ResolveUrl("~/UploadManage/PostLinkImages/" + Eval("ImageUrl")) %>" />
                    </div>
                    <div class="_clr"></div>
                </div>
            </a>
        </AlternatingItemTemplate>
    </asp:Repeater>
    <div class="_clr"></div>
    <div style="position: absolute; height: 1px; width: 100%; bottom: 0; left: 0; background-color: #fff;"></div>
    <div style="position: absolute; height: 100%; width: 1px; top: 0; right: 0; background-color: #fff;"></div>
</div>