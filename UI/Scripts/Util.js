function AJAXLoadData(url, data, successCallBack) {
    $.ajax({
        type: "POST",
        data: data,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (successCallBack) successCallBack(msg);
        },
        error: function (msg) {
            alert("error.");
        }
    });
}