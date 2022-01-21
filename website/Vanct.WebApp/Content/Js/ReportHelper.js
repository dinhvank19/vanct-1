var IsMobile = false;
if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/webOS/i)
        || navigator.userAgent.match(/iPhone/i)
    //|| navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/iPod/i)
        || navigator.userAgent.match(/BlackBerry/i)
        || navigator.userAgent.match(/Windows Phone/i)
) {
    IsMobile = true;
}

$.Hulk = {

    CallbackRequestUrl: function (url, returnType, postData, callback) {
        $.ajax({
            url: url,
            type: "POST",
            cache: false,
            data: postData,
            dataType: returnType,
            success: eval(callback)
        });
    },

    /*
    url: test.aspx
    returnType: json, xml, text, html
    postData: {name: 'hulk.vn', address: '123'}
    */
    RequestUrl: function (url, returnType, postData) {
        var oRes = null;
        $.ajax({
            url: url,
            type: "POST",
            async: false,
            cache: false,
            data: postData,
            dataType: returnType,
            success: function (result) {
                oRes = result;
            }
        });
        return oRes;
    }
};