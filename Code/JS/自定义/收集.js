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