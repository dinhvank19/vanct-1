<%@ Page Title="" Language="C#" MasterPageFile="~/Bep/BepLayout.Master" AutoEventWireup="true" CodeBehind="Ban.aspx.cs" Inherits="POS.LocalWeb.Bep.Ban" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-none">
        <asp:Button runat="server" ID="btnDaDoc" OnClick="OnBtnDaDoc" />
        <asp:Button runat="server" ID="btnDaChuyen" OnClick="OnBtnDaChuyen" />
        <asp:HiddenField runat="server" ID="txtLineId" />
    </div>
    <table class="table table-striped">
        <telerik:RadListView ID="gridLines" runat="server" ItemPlaceholderID="tableContainer">
            <LayoutTemplate>
                <thead>
                    <tr>
                        <th>Bàn</th>
                        <th>Tên món</th>
                        <th>Số lượng</th>
                        <th>Ghi chú</th>
                        <th>Nhập món<br />
                            cách đây(Phút)</th>
                        <th>Bắt đầu làm<br />
                            cách đây(Phút)</th>
                        <th>Đang Làm</th>
                        <th>Xong</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="tableContainer" runat="server"></asp:PlaceHolder>
                </tbody>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
            </ItemTemplate>
        </telerik:RadListView>
    </table>
</asp:Content>
