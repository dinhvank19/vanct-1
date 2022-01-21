<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Site1.Master" AutoEventWireup="true" CodeBehind="DailyReport.aspx.cs" Inherits="Vanct.WebApp.Report.DailyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/MetroUI/min/iconFont.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/MetroUI/min/metro-bootstrap.min.css") %>" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Content/MetroUI/min/jquery.widget.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Content/MetroUI/min/metro.min.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="rightInfoContent" runat="server">
    <div id="btnReportFilter" class="btn-filter right"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div id="divReportMenuOverlay">
        <div id="divReportMenu" class="menu shadow" style="display: none;">
            <div class="title">Điều kiện</div>
            <div class="metro" style="padding: 7px;">
                <label>Từ ngày</label>
                <div class="input-control text" data-role="datepicker" data-format='dd-mm-yyyy' data-date='<%=DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") %>'>
                    <input type="text" id="txtDateFrom">
                    <button class="btn-date"></button>
                </div>
                <label>Đến ngày</label>
                <div class="input-control text" data-role="datepicker" data-format='dd-mm-yyyy' data-date='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'>
                    <input type="text" id="txtDateTo">
                    <button class="btn-date"></button>
                </div>
                <div class="input-control switch" data-role="input-control">
                    <label class="inline-block" style="margin-right: 20px">
                        Xem theo ca
                        <input type="checkbox" id="chkWorkMode"/>
                        <span class="check"></span>
                    </label>
                </div>
                <input type="button" value="Xem" onclick="ReportFilter();"/>
                <div style="clear: both;"></div>
            </div>
        </div>
    </div>
    <div class="_header-height50"></div>
    <div id="divContent"></div>
    <script type="text/javascript">
        var worker = new Worker('LoadData.js');
        worker.addEventListener('message', function (e) {
            var data = e.data;
            switch (data.cmd) {
                case 'ReportFilter':
                    ReceivedReportFilter(data.data);
                    break;
            }
        }, false);

        $('#divUsername').html($('#divUsername').text() + ' - ' + 'Theo ngày');
        var btnReportFilter = $('#btnReportFilter');
        var divReportMenu = $('#divReportMenu');
        divReportMenu.height($(window).height());
        btnReportFilter.unbind('click').bind('click', function () {
            $('#divReportMenuOverlay').lightbox_me({
                modalCSS: { top: 0, right: 0 },
                overlaySpeed: 50,
            });
            divReportMenu.show();

            if (GetFilterParams() != null) {
                filterParameter = GetFilterParams();
                $('#txtDateFrom').val(filterParameter.DateFrom);
                $('#txtDateTo').val(filterParameter.DateTo);
                $('#chkWorkMode').get(0).checked = filterParameter.WorkMode;
            }
        });

        var divContent = $('#divContent');
        var filterParameter = GetFilterParams();
        function ReportFilter() {
            filterParameter = {
                DateFrom: $('#txtDateFrom').val(),
                DateTo: $('#txtDateTo').val(),
                WorkMode: $('#chkWorkMode').get(0).checked
            };
            worker.postMessage({
                cmd: 'ReportFilter',
                url: '<%=ResolveUrl("~/Report/Worker.ashx") %>',
                filterParamter: JSON.stringify(filterParameter)
            });
            SetFilterParams(filterParameter);
        }

        function ReceivedReportFilter(data) {
            if (!data.Result) {
                alert(data.Message);
                return;
            }

            divContent.html('');
            if (filterParameter.WorkMode) {
                $(data.List).each(function(index, record) {
                    var item = $('<div class="work-date">' +
                        '<div class="title"><div class="date left">' + record.D + '</div><div class="total right">' + record.T + '</div><div class="clr"></div></div>' +
                        '</div>');
                    divContent.append(item);
                    if (record.L.length > 0) {
                        var table = $('<table></table>');
                        $(record.L).each(function(jndex, line) {
                            var row = $('<tr><td>' + line.W + '</td><td>' + line.U + '</td><td>' + line.T + '</td></tr>');
                            table.append(row);
                        });
                        item.append(table);
                    }
                });
            } else {
                var table = $('<table class="work-line">' +
                    (IsMobile
                        ? '<tr class="header-row"><td colspan="4" class="padding5">Từ ngày: ' + filterParameter.DateFrom + '</td></tr>' +
                            '<tr class="header-row"><td colspan="4" class="padding5">Đến ngày: ' + filterParameter.DateTo + '</td></tr>' +
                            '<tr class="header-row"><td colspan="4" class="padding5">Doanh thu: ' + data.Total + '</td></tr>'
                        : '<tr class="header-row"><td colspan="4" class="padding5"><div class="left">Từ ngày ' + filterParameter.DateFrom + ' đến ngày ' + filterParameter.DateTo + '</div><div class="right">Doanh thu: ' + data.Total + '</div><div class="clr"></div></td></tr>') +
                    '<tr class="header-row" style="font-weight: bold">' +
                    '<td>Sản Phẩm</td>' +
                    '<td>SL</td>' +
                    '<td>Giá</td>' +
                    '<td>T.Tiền</td>' +
                    '</tr>' +
                    '</table>');
                $(data.List).each(function (index, group) {
                    var groupRow = $('<tr class="header-row"><td colspan="4" class="text-center"><div class="left">Nhóm ' + group.G + '</div><div class="right">' + group.T + '</div><div class="clr"></div></td></tr>');
                    table.append(groupRow);
                    $(group.L).each(function (jndex, record) {
                        var row = $('<tr><td>' + record.N + '</td>' +
                            '<td class="text-center">' + record.A + '</td>' +
                            '<td class="text-right">' + record.P + '</td>' +
                            '<td class="text-right">' + record.T + '</td></tr>');
                        table.append(row);
                    });
                });

                
                divContent.append(table);
            }

            divContent.append($('<div class="clr"></div>'));
            $('.js_lb_overlay').trigger('click');
        }

        function GetFilterParams() {
            return localStorage.FilterParams
                ? JSON.parse(localStorage.FilterParams)
                : null;
        }

        function SetFilterParams(value) {
            localStorage.FilterParams = JSON.stringify(value);
        }
    </script>
</asp:Content>
