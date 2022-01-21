<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Vanct.WebApp.Register" %>
<%@ Register Src="~/UserControls/MenuProductControl.ascx" TagName="MenuProductControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Đăng ký</h3>
    <div style="width: 724px;" class="_left">
        <div class="_line"></div>
        <br />
        <telerik:RadAjaxLoadingPanel runat="server" ID="loading"/>
        <telerik:RadAjaxPanel runat="server" LoadingPanelID="loading">
            <table>
                <tr>
                    <td style="text-align: right">Tên doanh nghiệp <span class="_important">(*)</span></td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCompany" Width="500" /></td>
                </tr>
                <tr>
                    <td style="text-align: right">Tên liên hệ <span class="_important">(*)</span></td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtContactName" Width="500" /></td>
                </tr>
                <tr>
                    <td style="text-align: right">Số điện thoại <span class="_important">(*)</span></td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtContactPhone" Width="500" /></td>
                </tr>
                <tr>
                    <td style="text-align: right">Email</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtContactEmail" Width="500"/></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top">Nội dung <span class="_important">(*)</span></td>
                    <td>
                        <telerik:RadEditor runat="server" ID="txtDescription" Width="500" Skin="Metro" EditModes="Design" Height="300">
                            <Tools>
                                <telerik:EditorToolGroup>
                                    <telerik:EditorTool Name="Cut" />
                                    <telerik:EditorTool Name="Copy" />
                                    <telerik:EditorTool Name="Paste" />
                                </telerik:EditorToolGroup>
                            </Tools>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <telerik:RadButton ID="RadButton1" OnClick="BtnSendClicked" runat="server" Text="Gửi"/>
                        <telerik:RadButton ID="RadButton2" OnClick="BtnResetClicked" runat="server" Text="Làm lại"/>
                        <br/>
                        <asp:Label runat="server" ForeColor="red" ID="lblMessage"/>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </div>
    <div style="width: 252px;" class="_right">
        <uc1:MenuProductControl ID="MenuProductControl1" runat="server" />
    </div>
    <div class="_clr"></div>
</asp:Content>
