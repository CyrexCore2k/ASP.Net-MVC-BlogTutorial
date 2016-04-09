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
            success: function (msg) {
                __URLCache(url, msg);
                if (successCallBack) successCallBack(msg);
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
            AppState().pushMessage("AJAXLoadData failed for url: " + url);
        }
    });
}