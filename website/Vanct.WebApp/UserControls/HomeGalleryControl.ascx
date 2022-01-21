<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeGalleryControl.ascx.cs" Inherits="Vanct.WebApp.UserControls.HomeGalleryControl" %>
<div class="carousel" id="carousel2">
    <asp:Repeater runat="server" ID="repeater">
        <ItemTemplate>
            <div class="slide">
                <a href="<%#Eval("Link") %>"><img src="<%#ResolveUrl("~/UploadManage/HomeGallery/" + Eval("ImageUrl")) %>"/></a>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<script>
    $(function () {
        $("#carousel2").carousel({
            height: 300,
            effect: 'slowdown',
            period: 3000,
            markers: {
                show: true,
                type: 'square',
                position: 'bottom-right'
            }
        });
    })
</script>