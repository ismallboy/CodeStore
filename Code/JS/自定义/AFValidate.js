/// <reference path="jquery-1.4.1.js" />
/// <reference path="jquery-1.4.1-vsdoc.js" />

$(document).ready(function () {
    // 绑定控件
    AFValidate.InitValidate();
});
var AFValidate = {}

/* 验证表单 */
AFValidate.Validate = function (group, alertMsg, elFocus, param) {
    ///	<summary>
    ///	    执行页面中带有validateMethod属性的input元素中的validateMethod属性指定的方法
    ///     如果有任何方法执行返回false，则验证失败，返回false，成功则返回true
    ///	</summary>
    ///	<param name="group" type="String">
    ///		验证范围,比如指定了div1，则只验证div1元素下面的input输入
    ///     可以用于局部验证
    ///	</param>
    ///	<param name="alertMsg" type="Boolean">
    ///		是否alert input元素中validateMsg属性中的信息
    ///     可以为input元素指定一个validateMsg属性，里面包含提示的默认信息
    ///	</param>
    ///	<param name="elFocus" type="Boolean">
    ///		焦点是否定位到input元素
    ///	</param>
    ///	<param name="param" type="Object">
    ///		用于传给回调validateMethod属性中方法的参数
    ///     validateMethod属性指定的方法中可以使用this来取得input元素，唯一一个参数就是此处指定的
    ///     例如，同意和退回按钮都需要验证，但是审批意见同意是非必填，退回是必填，
    ///     退回按钮的click事件可以传一个参数过来，验证方法可以能过此参数来判断是同意验证还是退回验证
    ///	</param>
    ///	<returns type="Boolean" />
    var els;
    if (elFocus == null) elFocus = true;
    if (group) {
        els = $('#' + group + ' input[validateMethod][validateMethod!=""],#' + group + ' textarea[validateMethod][validateMethod!=""]');
    } else {
        els = $('input[validateMethod][validateMethod!=""],textarea[validateMethod][validateMethod!=""]');
    }
    for (var i = 0; i < els.length; i++) {
        var el = els[i];
        var fn = $(el).attr('validateMethod');
        var result = eval(fn).call(el, param);
        if (result == false) {
            // 是否弹出窗口错误提示
            if (alertMsg) {
                var msg = $(el).attr('validateMsg');
                if (msg != undefined && msg != '') alert(msg);
            }
            //$(el).focus();
            if (elFocus) {
                $(el).select();
            }
            return false;
        }
    }
    return true;
}

/* 验证初始化,为需要验证的输入框绑定blur事件 */
AFValidate.InitValidate = function () {
    ///	<summary>
    ///	    为所有设置了validateMethod属性的input元素添加validateMethod属性中的方法到blur事件
    ///	</summary>
    var els = $('input[validateMethod][validateMethod!=""],textarea[validateMethod][validateMethod!=""]');
    $.each(els, function (i, v) {
        var fn = $(v).attr('validateMethod');
        var tagName = v.tagName.toLowerCase();
        var noticeClass = '';
        if (AFValidate.NoticeClass[tagName] != null && AFValidate.NoticeClass[tagName] != '') {
            noticeClass = AFValidate.NoticeClass[tagName];
        }
        if (fn.length > 0) {
            var fun = function () {
                var result = eval(fn).call(this);
                if (result == false) {
                    if (noticeClass != '') {
                        $(this).addClass(noticeClass);
                    }
                } else {
                    if (noticeClass != '') {
                        $(this).removeClass(noticeClass);
                    }
                }
            }
            $(v).unbind("blur", fun);
            $(v).blur(fun);
        }
    });
}

/* 用于验证页面上所有输入是否有非法字符 */
AFValidate.IllegalInputValidate = function () {
    // 不再验证
    return true;
    var els = $('input[type="text"]');
    var pat = new RegExp("[<>\/]", "i"); //("[^a-zA-Z0-9@\.\_\u4e00-\u9fa5]", "i");
    for (var i = 0; i < els.length; i++) {
        var el = $(els[i]);
        if (pat.test(el.val()) == true) {
            el.select();
            alert('输入出现非法字符');
            return false;
        }
    }
    els = $('textarea');
    for (var i = 0; i < els.length; i++) {
        var el = $(els[i]);
        if (pat.test(el.val()) == true) {
            el.select();
            alert('输入出现非法字符');
            return false;
        }
    }
    return true;
}

/* 验证是否为空 */
AFValidate.ValidateEmpty = function (str) {
    str = str.replace(/　/g, " ");
    str = $.trim(str);
    if (str == '') {
        return false;
    }
    else if (str == null) {
        return false;
    }
    else if (str == undefined) {
        return false;
    }
    return true;
}

/* 验证邮箱 */
AFValidate.ValidateEmail = function (str) {
    var reg = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return reg.test(str);
}

AFValidate.AddValidateNotice = function (el) {
    $(el).addClass('RedBorder');
}

AFValidate.RemoveValidateNotice = function (el) {
    $(el).removeClass('RedBorder');
}

/* 验证长度 */
AFValidate.ValidateLength = function (str, len) {
    // 如果为null或为空字符时，长度肯定小于len，验证通过
    if (AFValidate.ValidateEmpty(str) == false)
        return true;
    if (str.length <= len) {
        return true;
    }
    return false;
}

/* 验证日期格式 */
AFValidate.ValidateDateFormat = function (str) {
    try {
        var d = Date.parse(str);
    } catch (ex) {
        return false;
    }
    return true;
}


/* 限制输入只能输入数字的方法,在txtbox中加入onkeyup="clearNoNum(this)" */
AFValidate.clearNoNum = function (obj) {
    //先把非数字的都替换掉，除了数字和.
    obj.value = obj.value.replace(/[^\d]/g, "");
    //必须保证第一个为数字而不是.
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}

/* 限制输入只能输入金额的方法,在txtbox中加入onkeyup="clearNoMoney(this)" */
AFValidate.clearNoMoney = function (obj) {
    //先把非数字的都替换掉，除了数字和.
    obj.value = obj.value.replace(/[^\d.-]/g, "");
    //必须保证第一个为数字而不是.
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace("-", "$#$").replace(/-/g, "").replace("$#$", "-");
    /* 保证-只能出现在第一位 */
    if (obj.value.indexOf('-') > 1) {
        obj.value = obj.value.replace(/-/g, "");
    }
}

/* 隐藏按钮 */
AFValidate.Submiting = function (btnCode) {
    setTimeout(function () {
        var btn = $('input[ButtonFunGroup="' + btnCode + '"]');
        btn.attr('disabled', true);
    }, 10);
}

/* 警告提醒样式 */
AFValidate.NoticeClass = {};