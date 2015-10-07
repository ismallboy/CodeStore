var ActivityInfo = {};
var returnURL = "";

//初始化按钮
$(function () {
    returnURL = getQueryString('URL');

    ActivityInfo.InitEvent();
});


ActivityInfo.InitEvent = function () {

    $("#reIndex").click(function () {
        window.open("../default.aspx", "_self");
    });
    $("#reBack").click(function () {
        window.open(returnURL, "_self");
    });
};


