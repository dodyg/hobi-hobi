var common;
(function (common) {
    function say() {
        alert('hello world');
    }
    common.say = say;
})(common || (common = {}));
