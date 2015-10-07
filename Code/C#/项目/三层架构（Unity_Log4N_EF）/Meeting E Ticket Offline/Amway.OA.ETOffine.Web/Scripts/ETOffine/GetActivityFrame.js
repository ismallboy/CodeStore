// <reference path="../jquery-1.4.1-vsdoc.js" />
var GetActivityFrame = {};

//初始化按钮
$(function () {
    GetActivityFrame.InitEvent();
    setTimeout(function () {
        GetActivityFrame.ReSize();
    }, 100);
});

GetActivityFrame.InitEvent = function () {
    var client = GetWinClient();
    var url = $("#hidQueryUrl").val() + "?Online=0&ProxyUrl=" + encodeURIComponent(window.location.href.toLowerCase().replace("getactivityframe.aspx", "Proxy.htm"));
    var ifr = document.createElement("iframe");
    ifr.id = "ifrQueryActivity";
    ifr.style.width = "100%";
    //ifr.style.height = client.H + "px";
    ifr.frameBorder = 0;
    ifr.scrolling = "auto";
    ifr.src = url;
    //ifr.onload = "Javascript:GetActivityFrame.SetWinHeight(this);"
    document.getElementById("divIFrame").appendChild(ifr);
}

GetActivityFrame.CallBack = function (json) {
    //空字符串是点击关闭按钮回传的信息
    if (json.Message != "") {
        window.location.href = "DownLoadData.aspx?rid=" + json.Message;
    } else {
        window.location.href = "DownLoadData.aspx";
    }
}

GetActivityFrame.SetWinHeight = function (h) {
    var a_iframe = parent.parent.document.getElementById("ifrQueryActivity");
    a_iframe.height = h + "px";

}

GetActivityFrame.ReSize = function () {
    document.getElementById("ifrQueryActivity").style.height = (document.documentElement.clientHeight) - 5 + 'px';
    document.getElementsByTagName('body')[0].style.height = (document.documentElement.clientHeight) + 'px';
}
window.onresize = function () {
    GetActivityFrame.ReSize();
}

function GetWinClient() //函数：获取尺寸
{
    //获取窗口宽度
    if (window.innerWidth)
        winWidth = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        winWidth = document.body.clientWidth;
    //获取窗口高度
    if (window.innerHeight)
        winHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        winHeight = document.body.clientHeight;

    //通过深入Document内部对body进行检测，获取窗口大小
    if (document.documentElement && document.documentElement.clientHeight &&
                                         document.documentElement.clientWidth) {
        winHeight = document.documentElement.clientHeight;
        winWidth = document.documentElement.clientWidth;
    }
    //结果输出至两个文本框

    return { H: winHeight, W: winHeight };
}


