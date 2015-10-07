/// <reference path="../jquery-1.4.1-vsdoc.js" />
var checkTicketManage = {};

//初始化页面
$(function () {
    returnURL = getQueryString('URL');
    checkTicketManage.InitEvent();
});
checkTicketManage.InitEvent = function () {
    var time = $("#reLoadTime").val();
    setInterval(function () {
        window.location.reload();
    }, time);

    $("#reIndex").click(function () {
        window.open(returnURL, "_self");
    });

    $("#reBack").click(function () {
        window.open(returnURL, "_self");
    });
}