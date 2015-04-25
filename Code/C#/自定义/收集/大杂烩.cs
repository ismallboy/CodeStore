1.后台刷新界面：
	①//获取成功后关闭窗口并刷新父窗口
	string myScript = "<script>window.opener.location.href = window.opener.location.href;  if (window.opener.progressWindow) { window.opener.progressWindow.close(); }  window.close();</script>";
	Page.ClientScript.RegisterStartupScript(typeof(string), "closeWindowAndRefresh", myScript);
	②response.redirect("");
2.WebService允许访问session
	[WebMethod(EnableSession=true)]