<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavProductControl.ascx.cs" Inherits="Vanct.WebApp.UserControls.NavProductControl" %>
<ul class="dropdown-menu dark" data-role="dropdown">
    <asp:Repeater runat="server" ID="repeater" OnItemDataBound="RepeaterItemDataBound">
        <ItemTemplate>
            <li><a class="dropdown-toggle" href="<%#ResolveUrl("~/ProductTypeGroupPage.aspx?Id=" + Eval("Id")) %>"><%#Eval("Name") %></a>
                <ul class="dropdown-menu dark" data-role="dropdown">
                    <asp:Repeater runat="server" ID="subRepeater" OnItemDataBound="SubRepeaterItemDataBound">
                        <ItemTemplate>
                            <li style="width: 250px;"><a href="<%#ResolveUrl("~/ProductTypePage.aspx?Id=" + Eval("Id")) %>"><%#Eval("Name") %></a></li>
                            <asp:Literal runat="server" ID="lblDivider"><li class="divider"></li></asp:Literal>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </li>
            <asp:Literal runat="server" ID="lblDivider"><li class="divider"></li></asp:Literal>
        </ItemTemplate>
    </asp:Repeater>
</ul>