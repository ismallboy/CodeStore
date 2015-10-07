//判断一个字符串是否是数字
function NumberExtend(str) {
    str = str.replace(/,/g, "");
    return Number(str);
}
//判断控件的值是否为空
function IsEmpty(elem) {
    if ($(elem)[0].tagName == "INPUT" || $(elem)[0].tagName == "SELECT" || $(elem)[0].tagName == "TEXTAREA") {
        return $.trim($(elem).val()) === "";
    }
    else {
        return $.trim($(elem).text()) === "";
    }
}

function OnlyIntegerValue() {
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}

function OnlyFloatValue() {
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39) && !(event.keyCode == 190) && !(event.keyCode == 110))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}

function IsInt(str) {
    var reg = /^[0-9]*$/;
    return reg.test(str);
}

function IsFloat(str) {
    if (IsInt(str))
        return true;
    var reg = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
    return reg.test(str);
};

//去千分符
function commafyback(num) { 
    var x = num.split(','); 
    return parseFloat(x.join("")); 
};
//加千分符
function commafy(num) {
    var r = /(\d+)(\d{3})/;
    while (r.test(num)) {
        num = num.replace(r, '$1' + ',' + '$2');
    }
    return num; 
};
function EnableButton() {
    setTimeout(function () {
        var btn = $('input[ButtonFunGroup="ButtonContainer"]');
        btn.attr('disabled', false);
    }, 10);
}
function DisableButton() {
    setTimeout(function () {
        var btn = $('input[ButtonFunGroup="ButtonContainer"]');
        btn.attr('disabled', true);
    }, 10);
}

function SubString(obj, len, msg) {
    if (obj.value.length > len) {
        alert(msg);
        obj.value = obj.value.substring(0, len);
    }
}

function ValidateTextArea(obj) {
    var txtLength = $(obj).val().length;
    var maxLength = $(obj).attr("maxLength");

    if (maxLength > 0 && txtLength > maxLength) {
        alert("文本过长，不能大于" + $(obj).attr("maxLength"));
        $(obj).focus();
        return false;
    }
}

function AddDays(date, days) {
    var nd = new Date(date);
    nd = nd.valueOf();
    nd = nd + days * 24 * 60 * 60 * 1000;
    nd = new Date(nd);
    //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日"); 
    var y = nd.getFullYear();
    var m = nd.getMonth() + 1;
    var d = nd.getDate();
    if (m <= 9) m = "0" + m;
    if (d <= 9) d = "0" + d;
    var cdate = y + "-" + m + "-" + d;
    return cdate;
}

function DateDiff(d1, d2) {
    var day = 24 * 60 * 60 * 1000;
    try {
        var dateArr = d1.split("-");
        var checkDate = new Date();
        checkDate.setFullYear(dateArr[0], dateArr[1] - 1, dateArr[2]);
        var checkTime = checkDate.getTime();

        var dateArr2 = d2.split("-");
        var checkDate2 = new Date();
        checkDate2.setFullYear(dateArr2[0], dateArr2[1] - 1, dateArr2[2]);
        var checkTime2 = checkDate2.getTime();

        var cha = (checkTime2 - checkTime) / day;
        return cha;
    } catch (e) {
        return null;
    }
}

//获取url参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};

////////////////////////////////////////////////////////
// 函数名称：openDialog 
// 功能说明：放大镜的弹出窗
///////////////////////////////////////////////////////
function openDialog(sURL, vArguments, w, h) {
    var sFeatures = "dialogWidth: " + w + "px;";
    sFeatures += "dialogHeight: " + h + "px;";
    sFeatures += "toolbar: no;";
    sFeatures += "location: no;";
    sFeatures += "status: no;";
    sFeatures += "menubar: no;";
    sFeatures += "scrollbars: no;";
    sFeatures += "resizable: yes";
    return window.showModalDialog(sURL, vArguments, sFeatures);
}

////////////////////////////////////////////////////////
// 函数名称：Open Window
// 功能说明：弹出窗
///////////////////////////////////////////////////////
function openWindow(sURL, sName, w, h, left, top) {
    var sFeatures = "toolbar = no,";
    sFeatures += "location = no,";
    sFeatures += "status = no,";
    sFeatures += "menubar = no,";
    sFeatures += "scrollbars = no,";
    sFeatures += "resizable = yes,";
    sFeatures = "width = " + w + ",";
    sFeatures += "height = " + h + ",";
    sFeatures += "left = " + left + ",";
    sFeatures += "top = " + top;
    window.open(sURL, sName, sFeatures, true);
}

//获取颜色16进制值
$.fn.getHexBackgroundColor = function () {
    var rgb = $(this).css("background-color");
    if (rgb.indexOf('#') >= 0) {
        return rgb;
    }
    rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    function hex(x) { return ("0" + parseInt(x).toString(16)).slice(-2); }
    return rgb = "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
};