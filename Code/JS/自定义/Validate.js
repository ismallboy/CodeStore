//判断一个字符串是否是数字
function NumberExtend(str) {
    str = str.replace(/,/g, "");
    return Number(str);
}

//判断控件的值是否为空
function IsEmpty(elem) {
    if ($(elem)[0].tagName == "INPUT") {
        return $.trim($(elem).val()) === "";
    }
    else {
        return $.trim($(elem).text()) === "";
    }
}

//只允许输入整数（微软拼音输入法中文状态下可能不能输入数字，需加上：Style="ime-mode: disabled;"）
function OnlyIntegerValue() {
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}

//只允许输入小数
function OnlyFloatValue() {
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39) && !(event.keyCode == 190) && !(event.keyCode == 110))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}

//判断是否是整数
function IsInt(str) {
    var reg = /^[0-9]*$/;
    return reg.test(str);
}

//判断是否是小数
function IsFloat(str) {
    if (IsInt(str))
        return true;
    var reg = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
    return reg.test(str);
}

/******************身份证校验方法 *****************************/
CommonFunction.Append_Zore = function (temp) {
    if (temp < 10) {
        return "0" + temp;
    }
    else {
        return temp;
    }
}
CommonFunction.DentityCardChecked = function (person_id) {
    var aCity = {
        11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海",
        32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南",
        44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西",
        62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外"
    };

    var sum = 0;
    var birthday;
    var currentYear = new Date().getFullYear();
    var birthFullYear;
    var pattern = new RegExp(/(^\d{17}(\d|x|X)$)/i);

    if (person_id == "" || person_id == null) {
        alert("请输入身份证号码");
        return false;
    }

    if (pattern.exec(person_id)) {
        person_id = person_id.replace(/x|X$/i, "a");
        //获取18位证件号中的出生日期
        birthFullYear = person_id.substring(6, 10);
        birthday = person_id.substring(6, 10) + "-" + person_id.substring(10, 12) + "-" + person_id.substring(12, 14);
        //校验18位身份证号码的合法性
        for (var i = 17; i >= 0; i--) {
            sum += (Math.pow(2, i) % 11) * parseInt(person_id.charAt(17 - i), 11);
        }
        if (sum % 11 != 1) {
            alert("身份证号码不符合国定标准，请核对！");
            return false;
        }


        //检测证件地区的合法性								
        if (aCity[parseInt(person_id.substring(0, 2))] == null) {
            alert("身份证地区未知，请核对！");
            return false;
        }
        var dateStr = new Date(birthday.replace(/-/g, "/"));

        if (birthday != (dateStr.getFullYear() + "-" + CommonFunction.Append_Zore(dateStr.getMonth() + 1) + "-" + CommonFunction.Append_Zore(dateStr.getDate()))) {
            alert("身份证出生日期非法！");
            return false;
        }

        var yearValids = parseInt(currentYear) - parseInt(birthFullYear);
        if (yearValids > 200 || yearValids < 0) {
            alert("身份证年龄超出规定的范围，请核对");
            return false;
        }
    } else {
        alert("输入有误，请再次核对身份证号码");
        return false;
    }
    return true;

}
/******************身份证校验方法 *****************************/

//检验英文
CommonFunction.IsEn = function (input) {
    input = $(input);
    var value = input.val();
    if (value != null || value != "") {
        var pattern = "^[A-Za-z]+$";
        var regExp = new RegExp(pattern); //new RegExp(pattern, attributes);  如 new RegExp("[R|n]","gi") 
        return regExp.test(value);
    }
    return true;
}

//校验中文
CommonFunction.IsChinese = function (input) {
    //input = $(input);
    var value = input;
    if (value != null || value != "") {
        var pattern = "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$";
        var regExp = new RegExp(pattern); //new RegExp(pattern, attributes);  如 new RegExp("[R|n]","gi") 
        return regExp.test(value);
    }
    return true;
}

//JSON排序
//函数功能：json 排序  
// filed:(string)排序字段，  
// reverse: (bool) 是否倒置(是，为倒序)  
// primer (parse)转换类型  
var SortBy = function (filed, reverse, primer) {
    reverse = (reverse) ? -1 : 1;
    return function (a, b) {
        a = a[filed];
        b = b[filed];

        if (typeof (primer) != "undefined") {
            a = primer(a);
            b = primer(b);
        }

        if (a < b) {
            return reverse * -1;
        }
        if (a > b) {
            return reverse * 1;
        }
        return 0;
    }
}
// 调用
function JsonSort(jsonList, fieldName, asc, dataType) {
    jsonList.sort(SortBy(fieldName, asc, dataType));
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