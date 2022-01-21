<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportPart.ascx.cs" Inherits="Vanct.WebApp.Report.ReportPart" %>
<div id="divRightInfo" class="_header-margin50">
    <div class="right" style="text-align: left;">
        <div class="text">
            <div>Received:&nbsp;<span class="total" id="divTotal"></span></div>
            <div>Current:&nbsp;<span class="total" id="divTotalSoldOut"></span></div>
        </div>
    </div>
    <div class="left" style="text-align: left; margin-right: 30px;">
        <div class="text">
            <div>Busy/Tables:&nbsp;<span class="total" id="divCountTableBusy"></span></div>
            <div>Pending:&nbsp;<span class="total" id="divTableInProgress"></span></div>
        </div>
    </div>
    <div class="clr"></div>
</div>
<div id="divContent"></div>
<div id="divDetailsOverlay">
    <div id="divDetails" class="details shadow" style="display: none;"></div>
</div>
<script type="text/javascript">
    var worker = new Worker('LoadData.js');
    worker.addEventListener('message', function (e) {
        var data = e.data;
        switch (data.cmd) {
            case 'cmdLoad':
                postMessage();
                break;
            case 'cmdRender':
                render(data.data);
                break;
        }
    }, false);

    function postMessage() {
        worker.postMessage({
            cmd: 'cmdLoad',
            url: '<%=ResolveUrl("~/Report/Worker.ashx") %>',
            action: 'LoadReport'
        });
    }

    var divContent = $('#divContent');
    var divRightInfo = $('#divRightInfo');
    function render(data) {
        if (!data.Result) {
            alert(data.Message);
            window.location = '<%=ResolveUrl("~/Report/Default.aspx") %>';
            return;
        }
        divContent.html('');
        if ($(data.Tables).length === 0) {
            divContent.html('<table><tr><td><img src="<%=ResolveUrl("~/Content/Themes/Icons/Loading.gif") %>" /></td><td>Đang tải dữ liệu từ máy trạm, vui lòng chờ trong 30 giây ...</td></tr></table>');
        } else {
            $(data.Tables).each(function (index, record) {
                //var table = $('<div class="table left ' + (record.IsPrinted ? "printed" : "") + ' ' + (record.IsBusy ? "busy" : "") + '"><div class="name">' + record.TableNo + '</div><div class="total">' + (record.Total === 0 ? "" : record.Total) + '</div></div>');
                var table = $('<div class="table left ' +
                    (record.IsPrinted ? "printed " : " ") +
                    (record.Total > 0 && record.Processed && !record.IsPrinted ? "processed " : ' ') +
                    (record.IsBusy ? "busy " : " ") + '">' +
                    '<div class="name">' + record.TableNo + (record.Servicer && record.IsBusy ? ' - ' + record.Servicer : '') + 
                    (record.Moment ? ' [' + record.Moment + ']' : '') +
                    '</div>' +
                    '<div class="total">' + (record.Total === 0 ? '' : record.Total) + '</div>' +
                    '</div>');
                divContent.append(table);
                table.find('.total').formatCurrency({
                    roundToDecimalPlace: 0,
                    symbol: ''
                });
                if (record.Total > 0) {
                    table.unbind('click').bind('click', function () {
                        getTableline(record.TableNo);
                    });
                }
            });
            divContent.append($('<div class="clr"></div>'));
            divRightInfo.show();

            $('#divTotal').html(data.Total).formatCurrency({
                roundToDecimalPlace: 0,
                symbol: ''
            });

            $('#divTotalSoldOut').html(data.TotalSoldOut).formatCurrency({
                roundToDecimalPlace: 0,
                symbol: ''
            });

            $('#divCountTableBusy').text(data.CountTableBusy + '/' + data.Tables.length);
            $('#divTableInProgress').text(data.CountTableInProgress);
        }

        // re-send after 1 min
        if (data.Result) {
            setTimeout(function () { postMessage(); }, 30 * 1000);
        }
    }

    var divDetails = $('#divDetails');
    var divDetailsOverlay = $('#divDetailsOverlay');
    function getTableline(tableNo) {
        divDetails.html('');
        $.Hulk.CallbackRequestUrl('<%=ResolveUrl("~/Report/Worker.ashx?action=GetTableline") %>', 'json', { tableNo: tableNo }, function (rs) {
            if (!rs.Result) {
                alert(rs.Message);
                window.location = '<%=ResolveUrl("~/Report/Default.aspx") %>';
                return;
            }

            if ($(rs.Table.Lines).length === 0) {
                alert('Bàn trống');
                return;
            }

            var content = '<div class="line gray">' +
                '<div class="left"><b>' + rs.Table.TableNo + '</b></div>' +
                //'<div class="right"><b>' + (rs.Table.IsPrinted ? "ĐÃ IN BILL" : "ĐANG CÓ KHÁCH") + '</b></div>' +
                '<div class="right"><b>' + rs.Table.Moment + ' ago</b></div>' +
                '<div class="clr"></div></div>';

            //content += '<div class="line"><b>Bắt đầu</b>: ' + rs.Table.InDate.replace('T', ' ').substring(0, 16) + '</div>';
            content += '<div class="line"><b>Check In</b>: ' + rs.Table.InDate.replace('T', ' ').substring(0, 16) + '</div>';

            if (!IsMobile) {
                content += '<div class="line gray">' +
                (IsMobile ? '' : '<div class="daxuly"></div>') +
                '<div class="name"><b>Item</b></div>' +
                '<div class="amount"><b>Qty</b></div>' +
                (IsMobile ? '' : '<div class="moment"><b>Ago</b></div>') +
                '<div class="price"><b>Price</b></div>' +
                '<div class="price"><b>Total</b></div>' +
                '<div class="clr"></div></div>';
            }
                
            $(rs.Table.Lines).each(function (index, line) {
                if (IsMobile) {
                    content += '<div class="line2 ' + (index % 2 === 0 ? "" : "gray") + '"><div>' + line.ProductName + '</div><div class="secondline">' + line.Amout + ' x <span class="price">' + line.Price + '</span> = <span class="price">' + (line.Price * line.Amout) + '</span></div><div class="daxuly ' + (line.DaBao ? 'ico_checked' : '') + '"></div></div>';
                } else {
                    content += '<div class="line ' + (index % 2 === 0 ? "" : "gray") + '">' +
                    (IsMobile ? '' : '<div class="daxuly ' + (line.DaBao ? 'ico_checked' : '') + '"></div>') +
                    '<div class="name">' + line.ProductName + (IsMobile ? '<br/>(' + line.Moment + ')' : '') + (IsMobile && line.DaBao ? '<span class="daxuly ico_checked"></span>' : '') + '</div>' +
                    '<div class="amount">' + line.Amout + '</div>' +
                    (IsMobile ? '' : '<div class="moment"><span>' + line.Moment + '</span></div>') +
                    '<div class="price"><span>' + line.Price + '</span></div>' +
                    '<div class="price"><span>' + (line.Price * line.Amout) + '</span></div>' +
                    '<div class="clr"></div></div>';
                }
            });

            //content += '<div class="line discount">Giảm: <span>' + rs.Table.Discount + '</span></div>';
            content += '<div class="line">' + (IsMobile ? '<div class="btn-close"><input type="button" value="Closed" /></div>' : '') + '<div class="total">Total: <span>' + rs.Table.Total + '</span></div><div class="clr"></div></div>';

            divDetails.html(content).show();
            divDetails.find('.price span, .total span, .discount span, span.price').formatCurrency({
                roundToDecimalPlace: 0,
                symbol: ''
            });
            if (IsMobile) {
                var ww = $(window).width();
                var hh = $(window).height();

                divDetailsOverlay.css({ width: ww, height: hh - 42, position: 'fixed', top: 42, left: 0 }).addClass('auto-scroll').show();
                divDetails.width({ width: ww - 10 });
                divDetails.find('.line .name').width(ww - 180);
                $(document.body).addClass('hide-scroll');

                var btnClose = divDetails.find('.btn-close');
                btnClose.unbind('click').bind('click', function () {
                    divDetails.hide();
                    divDetailsOverlay.hide().removeClass('auto-scroll');
                    $(document.body).removeClass('hide-scroll');
                });
            } else {
                divDetailsOverlay.lightbox_me({
                    centered: true,
                    overlaySpeed: 50
                });
            }
        });
    }

    postMessage();
</script>