//�ж�һ���ַ����Ƿ�������
function NumberExtend(str) {
    str = str.replace(/,/g, "");
    return Number(str);
}

//�жϿؼ���ֵ�Ƿ�Ϊ��
function IsEmpty(elem) {
    if ($(elem)[0].tagName == "INPUT") {
        return $.trim($(elem).val()) === "";
    }
    else {
        return $.trim($(elem).text()) === "";
    }
}

//ֻ��������������΢��ƴ�����뷨����״̬�¿��ܲ����������֣�����ϣ�Style="ime-mode: disabled;"��
function OnlyIntegerValue() {
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}

//ֻ��������С��
function OnlyFloatValue() {
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39) && !(event.keyCode == 190) && !(event.keyCode == 110))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}

//�ж��Ƿ�������
function IsInt(str) {
    var reg = /^[0-9]*$/;
    return reg.test(str);
}

//�ж��Ƿ���С��
function IsFloat(str) {
    if (IsInt(str))
        return true;
    var reg = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
    return reg.test(str);
}

/******************���֤У�鷽�� *****************************/
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
        11: "����", 12: "���", 13: "�ӱ�", 14: "ɽ��", 15: "���ɹ�", 21: "����", 22: "����", 23: "������", 31: "�Ϻ�",
        32: "����", 33: "�㽭", 34: "����", 35: "����", 36: "����", 37: "ɽ��", 41: "����", 42: "����", 43: "����",
        44: "�㶫", 45: "����", 46: "����", 50: "����", 51: "�Ĵ�", 52: "����", 53: "����", 54: "����", 61: "����",
        62: "����", 63: "�ຣ", 64: "����", 65: "�½�", 71: "̨��", 81: "���", 82: "����", 91: "����"
    };

    var sum = 0;
    var birthday;
    var currentYear = new Date().getFullYear();
    var birthFullYear;
    var pattern = new RegExp(/(^\d{17}(\d|x|X)$)/i);

    if (person_id == "" || person_id == null) {
        alert("���������֤����");
        return false;
    }

    if (pattern.exec(person_id)) {
        person_id = person_id.replace(/x|X$/i, "a");
        //��ȡ18λ֤�����еĳ�������
        birthFullYear = person_id.substring(6, 10);
        birthday = person_id.substring(6, 10) + "-" + person_id.substring(10, 12) + "-" + person_id.substring(12, 14);
        //У��18λ���֤����ĺϷ���
        for (var i = 17; i >= 0; i--) {
            sum += (Math.pow(2, i) % 11) * parseInt(person_id.charAt(17 - i), 11);
        }
        if (sum % 11 != 1) {
            alert("���֤���벻���Ϲ�����׼����˶ԣ�");
            return false;
        }


        //���֤�������ĺϷ���								
        if (aCity[parseInt(person_id.substring(0, 2))] == null) {
            alert("���֤����δ֪����˶ԣ�");
            return false;
        }
        var dateStr = new Date(birthday.replace(/-/g, "/"));

        if (birthday != (dateStr.getFullYear() + "-" + CommonFunction.Append_Zore(dateStr.getMonth() + 1) + "-" + CommonFunction.Append_Zore(dateStr.getDate()))) {
            alert("���֤�������ڷǷ���");
            return false;
        }

        var yearValids = parseInt(currentYear) - parseInt(birthFullYear);
        if (yearValids > 200 || yearValids < 0) {
            alert("���֤���䳬���涨�ķ�Χ����˶�");
            return false;
        }
    } else {
        alert("�����������ٴκ˶����֤����");
        return false;
    }
    return true;

}
/******************���֤У�鷽�� *****************************/

//����Ӣ��
CommonFunction.IsEn = function (input) {
    input = $(input);
    var value = input.val();
    if (value != null || value != "") {
        var pattern = "^[A-Za-z]+$";
        var regExp = new RegExp(pattern); //new RegExp(pattern, attributes);  �� new RegExp("[R|n]","gi") 
        return regExp.test(value);
    }
    return true;
}

//У������
CommonFunction.IsChinese = function (input) {
    //input = $(input);
    var value = input;
    if (value != null || value != "") {
        var pattern = "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$";
        var regExp = new RegExp(pattern); //new RegExp(pattern, attributes);  �� new RegExp("[R|n]","gi") 
        return regExp.test(value);
    }
    return true;
}

//JSON����
//�������ܣ�json ����  
// filed:(string)�����ֶΣ�  
// reverse: (bool) �Ƿ���(�ǣ�Ϊ����)  
// primer (parse)ת������  
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
// ����
function JsonSort(jsonList, fieldName, asc, dataType) {
    jsonList.sort(SortBy(fieldName, asc, dataType));
}

/* ��������ֻ���������ֵķ���,��txtbox�м���onkeyup="clearNoNum(this)" */
AFValidate.clearNoNum = function (obj) {
    //�Ȱѷ����ֵĶ��滻�����������ֺ�.
    obj.value = obj.value.replace(/[^\d]/g, "");
    //���뱣֤��һ��Ϊ���ֶ�����.
    obj.value = obj.value.replace(/^\./g, "");
    //��ֻ֤�г���һ��.��û�ж��.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //��֤.ֻ����һ�Σ������ܳ�����������
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}

/* ��������ֻ��������ķ���,��txtbox�м���onkeyup="clearNoMoney(this)" */
AFValidate.clearNoMoney = function (obj) {
    //�Ȱѷ����ֵĶ��滻�����������ֺ�.
    obj.value = obj.value.replace(/[^\d.-]/g, "");
    //���뱣֤��һ��Ϊ���ֶ�����.
    obj.value = obj.value.replace(/^\./g, "");
    //��ֻ֤�г���һ��.��û�ж��.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //��֤.ֻ����һ�Σ������ܳ�����������
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace("-", "$#$").replace(/-/g, "").replace("$#$", "-");
    /* ��֤-ֻ�ܳ����ڵ�һλ */
    if (obj.value.indexOf('-') > 1) {
        obj.value = obj.value.replace(/-/g, "");
    }
}