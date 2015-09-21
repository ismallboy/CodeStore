/// <reference path="../jquery-1.4.1-vsdoc.js" />
var UpLoadData = {};

//初始化按钮
$(function () {
    UpLoadData.InitEvent();
});

UpLoadData.InitEvent = function () {
    //查看
    $("#lbtSearch").click(function () {
        var txt = $("#txtKey").val();
        if ($.trim(txt) == "") {
            alert("请填写活动编号活动名称");
            return false;
        }
        return true;
    });

    //移除活动
    $("#lbtDel").click(function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
            return false;
        }
        if (confirm("确认移除活动吗？")) {
            return true;
        } else {
            return false;
        }
    });

    //上传验票数据
    $("#lbtUpLoad").click(function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
            return false;
        }
    });

    //查看活动信息
    UpLoadData.btnView = function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
        } else {
            window.location.href = "ActivityInfo.aspx?RID=" + $("#lstAcivityList").val() + "&URL=UpLoadData.aspx";
        }
    }

}