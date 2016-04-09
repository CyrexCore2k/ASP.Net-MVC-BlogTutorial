function __GetAppStateVariableName() {
    return "__state";
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