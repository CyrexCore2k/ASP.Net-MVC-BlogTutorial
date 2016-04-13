function AppState() {
	var dataName = __GetAppStateVariableName();
	var state = $.data(document, dataName);
	if(!state) {
	    var state = {
            pushMessage: function(msg) { this.messages.push(msg); },
	        messages: [],
	        urlCache: { count: 0, urls: [], cache: [] }
	    };
	    $.data(document, dataName, state);
	}
	return state;
}

function AJAXLoadHTML(url, data, successCallBack) {
    var cache = __URLCache(url);
    if (!cache) {
        $.ajax({
            type: "GET",
            data: data,
            url: url,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (msg, status, xhr) {
                var responseType = xhr.getResponseHeader("content-type") || "";
                if (responseType.toLowerCase().indexOf("html") > -1) {
                    __URLCache(url, msg);
                    if (successCallBack) successCallBack(msg);
                } else {
                    ProcessMessage($.parseJSON(msg));
                }
            },
            error: function (msg) {
                AppState().pushMessage("AJAXLoadHTML failed for url: " + url);
            }
        });
    } else {
        if (successCallBack) successCallBack(cache);
    }
}

function AJAXLoadData(url, data, successCallBack) {
    url = "http://localhost:56697" + url;

    $.ajax({
        type: "GET",
        data: data,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "jsonp",
        success: function (msg) {
            ProcessMessage(msg, successCallBack);
        },
        error: function (msg) {
            AppState().pushMessage("AJAXLoadData failed for url: " + url);
        }
    });
}

function ProcessMessage(msg, successCallBack) {
    if (msg.ServerExceptions.length > 0) {
        $.each(msg.ServerExceptions, function (index, item) {
            alert("Server encountered an error: " + item.Message);
        });
    } else if (msg.ClientExceptions.length > 0) {
        $.each(msg.ClientExceptions, function (index, item) {
            alert(item.Number + ": " + item.Source + " caused error " + item.Message + ", value: " + item.Value);
        });
    } else if (successCallBack) {
        successCallBack(msg);
    }
}