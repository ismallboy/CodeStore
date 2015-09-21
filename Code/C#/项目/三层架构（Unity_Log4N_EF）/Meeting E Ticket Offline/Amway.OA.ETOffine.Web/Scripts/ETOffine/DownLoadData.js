/// <reference path="../jquery-1.4.1-vsdoc.js" />
var DownLoadData = {};

//初始化按钮
$(function () {
    DownLoadData.InitEvent();
});

DownLoadData.InitEvent = function () {
    $("#lbtDel").click(function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
            return false;
        }
        if (!confirm("确认移除活动吗？")) {
            return false;
        } 
    });
}

DownLoadData.btnAdd = function () {
    window.location.href = "GetActivityFrame.aspx";
}

//查看活动信息
DownLoadData.btnView = function () {
    var selected = $("#lstAcivityList").val();
    if (selected == null) {
        alert("请选择一个活动");
    }
    else {
        window.location.href = "ActivityInfo.aspx?RID=" + $("#lstAcivityList").val() + "&URL=DownLoadData.aspx";
    }
}




