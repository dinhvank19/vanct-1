<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/MobileMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="POS.WebApp.Mobile.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Blue" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <table style="height: 500px; width: 100%">
            <tr>
                <td style="width: 30%"></td>
                <td style="width: 40%; text-align: center;">
                    <asp:Button runat="server" ID="btnMorning" CssClass="btn btn-lg btn-primary" Width="120" Text="Ca sáng" OnClick="BtnMorning" />
                    <asp:Button runat="server" ID="btnAfternoon" CssClass="btn btn-lg btn-warning" Width="120" Text="Ca chiều" OnClick="BtnAfternoon" />
                    <asp:Button runat="server" ID="btnEvening" CssClass="btn btn-lg btn-danger" Width="120" Text="Ca tối" OnClick="BtnEvening" />
                </td>
                <td style="width: 30%"></td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
