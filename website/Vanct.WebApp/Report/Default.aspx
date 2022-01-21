<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Vanct.WebApp.Report.Default" %>

<%@ Register Src="~/Report/LoginPart.ascx" TagPrefix="Custom" TagName="LoginPart" %>
<%@ Register Src="~/Report/ReportPart.ascx" TagPrefix="Custom" TagName="ReportPart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="rightInfoContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <Custom:LoginPart runat="server" id="LoginControl" Visible="False" />
    <Custom:ReportPart runat="server" id="ReportPart" Visible="False" />
</asp:Content>
