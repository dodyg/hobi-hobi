var common;
(function (common) {
    function say() {
        alert('hello world');
    }
    common.say = say;
    function PostJson(url, json, done) {
        $.ajax(url, {
            data: json,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        }).done(done);
    }
    common.PostJson = PostJson;
})(common || (common = {}));

