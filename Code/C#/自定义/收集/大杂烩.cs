1.后台刷新界面：
	①//获取成功后关闭窗口并刷新父窗口
	string myScript = "<script>window.opener.location.href = window.opener.location.href;  if (window.opener.progressWindow) { window.opener.progressWindow.close(); }  window.close();</script>";
	Page.ClientScript.RegisterStartupScript(typeof(string), "closeWindowAndRefresh", myScript);
	②response.redirect("");
2.WebService允许访问session
	[WebMethod(EnableSession=true)]
3.asp.net webform 获取模板页控件
    ((TextBox)Master.FindControl("txtMsgTest")).Text;
4.<%#Eval("AdjustScore","{0:N2}") %>

5.校验最长文本：
	//求字符串占用的字节数（中文2个字节，英文1一个字节，空格一个字节）
	System.Text.Encoding.Default.GetBytes(txtTicketTitle.Text.Trim()).Length > 18