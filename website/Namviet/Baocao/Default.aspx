<%@ Page Title="" Language="C#" MasterPageFile="~/Baocao/Report.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Namviet.Baocao.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Xem Doanh Số | NamvietKhanhHoa.com</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-2">
                <div class="form-group">
                    <label>Từ ngay</label>
                    <asp:TextBox autocomplete="false" CssClass="form-control datepicker" ID="txtFromDate" AutoCompleteType="None" runat="server" />
                </div>
                <div class="form-group">
                    <label>Đến ngày</label>
                    <asp:TextBox autocomplete="false" CssClass="form-control datepicker" ID="txtToDate" AutoCompleteType="None" runat="server" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-success" Text="Xem" ID="btnXem" OnClick="BtnXemClick" />
                    <br />
                    <br />
                    <asp:Button ID="RadButton2" runat="server" CssClass="btn-default btn" OnClick="RadButton2Click" Text="Thoát, đăng nhập lại" />
                </div>
            </div>
            <div class="col-md-10">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <%--<th>Ngày</th>--%>
                            <th>Mã Hàng</th>
                            <th>Tên Hàng</th>
                            <th>Số Lượng</th>
                            <th>Đơn Giá</th>
                            <th>Thành Tiền</th>
                            <%
                                if (Deleteable)
                                {
                            %>
                            <th style="width: 50px"></th>
                            <%
                                }
                            %>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="gridDoanhThu">
                            <ItemTemplate>
                                <tr>
                                    <%--<td><%#Eval("CreatedDateAsText") %></td>--%>
                                    <td><%#Eval("Code") %></td>
                                    <td><%#Eval("Name") %></td>
                                    <td><%#Eval("Amount") %></td>
                                    <td><strong><%#Eval("PriceAsText") %></strong></td>
                                    <td><strong><%#Eval("TotalAsText") %></strong></td>
                                    <%
                                        if (Deleteable)
                                        {
                                    %>
                                    <td>
                                        <a href="javascript: deleteRecord('<%#Eval("Id") %>');" class="btn btn-sm btn-danger">xoá</a>
                                    </td>
                                    <%
                                        }
                                    %>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="6" class="text-right" style="margin: 50px;">
                                <strong>Tổng cộng 
                                    <asp:Label runat="server" ID="lblTotalReport" />
                                </strong>
                            </td>
                        </tr>
                    </tfoot>
                </table>
                <%
                    if (ChangePasswordable)
                    {
                %>
                <div class="text-right">
                    <div class="form-group">
                        <asp:HyperLink runat="server" ID="linkChangePassword" NavigateUrl="ChangePassword.aspx" CssClass="btn btn-warning" Text="Đổi mật khẩu" />
                    </div>
                </div>
                <%
                    }
                %>
            </div>
        </div>
    </div>
    <div class="hidden">
        <asp:TextBox runat="server" ID="txtDeletingRecordId" />
        <asp:Button runat="server" ID="btnDelete" OnClick="BtnDeleteOnClick" />
    </div>
    <script>
        function deleteRecord(id) {
            if (confirm('Xoa khong?')) {
                $('#<%= txtDeletingRecordId.ClientID %>').val(id);
                $('#<%= btnDelete.ClientID %>').click();
            }
        }
    </script>
</asp:Content>
