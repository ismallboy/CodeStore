// ���λس���text�ύ
$('input').keydown(function (event) {
	if (event.keyCode == 13) {
		var target;
		if (!event) {
			target = event.srcElement;
		} else {
			target = event.target;
		}
		var tag = target.tagName;
		var inputType = $(target).attr("type");
		if (inputType == 'text' || inputType == "radio") {
			return false;
		} else {
			return true;
		}
	}
});

//��ȡUrl����
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}