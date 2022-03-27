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
                    <td><%#Eval("TableNo") %></td>
                    <td><%#Eval("ProductName") %></td>
                    <td><%#Eval("Amout") %></td>
                    <td><%#Eval("GhiChu") %></td>
                    <td><%#Eval("Moment") %></td>
                    <td><%#Eval("MomentChuyen") %></td>
                    <td class="dadoc-<%#Eval("DaDoc") %>">
                        <button class="btn btn-lg btn-success pe-none btnTrue" type="button">
                            <i class="fa fa-check-square" aria-hidden="true"></i>
                            Đang làm
                        </button>
                        
                        <button class="btn btn-lg btn-primary btnFalse" type="button" onclick="markAsDaDoc('<%#Eval("Id") %>')">
                            <i class="fa fa-square" aria-hidden="true"></i>
                            Bắt đầu làm
                        </button>
                    </td>
                    <td>
                        <button class="btn btn-lg btn-primary" type="button" onclick="markAsDaChuyens('<%#Eval("Id") %>')">
                            <i class="fa fa-square" aria-hidden="true"></i>
                            Chuyển
                        </button>
                    </td>
                </tr>
            </ItemTemplate>
        </telerik:RadListView>
    </table>
    <script>
        function markAsDaDoc(id) {
            $('#<%=txtLineId.ClientID %>').val(id);
            <%=ClientScript.GetPostBackClientHyperlink(btnDaDoc, "") %>
        }

        function markAsDaDoc(id) {
            $('#<%=txtLineId.ClientID %>').val(id);
            <%=ClientScript.GetPostBackClientHyperlink(btnDaChuyen, "") %>
        }
    </script>
</asp:Content>
