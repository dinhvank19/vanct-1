self.addEventListener('message', function(e) {
    var data = e.data;
    var xmlhttp = new XMLHttpRequest();
    if (data.cmd === 'cmdLoad') {
        try {
            xmlhttp.onreadystatechange = function() {
                if (xmlhttp.readyState === 4 && xmlhttp.status === 200) {
                    var result = JSON.parse(xmlhttp.responseText);

                    // send report data
                    self.postMessage({ cmd: 'cmdRender', data: result });
                }
            };
            xmlhttp.open('POST', data.url + '?action=' + data.action, true);
            xmlhttp.send();
        } catch (e) {
            console.log('can not use XMLHttpRequest object');
        }
    }

    if (data.cmd === 'ReportFilter') {
        try {
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState === 4 && xmlhttp.status === 200) {
                    var result = JSON.parse(xmlhttp.responseText);

                    // send report data
                    self.postMessage({ cmd: 'ReportFilter', data: result });
                }
            };
            xmlhttp.open('POST', data.url + '?action=' + data.cmd, true);
            xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xmlhttp.send('filterParamter=' + data.filterParamter);
        } catch (e) {
            console.log('can not use XMLHttpRequest object');
        }
    }
}, false);