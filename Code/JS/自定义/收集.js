1.访问父页面元素事件：
	①window.opener.document.getElementById("btnSearch").click();
	② $("#btnSearch", window.opener.document).click();

2.访问父页面元素事件：
	$("#btnSearch", window.opener.document);

3.访问父页面方法：
	window.opener.test(); //调用父页面方法
4.刷新父页面：
	window.opener.location.reload();
5.只能输入数字
6.获取下拉框选中文本：
	$("#ddlPlanMeetingKind option:selected").text()
7. 在父窗口中获取iframe中的元素:
	1).格式：$("#iframe的ID").contents().find("#iframe中的控件ID").click();//jquery 方法1
	2).格式：$("#iframe中的控件ID",document.frames("frame的name").document).click();//jquery 方法2 
8.在iframe中获取父窗口的元素:
	格式：$('#父窗口中的元素ID', parent.document).click(); 
9.获取下拉框text：
	$("#ddlJobGrade option:selected").text();
10.获取URL参数
	function getQueryString(name) {
		var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
		var r = window.location.search.substr(1).match(reg);
		if (r != null) return unescape(r[2]); return null;
	}
11.正则表达式js验证
	//正则表达式验证是否是标准颜色值
    $("#txtTicketColor").change(function () {
        var txt = $(this).val();
        if (/^#[0-9|a-f|A-F]{6}$/.test(txt) == false) {
            $(this).focus();
            alert("请输入标准的6位颜色值");
            return false;
        } 
        $("#spTicketColor").css("background-color", $(this).val());
    });
12.获取颜色16进制值：
	$.fn.getHexBackgroundColor = function () {
		var rgb = $(this).css("background-color");
		if (rgb.indexOf('#')>=0) {
			return rgb;
		}
		rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
		function hex(x) { return ("0" + parseInt(x).toString(16)).slice(-2); }
		return rgb = "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
	};
13.弹出窗口居中：
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

居中显示：openWindow('PersonTemplateSave.aspx', '保存个人模板', 500, 100, screen.width/2 - 250, screen.height/2 - 50);

14.设置Dom对象显示最大文本，多出用...代替
////////////////////////////////////////////////////////
	//函数说明：设置对象最长文本，超出则用...代替
	//obj:需要显示文本的对象
	//maxLen设置最长文本
	//超出maxLen，则减少多少个字符用...代替
	//text需要显示的文本
	function SetMaxText(obj, maxLen, replaceNum, text) {
		var len = text.length;
		if (len > maxLen) {
			text = text.substring(0, maxLen - replaceNum) + "...";
		}
		$(obj).text(text);
	}

15.计算字符串的字节数：
    String.prototype.lenB = function () { 
		return this.replace(/[^\x00-\xff]/g, "**").length; 
	}
16.js提交
	setTimeout(__doPostBack($("#btnRebindControlData").attr("name"), ''),0);
17. C#后台获取URL参数乱码问题：js使用encodeURI(参数)即可；
18.保留两位小数：.toFixed(2)
19.用于设置最长文本：
//求字符串占用的字节数（中文2个字节，英文1一个字节，空格一个字节）
String.prototype.lenB = function () {
    return this.replace(/[^\x00-\xff]/g, "**").length;
}
20.
