function AppState() {
	var dataName = __GetAppStateVariableName();
	var state = $.data(document, dataName);
	if(!state) {
	    state = {
	        pushMessage: function (msg) { this.messages.push(msg); },
	        messages: [],
	        urlCache: { count: 0, urls: [], cache: [] },
	        routes: {
	            base: "/Content/ui/",
	            defaultFolder: "home/",
	            defaultPage: "default",
	            masterPage: "_master",
	            extension: ".htm",
	            language: {
	                _current: "",
	                getFolder: function () {
	                    return (state.routes.language._current == "" ? 
                            "en" : state.routes.language._current) + "/"; 
                    },
	                val: function (language) {
	                    if (!language) return state.routes.language._current;
	                    state.routes.language._current = language;
	                    return state.routes.language._current;
	                }
	            },
	            getLanguageBase: function () {
	                return state.routes.base + state.routes.language.getFolder(); 
                }
	        },
	        workspaces: { root: "__workspace", separator: "\\.", loaded: [] }
	    };
	    $.data(document, dataName, state);
	}
	return state;
}

function AJAXLoadHTML(url, data, successCallBack, config) {
    if (!config) config = {};

    var processError = function (msg) {
        if (config.errorCallBack)
            config.errorCallBack(msg);
        else
            AppState().pushMessage("AJAXLoadHTML failed for url: " + url);
    };

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
                    __URLCache(url, { success: true, msg: msg });
                    if (successCallBack) successCallBack(msg);
                } else {
                    if (config.clientExceptionCallBack)
                        config.clientExceptionCallBack($.parseJSON(msg))
                    else
                        ProcessMessage($.parseJSON(msg));
                }
            },
            error: function (msg) {
                __URLCache(url, { success: false, msg: msg });
                processError(msg);
            }
        });
    } else if (!cache.success) {
        processError(cache.msg);
    } else {
        if (successCallBack) successCallBack(cache.msg);
    }
}

function AJAXLoadData(url, data, successCallBack) {
    url = "http://service.phase4.mvctutorial.netortech.com" + url;

    $.ajax({
        type: "GET",
        data: data,
        url: url,
        contentType: "application/javascript; charset=utf-8",
        dataType: "jsonp",
        success: function (msg) {
            ProcessMessage(msg, successCallBack);
        },
        error: function (msg) {
            AppState().pushMessage("AJAXLoadData failed for url: " + url);
        }
    });
}

function PassportLoadData(url, data, successCallBack, clientExceptionCallBack) {
    url = "http://phase3.linuxtutorial.netortech.com" + url;

    $.ajax({
        type: "GET",
        data: data,
        url: url,
        contentType: "application/javascript; charset=utf-8",
        dataType: "jsonp",
        success: function (msg) {
            ProcessMessage(msg, successCallBack, clientExceptionCallBack);
        },
        error: function (msg) {
            AppState().pushMessage("AJAXLoadData failed for url: " + url);
        }
    });
}

function ProcessMessage(msg, successCallBack, clientExceptionCallBack) {
    if (msg.ServerExceptions.length > 0) {
        $.each(msg.ServerExceptions, function (index, item) {
            alert("Server encountered an error: " + item.Message);
        });
    } else if (msg.ClientExceptions.length > 0) {
        if (!clientExceptionCallBack)
            $.each(msg.ClientExceptions, function (index, item) {
                alert(item.Number + ": " + item.Source + " caused error " + item.Message + ", value: " + item.Value);
            });
        else
            clientExceptionCallBack(msg);
    } else if (successCallBack) {
        successCallBack(msg);
    }
}

function Login(username, password, success, failure) {
    PassportLoadData("/Service/GetChallenge/", { Username: username }, function (msg) {
        var hash = SHA256(password + msg.Data.Challenge.Salt);
        var response = SHA256(hash + msg.Data.Challenge.Value);
        PassportLoadData("/Service/AuthenticateChallenge/", {
            Username: username,
            Response: response,
            ID: msg.Data.Challenge.Id
        }, function (msg) {
            success(msg);
        }, function (msg) {
            failure(msg);
        });
    });
}
