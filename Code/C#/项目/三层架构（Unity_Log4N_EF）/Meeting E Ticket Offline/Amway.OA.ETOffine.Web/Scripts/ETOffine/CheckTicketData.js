/// <reference path="../jquery-1.4.1-vsdoc.js" />

var CheckTicketData = {};
$(function () {
    CheckTicketData.LoadInit();
    CheckTicketData.InitEvent();
});

CheckTicketData.LoadInit = function () {

};

CheckTicketData.InitEvent = function () {
    //验票
    $("#btnCheck").click(function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
            return false;
        }
        window.location.href = "CheckTicket.aspx?RID=" + $("#lstAcivityList").val();
        return false;
    });

    //监测验票情况
    $("#btnMonitor").click(function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
            return false;
        }
        window.location.href = "MonitorCheck.aspx?RID=" + $("#lstAcivityList").val();
        return false;
    });
    //查看活动信息
    $("#btnViewActivityInfo").click(function () {
        var selected = $("#lstAcivityList").val();
        if (selected == null) {
            alert("请选择一个活动");
            return false;
        }
        window.location.href = "ActivityInfo.aspx?RID=" + $("#lstAcivityList").val()+"&URL=CheckTicketData.aspx";
        return false;
    });    
};
