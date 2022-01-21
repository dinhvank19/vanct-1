<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Download.aspx.cs" Inherits="Vanct.WebApp.Download" %>
<%@ Register Src="~/UserControls/MenuProductControl.ascx" TagName="MenuProductControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Tải về</h3>
    <div style="width: 724px;" class="_left">
        <div class="_line"></div>
        <br />
        <asp:Repeater runat="server" ID="repeater">
            <ItemTemplate>
                <div><a target="_blank" href="<%#ResolveUrl("~/UploadManage/BaseFileFolder/" + Eval("FilePath")) %>">&raquo; <%#Eval("Name") %></a></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div style="width: 252px;" class="_right">
        <uc1:MenuProductControl ID="MenuProductControl1" runat="server" />
    </div>
    <div class="_clr"></div>
</asp:Content>
