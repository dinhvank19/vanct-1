<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuProductControl.ascx.cs" Inherits="Vanct.WebApp.UserControls.MenuProductControl" %>
<asp:Repeater runat="server" ID="repeater" OnItemDataBound="RepeaterItemDataBound">
    <ItemTemplate>
        <div class="panel">
            <div class="panel-header bg-active-yellow">
                <a href="<%#ResolveUrl("~/ProductTypeGroupPage.aspx?Id=" + Eval("Id")) %>"><%#Eval("Name") %></a>
            </div>
            <div class="panel-content">
                <asp:Repeater runat="server" ID="subRepeater">
                    <ItemTemplate>
                        <div><a href="<%#ResolveUrl("~/ProductTypePage.aspx?Id=" + Eval("Id")) %>">&raquo; <%#Eval("Name") %></a></div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <br />
    </ItemTemplate>
</asp:Repeater>
<div class="panel">
    <div class="panel-header bg-active-yellow">
        <a href="<%=ResolveUrl("~/PostLinkPage.aspx?n=tin-tuc") %>">Tin Tức</a>
    </div>
    <div class="panel-content">
        <asp:Repeater runat="server" ID="repeaterTinTuc">
            <ItemTemplate>
                <div>
                    <a href="<%#ResolveUrl("~/PostLinkDetails.aspx?Id=" + Eval("Id")) %>">&raquo; <%#Eval("Name") %></a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<br />
<div class="panel">
    <div class="panel-header bg-active-yellow">
        <a href="<%=ResolveUrl("~/PostLinkPage.aspx?n=giai-phap") %>">Giải Pháp</a>
    </div>
    <div class="panel-content">
        <asp:Repeater runat="server" ID="repeaterGiaiPhap">
            <ItemTemplate>
                <div>
                    <a href="<%#ResolveUrl("~/PostLinkDetails.aspx?Id=" + Eval("Id")) %>">&raquo; <%#Eval("Name") %></a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<br />
<div class="panel">
    <div class="panel-header bg-active-yellow">
        <a href="<%=ResolveUrl("~/PostLinkPage.aspx?n=khach-hang") %>">Khách Hàng</a>
    </div>
    <div class="panel-content">
        <div id="scroll">
            <telerik:RadRotator ID="logoRepeater" runat="server" Height="390" Width="230"
                ItemWidth="230" ItemHeight="130" ScrollDuration="500" ScrollDirection="Down">
                <ItemTemplate>
                    <a href="<%#ResolveUrl("~/PostLinkDetails.aspx?Id=" + Eval("Id")) %>">
                        <img style="max-height: 120px;" alt="" src="<%#ResolveUrl("~/UploadManage/PostLinkImages/" + Eval("ImageUrl")) %>" />
                    </a>
                </ItemTemplate>
            </telerik:RadRotator>
        </div>
    </div>
</div>
<br />
<div class="panel">
    <div class="panel-header bg-active-yellow">
        Hổ trợ trực tuyến
    </div>
    <div class="panel-content">
        <asp:Repeater runat="server" ID="repeaterSupportOnline">
            <ItemTemplate>
                <div>
                    <a href="skype:<%#Eval("Skype") %>?chat">
                        <div class="command-button bg-blue fg-white">
                            <i class="icon-skype on-left"></i>
                        </div>
                    </a>
                    <a href="ymsgr:sendim?<%#Eval("Yahoo") %>">
                        <div class="command-button bg-darkPink fg-white">
                            <i class="icon-yahoo on-right"></i>
                        </div>
                    </a><a target="_blank" href="<%#Eval("Facebook") %>">
                        <div class="command-button bg-lightBlue fg-white">
                            <i class="icon-facebook on-right"></i>
                        </div>
                    </a>
                </div>
                <div style="margin: 4px 0 10px 0;">
                    <a href="mailto:<%#Eval("Email") %>">
                        <div class="command-button bg-orange fg-white">
                            <i class="icon-mail on-right"></i>
                        </div>
                    </a>
                    <div class="command-button bg-red fg-white">
                        <i class="icon-phone on-right"></i>
                        <%#Eval("Hotline") %>
                        <small>Hãy gọi cho tôi</small>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<br />
<div class="panel">
    <div class="panel-header bg-active-yellow">
        Thống kê truy cập
    </div>
    <div class="panel-content">
        <div>Số lượt truy cập: <b><%= Application["CountVisit"].ToString() %></b></div>
        <div>Số người trực tuyến: <b><%= Application["Online"].ToString() %></b></div>
    </div>
</div>
<br/>
<div class="fb-like-box" data-href="https://www.facebook.com/acesofts" data-colorscheme="light" data-show-faces="true" data-header="true" data-stream="false" data-show-border="true" width="252"></div>
