/// <reference path="../jquery-1.4.1-vsdoc.js" />
/// <reference path="../json2.js" />

var CheckTicket = {};
var returnURL = "";

//初始化按钮
$(function () {
    returnURL = getQueryString('URL');

    CheckTicket.InitEvent();

//    window.onload = function () {
//        setTimeout(function () {
//            var wsh = new ActiveXObject("WScript.Shell");
//            wsh.sendKeys("{F11}");
//        }, 200);
//    }

//    window.onbeforeunload = function () {
//        var wsh = new ActiveXObject("WScript.Shell");
//        wsh.sendKeys("{F11}");
//    };

    document.onselectstart = function () {
        window.event.returnValue = false;
        return false;
    };
    document.oncontextmenu = function () {
        window.event.returnValue = false;
        return false;
    };
});

var _CanChecking = true;

CheckTicket.InitEvent = function () {
    $("#txtTicket").focus();
    $("#reIndex").click(function () {
        window.open(returnURL, "_self");
    });
    $("#reBack").click(function () {
        window.open(returnURL, "_self");
    });
    $("#btnOK").click(function () {
        if (_CanChecking) {
            CheckTicket.Check();
            //$("#txtTicket").focus();
        }
        return false;
    });
    $(document).keypress(function (el) {
        if (el.keyCode == 13) {
            if (_CanChecking) {
                CheckTicket.Check();
                //$("#txtTicket").focus();
            }
            else {
                $("#txtTicket").val('');
                $("#txtTicket").focus();
            }
            return false;
        };
    });

    setInterval(function () {
        var now = new Date();
        var hh = now.getHours();
        var mm = now.getMinutes();
        var ss = now.getSeconds();
        if (hh < 10) {
            hh = "0" + hh;
        }
        if (mm < 10) {
            mm = "0" + mm;
        }
        if (ss < 10) {
            ss = "0" + ss;
        }
        $("span[class='checkTime']").text(hh + ":" + mm + ":" + ss)
    }, 1000);

    $("#txtTicket").blur(function () {
        setTimeout(function () {
            $("#txtTicket").focus();
        }, 100);
    });
}

var _IsGotoAlert = false;
var _setTimeoutCheckRestore = 0;
//验票
CheckTicket.Check = function () {
    clearTimeout(_setTimeoutCheckRestore);

    if ($.trim($("#txtTicket").val()) == "") {
        return false;
    };

    $("#txtTicket").val(CheckTicket.GetAdaCardFromECard($.trim($("#txtTicket").val())));

    $("#txtTicket").attr("disabled", true);
    _CanChecking = false;
    _IsGotoAlert = false;

    var p = {};
    p.activityID = getQueryString("RID");
    p.ticketNumber = $.trim($("#txtTicket").val());
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: ETOffineServiceUrl + "/CheckTicket",
        data: JSON.stringify(p),
        timeout: 10000,
        dataType: 'json',
        async: true,
        success: function (result) {
            if (result == null) {
                if (_IsGotoAlert == false) {
                    alert("主机不可用或已断开，返回多机验票。");
                }
                window.open(returnURL, "_self");
            }
            else if (result.d.Result == 1) {
                $("#AutoBox").attr("class", "checkbox-result-yes");
                $("#ResultTxt").html(result.d.Message);
            }
            else if (result.d.Result == 2) {
                alert("主机不可用或已断开，返回多机验票。");
                window.open(returnURL, "_self");
            }
            else {
                $("#AutoBox").attr("class", "checkbox-result-no");
                $("#ResultTxt").html(result.d.Message);
            }

            _setTimeoutCheckRestore = setTimeout(function () {
                _CanChecking = true;
                $("#txtTicket").attr("disabled", false);
                $("#txtTicket").val('');
                $("#txtTicket").focus();
            }, 500);
        },
        error: function (er) {
            alert("主机不可用或已断开，返回多机验票。");
            _IsGotoAlert = true;
            window.open(returnURL, "_self");
        }
    });
};


CheckTicket.GetAdaCardFromECard = function (eCard) {
    /*             
    授权卡确定下来的二维码规则如下：
    授权标识位+悦享分标识位+授权人区号+授权人安利卡号+被授权人区号+被授权人安利卡号+ID+到期时间（YYYYMMDDHHMMSS）
    Ø  到期时间：按二维码生成时间加60分钟，格式为YYYYMMDDHHMMSS，其中生成时间按服务器时间。
    Ø  授权标识位：1位，0表示本人安利卡，1表示授权卡
    Ø  悦享分标识位：1位，本人安利卡以0表示，1表示授权不允许兑换悦享分，2表示授权允许兑换悦享分；
    Ø  授权人区号：3位，不满3位前面补零；
    Ø  授权人安利卡号：；11位，不满11位前面补零；
    Ø  被授权人区号（本人二维码以全“0”表示）：3位，不满3位前面补零；
    Ø  被授权人安利卡号（本人二维码以全“0”表示）：11位，不满11位前面补零；
    Ø  ID（本人二维码以 全“0”表示）：18位，授权记录在数据库所对应的ID。
 
    例子数据：授权人卡号12345，被授权人卡号67890，
    例子：
    卡号为12345的本人卡：OO360000000123450000000000000000000000000000000020150516121212
    他人授权卡（允许兑换悦享分）：12360000000123453600000006789000000000000000000220150516121212
    */
    var reVal = "";
    if (eCard.length >= 62) {
        if (eCard.substr(0, 1) == "0") {
            reVal = parseInt(eCard.substr(5, 11)) + "";
        }
        else {
            // 非本人安利卡
        }
    }
    else {
        reVal = eCard;
        // 卡长度不正确
    }
    return reVal;
};