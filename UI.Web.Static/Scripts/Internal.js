function __GetAppStateVariableName() {
    return "__state";
}
function __Init() {
    $(window).hashchange(function () {
        AppState().hashInfo = __GetHashInfo();
        AppState().data = AppState().hashInfo.data;
        __Route();
    });
    $(window).hashchange();
}
function __URLCache(url, val) {

    var urlCache = AppState().urlCache;

    if (!val) {
        var ret;

        if (urlCache) {
            for (var index = 0; index < urlCache.count; index++) {
                if (urlCache.urls[index] == url) {
                    ret = urlCache.cache[index];
                    break;
                }
            }
        } else {
            ret = false;
        }

        if (ret)
            AppState().pushMessage("Cache for URL '" + url + "' found.");
        else
            AppState().pushMessage("No cache for URL '" + url + "' found.");

        return ret;
    } else {
        var index = -1;
        $.each(urlCache.urls, function (_index, item) { if (item == url) { index = _index; return false; } });
        if (index == -1) index = urlCache.count;

        urlCache.count++;
        urlCache.urls[index] = url;
        urlCache.cache[index] = val;
    }
}
function __GetHashInfo() {
    var info = {};

    if (window.location.hash && window.location.hash.length > 1) {
        info.hash = window.location.hash.substr(1, window.location.hash.length - 1);
        info.parts = info.hash.split("/");
        if (info.hash.indexOf("?") > -1) {
            info.nav = info.hash.substr(0, info.hash.indexOf("?"));
            if (info.hash.indexOf("?") + 1 < info.hash.length) {
                var datastr = info.hash.substr(info.hash.indexOf("?") + 1, info.hash.length - (info.hash.indexOf("?") + 1));
                var dataparts = datastr.split("&");
                info.data = { count: 0, vars: {} };
                $.each(dataparts, function (index, item) { info.data.count++; info.data.vars[item.split("=")[0]] = item.split("=")[1]; });
            }
        } else {
            info.nav = info.hash;
        }
    }
    return info;
}
function __Route() {
    var base = "/Content/ui";
    var resource = base;

    if (!AppState().hashInfo.nav) // default
    {
        AJAXLoadHTML(base + "/home/landing.htm", null, function (msg) { $("#workspace").html(msg); });
        return;
    }

    if (AppState().hashInfo.parts.length == 1)
        resource += "/home/" + AppState().hashInfo.parts[0];
    else {
        $.each(AppState().hashInfo.parts, function (index, item) {
            resource += "/" + item;
        });
    }

    AJAXLoadHTML(resource + ".htm", null, function (msg) { $("#workspace").html(msg); });
}
